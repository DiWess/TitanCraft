#!/usr/bin/env python3
"""Validate TitanCraft Agent Studio governance structure without third-party deps."""
from pathlib import Path
import re
import sys

ROOT = Path(__file__).resolve().parents[1]
STUDIO = ROOT / "studio"

sys.path.insert(0, str(Path(__file__).resolve().parent))
from agent_ownership import (  # noqa: E402
    HUMAN_OWNER,
    RIGHTS as OWNERSHIP_RIGHTS,
    OwnershipError,
    load_entries as load_ownership_entries,
)

AGENT_HEADINGS = [
    "Mission", "Authority", "Owned Paths", "Forbidden Actions", "Required Inputs",
    "Required Outputs", "Required Memories", "Required Skills", "Review Questions",
    "Automatic Rejection Conditions", "Approved Verdicts", "Escalation Rules",
]
MEMORY_FIELDS = [
    "id", "title", "tags", "applies_when", "memory", "avoid",
    "required_action", "evidence_required", "related_agents", "related_skills",
]
SKILL_HEADINGS = [
    "purpose", "when_to_use", "required_inputs", "procedure", "automatic_failures",
    "output_format", "evidence_required", "example_good_output", "example_bad_output",
]
ROUTING_FILES = [
    "agent_routing.yml", "memory_routing.yml", "skill_routing.yml",
    "verdicts.yml", "evidence_requirements.yml", "ownership.yml",
]
OWNERSHIP_FIELDS = ["path", "owner", "reviewers", "rights"]
FORBIDDEN_VAGUE = ["done", "improved", "looks good", "should be fine", "tests passed"]

errors = []

def read(path: Path) -> str:
    try:
        return path.read_text(encoding="utf-8")
    except UnicodeDecodeError:
        errors.append(f"Non-UTF8 file: {path}")
        return ""

def has_heading(text: str, heading: str) -> bool:
    return re.search(rf"^##\s+{re.escape(heading)}\s*$", text, re.I | re.M) is not None

def validate_agents():
    for path in sorted((STUDIO / "agents").glob("*.md")):
        text = read(path)
        for heading in AGENT_HEADINGS:
            if not has_heading(text, heading):
                errors.append(f"Agent missing heading '{heading}': {path}")

def card_blocks(text: str):
    parts = re.split(r"^###\s+", text, flags=re.M)
    return [p for p in parts[1:] if p.strip()]

def validate_memory():
    index = STUDIO / "memory" / "index.yml"
    index_text = read(index)
    referenced = re.findall(r"path:\s*([^\s]+)", index_text)
    for rel in referenced:
        target = STUDIO / "memory" / rel
        if not target.exists():
            errors.append(f"Memory index references missing file: {rel}")
    for path in sorted((STUDIO / "memory").glob("*.md")):
        text = read(path)
        cards = card_blocks(text)
        if not cards:
            errors.append(f"Memory file has no cards: {path}")
            continue
        for card in cards:
            first = card.splitlines()[0].strip()
            for field in MEMORY_FIELDS:
                if re.search(rf"^-\s+{field}:\s*\S", card, re.M) is None:
                    errors.append(f"Memory card {first} missing field '{field}' in {path}")

def validate_skills():
    index = STUDIO / "skills" / "index.yml"
    index_text = read(index)
    referenced = re.findall(r"path:\s*([^\s]+)", index_text)
    for rel in referenced:
        target = STUDIO / "skills" / rel
        if not target.exists():
            errors.append(f"Skills index references missing file: {rel}")
    for path in sorted((STUDIO / "skills").glob("*.md")):
        text = read(path)
        for heading in SKILL_HEADINGS:
            if not has_heading(text, heading):
                errors.append(f"Skill missing heading '{heading}': {path}")

def validate_indexes():
    for rel in ROUTING_FILES:
        path = STUDIO / "indexes" / rel
        if not path.exists():
            errors.append(f"Missing routing index: {path}")
    verdicts = read(STUDIO / "indexes" / "verdicts.yml")
    approved_section = verdicts.split("approved:", 1)[-1] if "approved:" in verdicts else verdicts
    for vague in FORBIDDEN_VAGUE:
        if re.search(rf"\b{re.escape(vague)}\b", approved_section, re.I):
            errors.append(f"Forbidden vague verdict appears in approved verdicts: {vague}")

def declared_owned_paths(text: str) -> set:
    """Read the globs an agent file claims under its `## Owned Paths` heading."""
    match = re.search(r"^##\s+Owned Paths\s*$", text, re.I | re.M)
    if not match:
        return set()
    rest = text[match.end():]
    nxt = re.search(r"^##\s", rest, re.M)
    section = rest[: nxt.start()] if nxt else rest
    line = re.search(r"^-\s+Owns\s+\(`agent_write`\):\s*(.+)$", section, re.M)
    if not line:
        return set()
    return set(re.findall(r"`([^`]+)`", line.group(1)))


def validate_ownership():
    agents = {path.stem for path in (STUDIO / "agents").glob("*.md")}
    try:
        entries = load_ownership_entries(STUDIO / "indexes" / "ownership.yml")
    except OwnershipError as error:
        errors.append(f"Ownership index unreadable: {error}")
        return

    seen = set()
    owned_by_agent = {}
    for entry in entries:
        label = entry.get("path", "<missing path>")
        for field in OWNERSHIP_FIELDS:
            if field not in entry:
                errors.append(f"Ownership entry '{label}' missing field '{field}'")
        if "owner" not in entry or "rights" not in entry:
            continue

        if label in seen:
            errors.append(f"Duplicate ownership path: {label}")
        seen.add(label)

        if entry["rights"] not in OWNERSHIP_RIGHTS:
            errors.append(
                f"Ownership entry '{label}' has unknown rights '{entry['rights']}'"
            )

        owner = entry["owner"]
        if owner != HUMAN_OWNER and owner not in agents:
            errors.append(f"Ownership entry '{label}' names unknown owner '{owner}'")
        if owner == HUMAN_OWNER and entry["rights"] != "human_approval_required":
            errors.append(
                f"Ownership entry '{label}' is human-owned but not human_approval_required"
            )
        if owner != HUMAN_OWNER and entry["rights"] == "human_approval_required":
            errors.append(
                f"Ownership entry '{label}' requires human approval but names agent owner '{owner}'"
            )

        if not entry["reviewers"]:
            errors.append(f"Ownership entry '{label}' declares no reviewers")
        for reviewer in entry["reviewers"]:
            if reviewer not in agents:
                errors.append(
                    f"Ownership entry '{label}' names unknown reviewer '{reviewer}'"
                )
        if owner in entry["reviewers"]:
            errors.append(f"Ownership entry '{label}' lists its owner as its own reviewer")

        if owner != HUMAN_OWNER:
            owned_by_agent.setdefault(owner, set()).add(label)

    # Agent files and the index must not drift apart.
    for path in sorted((STUDIO / "agents").glob("*.md")):
        declared = declared_owned_paths(read(path))
        indexed = owned_by_agent.get(path.stem, set())
        for extra in sorted(declared - indexed):
            errors.append(
                f"Agent {path.stem} declares owned path '{extra}' "
                "that studio/indexes/ownership.yml does not assign to it"
            )
        for missing in sorted(indexed - declared):
            errors.append(
                f"Agent {path.stem} does not declare owned path '{missing}' "
                "assigned to it by studio/indexes/ownership.yml"
            )


def validate_no_empty_placeholders():
    for path in STUDIO.rglob("*"):
        if path.is_file():
            text = read(path)
            if not text.strip():
                errors.append(f"Empty placeholder file: {path}")
            if path.suffix == ".md" and text.strip() in {"TODO", "TBD", "# TODO"}:
                errors.append(f"Placeholder-only file: {path}")

validate_agents()
validate_memory()
validate_skills()
validate_indexes()
validate_ownership()
validate_no_empty_placeholders()

if errors:
    print("Agent Studio validation failed:")
    for error in errors:
        print(f"- {error}")
    sys.exit(1)

print("Agent Studio validation passed.")
print("Checked agents, memory cards, memory/skills indexes, skill headings, routing indexes, verdict index, ownership rights, and empty placeholders.")
