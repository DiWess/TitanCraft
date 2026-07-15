#!/usr/bin/env python3
"""Route TitanCraft Agent Studio tasks using checked-in indexes only."""
from __future__ import annotations
from pathlib import Path
import argparse
import json
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
INDEX = ROOT / "studio" / "indexes"

CATEGORY_RULES = [
    ("visual_scene_composition", "visual", ["screenshot", "visual", "stage a", "toy-like", "toy like", "route slab", "terrain", "hull", "composition", "png", "blender", "best version", "art direction", "design quality"]),
    ("asset_import", "asset", ["asset", "provenance", "licence", "license", "source url", "obj", "hash", "audition"]),
    ("build_failure", "build", ["windows export", "export", "build failure", "ci", "artifact", "release failure"]),
    ("godot_scene_change", "architecture", ["godot scene", "scene edit", "collision", ".tscn", "node", "import"]),
    ("gameplay_bug", "gameplay", [
        "agent studio gameplay", "gameplay workflow", "gameplay governance", "gameplay", "resource pickup", "pickup", "inventory", "mission", "player", "enemy", "bug",
        "mvp loop", "smoke test", "integration test", "crash site gameplay", "a-to-z crash site", "a to z crash site", "playthrough",
        "save continuation", "victory", "defeat", "resource collection", "crafting", "combat",
        "beacon", "galaxabrain", "hud objective", "objective hud", "objective consistency",
        "hudbreadcrumb", "crash site hud", "mission objective text", "mvp objective text",
        "gameplay hud assertion", "integration hud objective", "smoke objective assertion",
    ]),
    ("prompt_or_agent_governance", "documentation", ["prompt", "scope expansion", "documentation-only", "docs-only", "agent studio"]),
    ("stage_gate", "release", ["stage b", "stage gate", "stage a failed", "go/no-go", "readiness"]),
]
CHECKLISTS = {
    "visual": ["before_task", "before_visual_claim", "visual_review", "before_pr"],
    "gameplay": ["before_task", "gameplay_slice", "gameplay_validation", "before_commit", "before_pr"],
    "asset": ["before_task", "asset_import", "before_pr"],
    "build": ["before_task", "release_readiness", "before_pr"],
    "architecture": ["before_task", "before_scene_edit", "before_commit", "before_pr"],
    "documentation": ["before_task", "before_pr"],
    "release": ["before_task", "release_readiness", "before_merge"],
}
VALIDATION_COMMANDS = {
    "visual": ["python3 tools/validate_agent_studio.py", "git diff --check"],
    "gameplay": ["unit tests when behavior changes", "integration or mission smoke test", "git diff --check"],
    "asset": ["python3 tools/validate_agent_studio.py", "git diff --check"],
    "build": ["python3 tools/validate_agent_studio.py", "git diff --check"],
    "architecture": ["python3 tools/validate_agent_studio.py", "git diff --check"],
    "documentation": ["python3 tools/validate_agent_studio.py", "git diff --check"],
    "release": ["python3 tools/validate_agent_studio.py", "git diff --check"],
}
SCOPE_WARNINGS = {
    "visual": ["Do not claim visual success without opened PNGs and visual diagnosis.", "Do not decorate toy-like hulls or call route slabs terrain."],
    "gameplay": ["Do not add forbidden MVP features while fixing the bug.", "Runtime tests do not replace manual mission smoke evidence."],
    "asset": ["Do not import assets without source URL, licence, hash, and audition evidence.", "Fake placeholder OBJ files are forbidden."],
    "build": ["Do not treat dummy or local artifacts as production readiness.", "No gameplay tests are required for Agent Studio routing changes."],
    "architecture": ["Do not edit production scenes unless explicitly requested.", "Separate collision/runtime safety from visual approval."],
    "documentation": ["Do not submit documentation-only PRs for implementation requests.", "Block prompt scope expansion beyond README."],
    "release": ["Do not advance Stage B when Stage A failed.", "Release remains NOT_GO without required evidence."],
}


def read(path: Path) -> str:
    return path.read_text(encoding="utf-8")


def parse_routes(text: str) -> dict:
    routes = {}
    current = None
    for raw in text.splitlines():
        if not raw.startswith(" ") or raw.strip() in {"routes:", "version: 1"}:
            continue
        if raw.startswith("  ") and not raw.startswith("    "):
            current = raw.strip().rstrip(":")
            routes[current] = {"primary": "", "secondary": []}
        elif current and "primary:" in raw:
            routes[current]["primary"] = raw.split("primary:", 1)[1].strip()
        elif current and "secondary:" in raw:
            value = raw.split("secondary:", 1)[1].strip().strip("[]")
            routes[current]["secondary"] = [item.strip() for item in value.split(",") if item.strip()]
    return routes


def parse_list_index(text: str) -> dict:
    data = {}
    current = None
    for raw in text.splitlines():
        line = raw.rstrip()
        if re.match(r"^  [^ ].*:\s*$", line):
            current = line.strip().rstrip(":")
            data[current] = []
        elif current and line.strip().startswith("-"):
            data[current].append(line.strip()[1:].strip())
    return data


def parse_signal_memories(text: str) -> dict:
    signals = {}
    for raw in text.splitlines():
        m = re.match(r"^  ([^:]+):\s*\[(.*)\]", raw)
        if m:
            signals[m.group(1).strip()] = [x.strip() for x in m.group(2).split(",") if x.strip()]
    return signals


def parse_skill_routes(text: str) -> dict:
    routes = {}
    for raw in text.splitlines():
        m = re.match(r"^  ([^:]+):\s*\[(.*)\]", raw)
        if m:
            routes[m.group(1).strip()] = [x.strip() for x in m.group(2).split(",") if x.strip()]
    return routes


def parse_verdicts(text: str) -> tuple[list[str], dict]:
    forbidden = []
    approved = {}
    m = re.search(r"forbidden_vague_verdicts:\s*\[(.*)\]", text)
    if m:
        forbidden = [x.strip() for x in m.group(1).split(",") if x.strip()]
    for raw in text.splitlines():
        m = re.match(r"^  ([^:]+):\s*\[(.*)\]", raw)
        if m:
            approved[m.group(1)] = [x.strip() for x in m.group(2).split(",") if x.strip()]
    return forbidden, approved


def keyword_matches(text: str, keyword: str) -> bool:
    """Match routing signals as words/phrases, not accidental substrings.

    This prevents terms such as ``hash`` from matching ``Crash`` while still
    allowing symbolic signals like ``.tscn`` to match as literal substrings.
    """
    if not re.search(r"[a-z0-9]", keyword):
        return keyword in text
    if keyword.startswith("."):
        return keyword in text
    pattern = rf"(?<![a-z0-9]){re.escape(keyword)}(?![a-z0-9])"
    return re.search(pattern, text) is not None


def detect_category(description: str) -> tuple[str, str]:
    text = description.lower()
    scores = []
    for route_key, evidence_key, keywords in CATEGORY_RULES:
        score = sum(1 for keyword in keywords if keyword_matches(text, keyword))
        scores.append((score, route_key, evidence_key))
    score, route_key, evidence_key = max(scores, key=lambda item: item[0])
    return (route_key, evidence_key) if score else ("prompt_or_agent_governance", "documentation")


def matching_memories(description: str, route_key: str) -> list[str]:
    text = description.lower()
    signal_map = parse_signal_memories(read(INDEX / "memory_routing.yml"))
    memories = []
    for signal, cards in signal_map.items():
        if signal.lower() in text:
            memories.extend(cards)
    route_defaults = {
        "visual_scene_composition": ["MEM-VISFAIL-001", "MEM-VISFAIL-002", "MEM-VISFAIL-004", "MEM-VISFAIL-005"],
        "gameplay_bug": ["MEM-PRODUCT-001", "MEM-GAMEPLAY-MVP-SCOPE-001"],
        "asset_import": ["MEM-ASSET-006", "MEM-ASSET-007", "MEM-ASSET-PROVENANCE-001"],
        "build_failure": ["MEM-CI-RELEASE-LESSONS-004", "MEM-CI-013"],
        "godot_scene_change": ["MEM-GODOT-011", "MEM-GODOT-SCENE-SAFETY-001"],
        "stage_gate": ["MEM-STAGE-008", "MEM-PRODUCTION-STAGE-GATES-001"],
        "prompt_or_agent_governance": ["MEM-PROMPT-009", "MEM-GOV-001", "MEM-GOV-002"],
    }
    memories.extend(route_defaults.get(route_key, []))
    unique = []
    for item in memories:
        if item not in unique:
            unique.append(item)
    return unique


def route_task(description: str) -> dict:
    route_key, evidence_key = detect_category(description)
    agent_routes = parse_routes(read(INDEX / "agent_routing.yml"))
    skill_routes = parse_skill_routes(read(INDEX / "skill_routing.yml"))
    evidence = parse_list_index(read(INDEX / "evidence_requirements.yml"))
    forbidden, approved = parse_verdicts(read(INDEX / "verdicts.yml"))
    route = agent_routes[route_key]
    return {
        "task_description": description,
        "detected_task_category": route_key,
        "evidence_category": evidence_key,
        "primary_agent": route["primary"],
        "secondary_agents": route["secondary"],
        "required_memory_packs_cards": matching_memories(description, route_key),
        "required_skills": skill_routes.get(route_key, []),
        "required_checklists": CHECKLISTS[evidence_key],
        "required_evidence": evidence[evidence_key],
        "forbidden_verdicts": forbidden,
        "approved_final_verdicts": approved[evidence_key],
        "minimum_validation_commands": VALIDATION_COMMANDS[evidence_key],
        "scope_warnings": SCOPE_WARNINGS[evidence_key],
    }


def main(argv: list[str]) -> int:
    parser = argparse.ArgumentParser(description="Route an Agent Studio task into an evidence packet.")
    parser.add_argument("task", nargs="*", help="Task description. If omitted, stdin is used.")
    args = parser.parse_args(argv)
    description = " ".join(args.task).strip() or sys.stdin.read().strip()
    if not description:
        print("Task description is required.", file=sys.stderr)
        return 2
    print(json.dumps(route_task(description), indent=2, sort_keys=True))
    return 0

if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
