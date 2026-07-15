#!/usr/bin/env python3
"""Rehearse Agent Studio task routing against checked-in production scenarios."""
from pathlib import Path
import json
import re
import subprocess
import sys

ROOT = Path(__file__).resolve().parents[1]
REHEARSALS = ROOT / "studio" / "rehearsals"
ROUTER = ROOT / "tools" / "agent_task_router.py"
FORBIDDEN_VAGUE = {"done", "improved", "looks good", "should be fine", "tests passed"}


def section(text: str, name: str) -> str:
    pattern = rf"## {re.escape(name)}\n\n(.*?)(?=\n\n## |\Z)"
    match = re.search(pattern, text, re.S)
    if not match:
        raise AssertionError(f"Missing section {name}")
    return match.group(1).strip()


def split_expected(value: str) -> list[str]:
    return [item.strip() for item in re.split(r"[,;]\s*", value) if item.strip()]


def split_semicolon(value: str) -> list[str]:
    return [item.strip() for item in value.split(";") if item.strip()]


def route(description: str) -> dict:
    output = subprocess.check_output([sys.executable, str(ROUTER), description], text=True)
    return json.loads(output)


def assert_contains_all(actual: list[str], expected: list[str], label: str, file: Path):
    missing = [item for item in expected if item not in actual]
    if missing:
        raise AssertionError(f"{file.name}: missing {label}: {missing}; actual={actual}")


def run_rehearsal(path: Path):
    text = path.read_text(encoding="utf-8")
    packet = route(section(text, "task_description"))
    assert packet["primary_agent"] == section(text, "expected_primary_agent"), path.name
    assert_contains_all(packet["secondary_agents"], split_expected(section(text, "expected_secondary_agents")), "secondary agents", path)
    assert_contains_all(packet["required_memory_packs_cards"], split_expected(section(text, "expected_memories")), "memories", path)
    assert_contains_all(packet["required_skills"], split_expected(section(text, "expected_skills")), "skills", path)
    expected_evidence = split_semicolon(section(text, "expected_evidence"))
    for evidence in expected_evidence:
        if not any(evidence == item or evidence.lower() in item.lower() for item in packet["required_evidence"]):
            raise AssertionError(f"{path.name}: missing evidence '{evidence}' in {packet['required_evidence']}")
    if any(v in FORBIDDEN_VAGUE for v in packet["approved_final_verdicts"]):
        raise AssertionError(f"{path.name}: vague verdict approved: {packet['approved_final_verdicts']}")
    if not set(packet["approved_final_verdicts"]).issubset({"PASS", "FAIL_REPO_OWNED", "HUMAN_BLOCKED", "ENVIRONMENT_BLOCKED", "EXTERNAL_SECRET_BLOCKED", "INTENTIONAL_GATE", "DRY_RUN_ONLY", "NOT_GO", "GO"}):
        raise AssertionError(f"{path.name}: unapproved verdict vocabulary: {packet['approved_final_verdicts']}")
    category = packet["evidence_category"]
    evidence_blob = " | ".join(packet["required_evidence"]).lower()
    if category == "visual":
        assert "png screenshots" in evidence_blob, path.name
        assert "visual diagnosis" in evidence_blob, path.name
    if category == "gameplay":
        assert "integration tests" in evidence_blob or "smoke test" in evidence_blob, path.name
    if category == "asset":
        for required in ["source url", "licence", "hash", "audition screenshot"]:
            assert required in evidence_blob, f"{path.name}: missing asset evidence {required}"
    expected_file = REHEARSALS / "expected_packets" / f"{path.stem}.json"
    checked_in = json.loads(expected_file.read_text(encoding="utf-8"))
    comparable = {key: checked_in[key] for key in packet}
    if comparable != packet:
        raise AssertionError(f"{path.name}: checked-in expected packet differs from router output")


def test_gameplay_qa_keywords_outrank_agent_studio_governance():
    packet = route("Agent Studio routing for gameplay smoke test and integration test tasks")
    assert packet["detected_task_category"] == "gameplay_bug"
    assert packet["evidence_category"] == "gameplay"
    assert packet["primary_agent"] == "gameplay_engineer"
    assert "qa_lead" in packet["secondary_agents"]



def test_agent_studio_gameplay_workflow_routes_to_gameplay_with_slice_skill():
    packet = route("make Agent Studio gameplay workflow A-to-Z for Crash Site MVP")
    assert packet["detected_task_category"] == "gameplay_bug"
    assert packet["evidence_category"] == "gameplay"
    assert packet["primary_agent"] == "gameplay_engineer"
    assert "gameplay_slice" in packet["required_checklists"]
    assert "crash_site_gameplay_slice" in packet["required_skills"]
    blob = " | ".join(packet["required_evidence"]).lower()
    assert "crash site mvp scope confirmation" in blob
    assert "galaxabrain scout combat" in blob


def test_crash_site_hud_objective_routes_to_gameplay_qa_not_assets():
    packet = route("Audit and harden Crash Site MVP objective HUD consistency")
    memories = " | ".join(packet["required_memory_packs_cards"])
    assert packet["detected_task_category"] == "gameplay_bug"
    assert packet["evidence_category"] == "gameplay"
    assert packet["primary_agent"] in {"gameplay_engineer", "qa_lead"}
    assert packet["primary_agent"] != "asset_librarian"
    assert "asset_librarian" not in packet["secondary_agents"]
    assert "MEM-ASSET-PROVENANCE-001" not in packet["required_memory_packs_cards"]
    assert "MEM-GAMEPLAY-MVP-SCOPE-001" in packet["required_memory_packs_cards"]
    assert "MEM-PRODUCT-001" in packet["required_memory_packs_cards"]
    assert "gameplay" in memories.lower() or "product" in memories.lower()
    assert "asset_provenance" not in packet["required_skills"]
    assert "asset_audition" not in packet["required_skills"]
    assert "csharp_gameplay_validation" in packet["required_skills"]
    assert "production_debugging" in packet["required_skills"]
    assert "evidence_reporting" in packet["required_skills"]
    evidence_blob = " | ".join(packet["required_evidence"]).lower()
    assert "source url" not in evidence_blob
    assert "licence" not in evidence_blob
    assert "audition screenshot" not in evidence_blob


def test_hud_objective_search_terms_route_to_gameplay_qa():
    phrases = [
        "HUD objective",
        "objective HUD",
        "objective consistency",
        "HudBreadcrumb",
        "Crash Site HUD",
        "mission objective text",
        "MVP objective text",
        "gameplay HUD assertion",
        "integration HUD objective",
        "smoke objective assertion",
    ]
    for phrase in phrases:
        packet = route(f"Audit Crash Site MVP {phrase}")
        assert packet["detected_task_category"] == "gameplay_bug", phrase
        assert packet["evidence_category"] == "gameplay", phrase
        assert packet["primary_agent"] in {"gameplay_engineer", "qa_lead"}, phrase
        assert packet["primary_agent"] != "asset_librarian", phrase
        assert "MEM-ASSET-PROVENANCE-001" not in packet["required_memory_packs_cards"], phrase

def main() -> int:
    test_gameplay_qa_keywords_outrank_agent_studio_governance()
    test_crash_site_hud_objective_routes_to_gameplay_qa_not_assets()
    test_agent_studio_gameplay_workflow_routes_to_gameplay_with_slice_skill()
    test_hud_objective_search_terms_route_to_gameplay_qa()
    files = sorted(p for p in REHEARSALS.glob("*.md") if p.is_file())
    if not files:
        raise AssertionError("No rehearsals found")
    for path in files:
        run_rehearsal(path)
    print(f"Agent task router rehearsals passed: {len(files)} scenarios")
    return 0

if __name__ == "__main__":
    raise SystemExit(main())
