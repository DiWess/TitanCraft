#!/usr/bin/env python3
"""Behavioral tests for Agent Studio ownership rights resolution and enforcement.

Asserts behavior, not file existence: every negative case mutates a throwaway
copy of the repository governance tree and requires the validator to fail.
"""
from pathlib import Path
import re
import shutil
import subprocess
import sys
import tempfile

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / "tools"))

from agent_ownership import (  # noqa: E402
    load_entries,
    parse_ownership,
    pattern_to_regex,
    resolve,
    specificity,
)

failures: list[str] = []


def check(condition: bool, message: str) -> None:
    if not condition:
        failures.append(message)


# --- glob translation -------------------------------------------------------

def test_pattern_matching():
    deep = pattern_to_regex("src/Player/**")
    check(bool(deep.match("src/Player/PlayerController.cs")), "deep glob must match a nested file")
    check(bool(deep.match("src/Player/Sub/Dir/File.cs")), "deep glob must match any depth")
    check(not deep.match("src/Enemies/Scout.cs"), "deep glob must not match a sibling directory")
    check(not deep.match("src/PlayerExtras/File.cs"), "deep glob must not match a prefix directory")

    exact = pattern_to_regex("README.md")
    check(bool(exact.match("README.md")), "exact pattern must match itself")
    check(not exact.match("docs/README.md"), "exact root pattern must not match a nested file")

    single = pattern_to_regex("docs/*.md")
    check(bool(single.match("docs/asset-policy.md")), "single-star must match within one segment")
    check(not single.match("docs/art/brief.md"), "single-star must not cross a path separator")


# --- precedence -------------------------------------------------------------

def test_specificity_precedence():
    check(
        specificity("scenes/UI/**") > specificity("scenes/**"),
        "a deeper concrete path must outrank a shallower one",
    )
    entries = load_entries()
    scene = resolve("scenes/UI/Hud.tscn", entries)
    check(scene is not None and scene["path"] == "scenes/UI/**",
          "scenes/UI/Hud.tscn must resolve to the scenes/UI/** entry")
    check(scene is not None and "ux_designer" in scene["reviewers"],
          "UI scene changes must require a ux_designer review")

    tool = resolve("tools/blender/forge.py", entries)
    check(tool is not None and tool["path"] == "tools/blender/**",
          "tools/blender files must resolve to the blender entry, not tools/**")

    generic = resolve("tools/agent_preflight.py", entries)
    check(generic is not None and generic["path"] == "tools/**",
          "a plain tools file must fall back to tools/**")

    doc = resolve("docs/art/reviews/stage-a.md", entries)
    check(doc is not None and doc["owner"] == "art_director",
          "docs/art must be owned by the art_director, not the docs/** fallback")


def test_windows_paths_resolve():
    entries = load_entries()
    windows = resolve(r"src\Player\PlayerController.cs", entries)
    check(windows is not None and windows["owner"] == "gameplay_engineer",
          "Windows-style separators must resolve (the project is Windows-first)")


def test_dotted_paths_keep_their_leading_dot():
    """Regression: normalizing with lstrip('./') ate the dot and left CI unowned."""
    entries = load_entries()
    workflow = resolve(".github/workflows/ci.yml", entries)
    check(workflow is not None and workflow["owner"] == "build_release_engineer",
          ".github/workflows must resolve to the build_release_engineer")
    template = resolve("./.github/pull_request_template.md", entries)
    check(template is not None and template["owner"] == "producer",
          "a leading ./ must be stripped as a prefix without eating the dotted directory")


def test_unowned_path_returns_none():
    check(resolve("no/such/place/file.txt", load_entries()) is None,
          "an unmatched path must resolve to None so it can be routed to the producer")


def test_constitutional_files_are_human_owned():
    entries = load_entries()
    for name in ("README.md", "AGENTS.md", "CLAUDE.md"):
        entry = resolve(name, entries)
        check(entry is not None and entry["owner"] == "human",
              f"{name} must be human-owned")
        check(entry is not None and entry["rights"] == "human_approval_required",
              f"{name} must require explicit human instruction")


def test_no_agent_owns_a_path_it_also_reviews():
    for entry in load_entries():
        check(entry["owner"] not in entry["reviewers"],
              f"{entry['path']}: an owner cannot be its own reviewer")


def test_parser_ignores_comments_and_trailing_sections():
    text = (
        "ownership:\n"
        "  # a comment line\n"
        "  - path: a/**\n"
        "    owner: producer\n"
        "    reviewers: [qa_lead, technical_director]\n"
        "    rights: agent_write\n"
        "\n"
        "trailing_key:\n"
        "  - path: ignored/**\n"
    )
    entries = parse_ownership(text)
    check(len(entries) == 1, "parser must stop at the next top-level key")
    check(entries[0]["reviewers"] == ["qa_lead", "technical_director"],
          "parser must read the reviewer list")


# --- validator enforcement --------------------------------------------------

def run_validator(root: Path) -> subprocess.CompletedProcess:
    return subprocess.run(
        [sys.executable, str(root / "tools" / "validate_agent_studio.py")],
        capture_output=True,
        text=True,
    )


def sandbox() -> Path:
    tmp = Path(tempfile.mkdtemp(prefix="titancraft-ownership-"))
    shutil.copytree(ROOT / "studio", tmp / "studio")
    (tmp / "tools").mkdir()
    for name in ("validate_agent_studio.py", "agent_ownership.py"):
        shutil.copy(ROOT / "tools" / name, tmp / "tools" / name)
    return tmp


def test_validator_accepts_the_real_repository():
    result = run_validator(ROOT)
    check(result.returncode == 0,
          f"validator must pass on the checked-in repository, got:\n{result.stdout}{result.stderr}")


def assert_validator_rejects(mutate, expected: str, label: str) -> None:
    tmp = sandbox()
    try:
        mutate(tmp)
        result = run_validator(tmp)
        check(result.returncode != 0, f"{label}: validator must exit non-zero")
        check(expected in result.stdout,
              f"{label}: expected {expected!r} in validator output, got:\n{result.stdout}")
    finally:
        shutil.rmtree(tmp, ignore_errors=True)


def test_validator_rejects_unknown_owner():
    def mutate(tmp: Path) -> None:
        index = tmp / "studio" / "indexes" / "ownership.yml"
        index.write_text(
            index.read_text(encoding="utf-8").replace(
                "  - path: tests/**\n    owner: qa_lead",
                "  - path: tests/**\n    owner: ghost_agent",
            ),
            encoding="utf-8",
        )
    assert_validator_rejects(mutate, "unknown owner 'ghost_agent'", "unknown owner")


def test_validator_rejects_unknown_reviewer():
    def mutate(tmp: Path) -> None:
        index = tmp / "studio" / "indexes" / "ownership.yml"
        index.write_text(
            index.read_text(encoding="utf-8").replace(
                "  - path: tests/**\n    owner: qa_lead\n    reviewers: [gameplay_engineer, technical_director]",
                "  - path: tests/**\n    owner: qa_lead\n    reviewers: [nobody]",
            ),
            encoding="utf-8",
        )
    assert_validator_rejects(mutate, "unknown reviewer 'nobody'", "unknown reviewer")


def test_validator_rejects_agent_file_drift():
    def mutate(tmp: Path) -> None:
        agent = tmp / "studio" / "agents" / "qa_lead.md"
        text = agent.read_text(encoding="utf-8")
        text = re.sub(r"`tests/\*\*`, ", "", text, count=1)
        agent.write_text(text, encoding="utf-8")
    assert_validator_rejects(
        mutate,
        "does not declare owned path 'tests/**'",
        "agent file drops an owned path",
    )


def test_validator_rejects_undeclared_agent_claim():
    def mutate(tmp: Path) -> None:
        agent = tmp / "studio" / "agents" / "qa_lead.md"
        text = agent.read_text(encoding="utf-8")
        text = text.replace(
            "- Owns (`agent_write`): `studio/rehearsals/**`",
            "- Owns (`agent_write`): `src/Player/**`, `studio/rehearsals/**`",
            1,
        )
        agent.write_text(text, encoding="utf-8")
    assert_validator_rejects(
        mutate,
        "declares owned path 'src/Player/**'",
        "agent file claims a path it does not own",
    )


def test_validator_rejects_missing_owned_paths_heading():
    def mutate(tmp: Path) -> None:
        agent = tmp / "studio" / "agents" / "builder.md"
        text = agent.read_text(encoding="utf-8")
        agent.write_text(text.replace("## Owned Paths", "## Paths"), encoding="utf-8")
    assert_validator_rejects(
        mutate, "missing heading 'Owned Paths'", "agent file without an Owned Paths heading"
    )


def test_validator_rejects_human_owner_with_agent_write():
    def mutate(tmp: Path) -> None:
        index = tmp / "studio" / "indexes" / "ownership.yml"
        index.write_text(
            index.read_text(encoding="utf-8").replace(
                "  - path: README.md\n    owner: human\n    reviewers: [game_director, producer]\n    rights: human_approval_required",
                "  - path: README.md\n    owner: human\n    reviewers: [game_director, producer]\n    rights: agent_write",
            ),
            encoding="utf-8",
        )
    assert_validator_rejects(
        mutate,
        "human-owned but not human_approval_required",
        "human-owned path opened to agent writes",
    )


def test_validator_rejects_duplicate_path():
    def mutate(tmp: Path) -> None:
        index = tmp / "studio" / "indexes" / "ownership.yml"
        index.write_text(
            index.read_text(encoding="utf-8")
            + "  - path: tests/**\n    owner: producer\n    reviewers: [qa_lead]\n    rights: agent_write\n",
            encoding="utf-8",
        )
    assert_validator_rejects(mutate, "Duplicate ownership path: tests/**", "duplicate path")


def test_validator_rejects_missing_ownership_index():
    def mutate(tmp: Path) -> None:
        (tmp / "studio" / "indexes" / "ownership.yml").unlink()
    assert_validator_rejects(mutate, "ownership.yml", "deleted ownership index")


def main() -> int:
    tests = [value for name, value in sorted(globals().items()) if name.startswith("test_")]
    for test in tests:
        test()
    if failures:
        print("Ownership tests failed:")
        for failure in failures:
            print(f"- {failure}")
        return 1
    print(f"Ownership tests passed: {len(tests)} tests, 0 failures.")
    print("Checked glob translation, precedence, Windows paths, human-owned paths, and eight validator rejection cases.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
