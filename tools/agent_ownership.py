#!/usr/bin/env python3
"""Resolve TitanCraft Agent Studio ownership rights without third-party deps.

Answers "who owns this file, who must review it, and may an agent write it"
from `studio/indexes/ownership.yml`.

Usage:
    python3 tools/agent_ownership.py src/Player/PlayerController.cs
    python3 tools/agent_ownership.py --json scenes/UI/Hud.tscn
    python3 tools/agent_ownership.py --list
"""
from pathlib import Path
import json
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
OWNERSHIP_INDEX = ROOT / "studio" / "indexes" / "ownership.yml"

HUMAN_OWNER = "human"
RIGHTS = ("agent_write", "human_approval_required")


class OwnershipError(Exception):
    """Raised when the ownership index cannot be parsed."""


def parse_ownership(text: str) -> list[dict]:
    """Parse the `ownership:` list into records.

    Kept line-based on purpose: the Agent Studio validators must run on a clean
    Python install with no third-party YAML dependency.
    """
    entries: list[dict] = []
    in_section = False
    current: dict | None = None

    for raw in text.splitlines():
        line = raw.split("#", 1)[0].rstrip() if not raw.strip().startswith("#") else ""
        if not line.strip():
            continue
        if re.match(r"^ownership:\s*$", line):
            in_section = True
            continue
        if not in_section:
            continue
        # A new top-level key ends the ownership section.
        if re.match(r"^[^\s#-][^:]*:", line):
            break

        start = re.match(r"^\s*-\s*path:\s*(\S.*?)\s*$", line)
        if start:
            current = {"path": start.group(1), "reviewers": []}
            entries.append(current)
            continue
        if current is None:
            continue

        owner = re.match(r"^\s+owner:\s*(\S.*?)\s*$", line)
        if owner:
            current["owner"] = owner.group(1)
            continue
        rights = re.match(r"^\s+rights:\s*(\S.*?)\s*$", line)
        if rights:
            current["rights"] = rights.group(1)
            continue
        reviewers = re.match(r"^\s+reviewers:\s*\[(.*)\]\s*$", line)
        if reviewers:
            current["reviewers"] = [
                item.strip() for item in reviewers.group(1).split(",") if item.strip()
            ]
            continue

    return entries


def load_entries(index_path: Path = OWNERSHIP_INDEX) -> list[dict]:
    if not index_path.exists():
        raise OwnershipError(f"Missing ownership index: {index_path}")
    entries = parse_ownership(index_path.read_text(encoding="utf-8"))
    if not entries:
        raise OwnershipError(f"Ownership index declares no entries: {index_path}")
    return entries


def pattern_to_regex(pattern: str) -> re.Pattern:
    """Translate a `/**` and `*` path glob into an anchored regex."""
    out: list[str] = []
    index = 0
    while index < len(pattern):
        if pattern.startswith("/**", index):
            out.append("(?:/.*)?")
            index += 3
        elif pattern.startswith("**", index):
            out.append(".*")
            index += 2
        elif pattern[index] == "*":
            out.append("[^/]*")
            index += 1
        elif pattern[index] == "?":
            out.append("[^/]")
            index += 1
        else:
            out.append(re.escape(pattern[index]))
            index += 1
    return re.compile("^" + "".join(out) + "$")


def specificity(pattern: str) -> tuple[int, int]:
    """Rank patterns so the most specific path wins, per AGENTS.md section 11."""
    segments = pattern.split("/")
    concrete = sum(1 for segment in segments if "*" not in segment)
    literal = len(pattern.replace("*", ""))
    return (concrete, literal)


def normalize(path: str) -> str:
    """Normalize to repo-relative POSIX form.

    Strips a leading `./` as a prefix, never as a character set: dotted paths
    such as `.github/workflows/` must keep their leading dot.
    """
    text = str(path).replace("\\", "/").strip()
    while text.startswith("./"):
        text = text[2:]
    return text.strip("/")


def resolve(path: str, entries: list[dict] | None = None) -> dict | None:
    """Return the owning entry for `path`, or None when the path is unowned."""
    entries = entries if entries is not None else load_entries()
    target = normalize(path)
    matches = [
        entry for entry in entries if pattern_to_regex(entry["path"]).match(target)
    ]
    if not matches:
        return None
    return max(matches, key=lambda entry: specificity(entry["path"]))


def describe(path: str, entry: dict | None) -> str:
    if entry is None:
        return (
            f"{path}\n"
            "- owner: UNOWNED\n"
            "- required_reviewers: none\n"
            "- rights: unowned\n"
            "- action: route to the producer and add the path to "
            "studio/indexes/ownership.yml before changing it."
        )
    reviewers = ", ".join(entry["reviewers"]) or "none"
    if entry["owner"] == HUMAN_OWNER:
        action = (
            "no agent may change this path without explicit human instruction in the task."
        )
    else:
        action = (
            f"{entry['owner']} authors the change; "
            f"{reviewers} must review before a PASS verdict."
        )
    return (
        f"{path}\n"
        f"- matched_pattern: {entry['path']}\n"
        f"- owner: {entry['owner']}\n"
        f"- required_reviewers: {reviewers}\n"
        f"- rights: {entry['rights']}\n"
        f"- action: {action}"
    )


def main(argv: list[str]) -> int:
    args = [arg for arg in argv if not arg.startswith("--")]
    as_json = "--json" in argv

    try:
        entries = load_entries()
    except OwnershipError as error:
        print(f"Ownership resolution failed: {error}", file=sys.stderr)
        return 1

    if "--list" in argv:
        if as_json:
            print(json.dumps(entries, indent=2))
        else:
            for entry in sorted(entries, key=lambda item: item["path"]):
                reviewers = ", ".join(entry["reviewers"]) or "none"
                print(
                    f"{entry['path']} -> {entry['owner']} "
                    f"[{entry['rights']}] reviewers: {reviewers}"
                )
        return 0

    if not args:
        print(__doc__.strip(), file=sys.stderr)
        return 2

    results = [(path, resolve(path, entries)) for path in args]
    if as_json:
        payload = [
            {
                "path": path,
                "owner": entry["owner"] if entry else None,
                "matched_pattern": entry["path"] if entry else None,
                "required_reviewers": entry["reviewers"] if entry else [],
                "rights": entry["rights"] if entry else "unowned",
            }
            for path, entry in results
        ]
        print(json.dumps(payload, indent=2))
    else:
        print("\n\n".join(describe(path, entry) for path, entry in results))
    return 0


if __name__ == "__main__":
    sys.exit(main(sys.argv[1:]))
