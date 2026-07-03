#!/usr/bin/env python3
"""Generate mandatory Agent Studio preflight packets from router output."""
from __future__ import annotations

import argparse
import json
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
if str(ROOT / "tools") not in sys.path:
    sys.path.insert(0, str(ROOT / "tools"))

from agent_task_router import route_task  # noqa: E402

GENERAL_FORBIDDEN_ACTIONS = [
    "Do not edit files before reading README.md and this preflight packet.",
    "Do not expand MVP gameplay scope beyond README.md.",
    "Do not ignore scope warnings or missing evidence gates.",
    "Do not use vague verdicts or non-approved final verdict language.",
    "Do not claim PASS without required evidence and validation output.",
]

BEFORE_EDITING_CHECKLIST = [
    "Read README.md and confirm the task does not conflict with MVP Crash Site scope.",
    "Read root AGENTS.md and any scoped AGENTS.md files for touched paths.",
    "Load the required memories listed in this packet.",
    "Review the required skills and checklists listed in this packet.",
    "Confirm forbidden actions, forbidden scope, and scope warnings are understood.",
    "Confirm required evidence can be produced; otherwise stop with a blocking verdict.",
    "Plan three to seven steps before modifying the minimum necessary files.",
]


def build_packet(description: str) -> dict:
    """Return a deterministic preflight packet, extending router output only."""
    routed = route_task(description)
    packet = dict(routed)
    packet["forbidden_actions"] = GENERAL_FORBIDDEN_ACTIONS
    packet["forbidden_scope"] = [
        "gameplay code unless explicitly requested and permitted by README.md",
        "production scenes, Godot project settings, C# runtime code, tests, assets, or visual content unless explicitly in scope",
        "forbidden MVP features listed in README.md and AGENTS.md",
    ]
    packet["before_editing_files_checklist"] = BEFORE_EDITING_CHECKLIST
    return packet


def lines(title: str, items: list[str]) -> list[str]:
    if not items:
        return [f"## {title}", "- None"]
    return [f"## {title}", *[f"- {item}" for item in items]]


def render_human(packet: dict) -> str:
    out: list[str] = [
        "# Agent Studio Preflight Packet",
        "",
        "## Task Description",
        packet["task_description"],
        "",
        "## Task Category",
        f"- Routed category: {packet['detected_task_category']}",
        f"- Evidence category: {packet['evidence_category']}",
        "",
        "## Primary Agent",
        f"- {packet['primary_agent']}",
        "",
    ]
    sections = [
        ("Secondary Agents", packet["secondary_agents"]),
        ("Required Memories", packet["required_memory_packs_cards"]),
        ("Required Skills", packet["required_skills"]),
        ("Required Checklists", packet["required_checklists"]),
        ("Required Evidence", packet["required_evidence"]),
        ("Forbidden Actions", packet["forbidden_actions"]),
        ("Forbidden Scope", packet["forbidden_scope"]),
        ("Forbidden Verdicts", packet["forbidden_verdicts"]),
        ("Approved Verdicts", packet["approved_final_verdicts"]),
        ("Minimum Validation Commands", packet["minimum_validation_commands"]),
        ("Scope Warnings", packet["scope_warnings"]),
        ("Before Editing Files Checklist", packet["before_editing_files_checklist"]),
    ]
    for title, items in sections:
        out.extend(lines(title, items))
        out.append("")
    return "\n".join(out).rstrip() + "\n"


def main(argv: list[str]) -> int:
    parser = argparse.ArgumentParser(description="Generate an Agent Studio preflight packet.")
    parser.add_argument("task", nargs="*", help="Task description. If omitted, stdin is used.")
    parser.add_argument("--json", action="store_true", help="Emit deterministic JSON instead of human-readable text.")
    args = parser.parse_args(argv)
    description = " ".join(args.task).strip() or sys.stdin.read().strip()
    if not description:
        print("Task description is required.", file=sys.stderr)
        return 2
    packet = build_packet(description)
    if args.json:
        print(json.dumps(packet, indent=2, sort_keys=True))
    else:
        print(render_human(packet), end="")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv[1:]))
