#!/usr/bin/env python3
"""Validate Windows playtest journey verdict documents without third-party deps.

Enforces studio/decisions/quality_benchmark_v2_agent_gate_delegation.md:
the aesthetic verdict may come from the Visual Reviewer agent on opened CI
capture artifacts, and performance/stability claims must cite generated
artifacts — but subjective feel adjectives are still rejected unless the
document carries a dated human playtest note. This keeps the chain
agent-complete without letting any agent fabricate an experience it never
had.

Usage:
  python3 tools/validate_playtest_evidence.py            # validate committed verdicts
  python3 tools/validate_playtest_evidence.py --draft F  # validate a generated draft
"""
from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
PLAYTEST_DIR = ROOT / "docs/production/playtests"

APPROVED_VERDICTS = {
    "PASS", "FAIL_REPO_OWNED", "HUMAN_BLOCKED", "ENVIRONMENT_BLOCKED",
    "EXTERNAL_SECRET_BLOCKED", "INTENTIONAL_GATE", "DRY_RUN_ONLY", "NOT_GO", "GO",
}
DRAFT_PLACEHOLDER = "PENDING"

REQUIRED_HEADINGS = [
    "Build Provenance",
    "Windows Smoke Evidence",
    "Aesthetic Verdict",
    "Feel Evidence",
    "Final Verdict",
]

# Subjective feel language that no agent may assert. A dated human note is
# the only thing that legitimizes these phrases in a verdict document.
BANNED_FEEL_PHRASES = [
    "feels responsive", "feels great", "feels good", "feels satisfying",
    "plays great", "plays well", "combat is satisfying", "movement is satisfying",
    "game feel is", "feel is excellent", "feels polished", "feels smooth",
]

HUMAN_NOTE_PATTERN = re.compile(r"human\s+note\s*\(\s*\d{4}-\d{2}-\d{2}\s*\)", re.I)
SHA256_PATTERN = re.compile(r"\b[0-9a-f]{64}\b")
RUN_REFERENCE_PATTERN = re.compile(r"(runs?/\d+|workflow\s+run|run\s+id\s*[:=]|github\.run_id)", re.I)
PNG_PATTERN = re.compile(r"\S+\.png\b")
EXIT_CODE_PATTERN = re.compile(r"exit[_\s]code\D{0,4}0\b", re.I)


def section(text: str, heading: str) -> str | None:
    match = re.search(rf"^##\s+{re.escape(heading)}\s*$(.*?)(?=^##\s|\Z)", text, re.M | re.S)
    return match.group(1) if match else None


def verdict_values(body: str) -> list[str]:
    return [m.group(1) for m in re.finditer(r"^\s*(?:\*\*)?Verdict(?:\*\*)?\s*:\s*`?([A-Z_]+|PENDING)`?", body, re.M)]


def validate_document(path: Path, draft: bool) -> list[str]:
    errors: list[str] = []
    text = path.read_text(encoding="utf-8")
    label = str(path)

    for heading in REQUIRED_HEADINGS:
        if section(text, heading) is None:
            errors.append(f"{label}: missing required section '## {heading}'")
    if errors:
        return errors

    provenance = section(text, "Build Provenance") or ""
    if not SHA256_PATTERN.search(provenance):
        errors.append(f"{label}: Build Provenance must cite a SHA-256 hash of the played executable")
    if not RUN_REFERENCE_PATTERN.search(provenance):
        errors.append(f"{label}: Build Provenance must cite the workflow run that produced the build")

    smoke = section(text, "Windows Smoke Evidence") or ""
    if not draft and not EXIT_CODE_PATTERN.search(smoke):
        errors.append(f"{label}: Windows Smoke Evidence must record exit code 0 from the Windows runner smoke")
    if "artifact" not in smoke.lower():
        errors.append(f"{label}: Windows Smoke Evidence must name the uploaded artifact it came from")

    aesthetic = section(text, "Aesthetic Verdict") or ""
    aesthetic_verdicts = verdict_values(aesthetic)
    if not aesthetic_verdicts:
        errors.append(f"{label}: Aesthetic Verdict must contain a 'Verdict:' line")
    if not PNG_PATTERN.search(aesthetic):
        errors.append(f"{label}: Aesthetic Verdict must cite the opened capture PNG paths it judged")
    if "reviewer" not in aesthetic.lower() and "human" not in aesthetic.lower():
        errors.append(f"{label}: Aesthetic Verdict must name the reviewing role (Visual Reviewer agent or human)")

    feel = section(text, "Feel Evidence") or ""
    has_human_note = HUMAN_NOTE_PATTERN.search(text) is not None
    lowered = text.lower()
    for phrase in BANNED_FEEL_PHRASES:
        if phrase in lowered and not has_human_note:
            errors.append(
                f"{label}: subjective feel claim '{phrase}' requires a dated 'Human note (YYYY-MM-DD)'; "
                "agents must cite measured proxies instead"
            )
    if not has_human_note and not re.search(r"(measured|metric|proxy|frames|seconds|ms\b)", feel, re.I):
        errors.append(f"{label}: Feel Evidence must cite measured proxies when no dated human note exists")

    final = section(text, "Final Verdict") or ""
    final_verdicts = verdict_values(final)
    if not final_verdicts:
        errors.append(f"{label}: Final Verdict must contain a 'Verdict:' line")
    for value in aesthetic_verdicts + final_verdicts:
        if value == DRAFT_PLACEHOLDER:
            if not draft:
                errors.append(f"{label}: committed verdicts may not contain PENDING placeholders")
        elif value not in APPROVED_VERDICTS:
            errors.append(f"{label}: verdict '{value}' is not in the approved vocabulary")

    return errors


def main() -> int:
    if len(sys.argv) >= 2 and sys.argv[1] == "--draft":
        if len(sys.argv) != 3:
            print("usage: validate_playtest_evidence.py --draft <file>")
            return 2
        targets = [Path(sys.argv[2])]
        draft = True
    else:
        targets = sorted(PLAYTEST_DIR.glob("*.md")) if PLAYTEST_DIR.exists() else []
        draft = False
        if not targets:
            print("Playtest evidence validation passed: no committed verdict documents yet "
                  f"({PLAYTEST_DIR.relative_to(ROOT)} is empty).")
            return 0

    errors: list[str] = []
    for target in targets:
        if not target.exists():
            errors.append(f"{target}: file not found")
            continue
        errors.extend(validate_document(target, draft))

    if errors:
        print("Playtest evidence validation failed:")
        for error in errors:
            print(f"- {error}")
        return 1

    mode = "draft" if draft else "committed verdict"
    print(f"Playtest evidence validation passed: {len(targets)} {mode} document(s) checked "
          "(provenance hash, smoke evidence, cited PNGs, verdict vocabulary, feel-claim guard).")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
