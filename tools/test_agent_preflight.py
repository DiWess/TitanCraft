#!/usr/bin/env python3
"""Validate Agent Studio preflight packet behavior."""
from __future__ import annotations

import json
import subprocess
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
PREFLIGHT = ROOT / "tools" / "agent_preflight.py"
REHEARSAL_PACKET = ROOT / "studio" / "rehearsals" / "expected_packets" / "bespoke_stage_a_art.json"
GENERATED_PROMPT = ROOT / "studio" / "prompts" / "generated" / "bespoke_stage_a_art_prompt.md"
FORBIDDEN_VAGUE = {"done", "improved", "looks good", "should be fine", "tests passed"}


def run(args: list[str]) -> str:
    return subprocess.check_output([sys.executable, str(PREFLIGHT), *args], text=True)


def packet(task: str) -> dict:
    return json.loads(run([task, "--json"]))


def evidence_blob(data: dict) -> str:
    return " | ".join(data["required_evidence"]).lower()


def test_human_output():
    output = run(["Agent Studio governance routing rehearsal before PR"])
    lowered = output.lower()
    for expected in ["primary agent", "required memories", "required skills", "required evidence", "approved verdicts"]:
        assert expected in lowered, f"missing human section: {expected}"


def test_json_parses():
    data = packet("Agent Studio governance routing rehearsal before PR")
    assert data["detected_task_category"] == "prompt_or_agent_governance"
    assert data["primary_agent"]
    assert isinstance(data["before_editing_files_checklist"], list)


def test_visual_evidence():
    data = packet("Review visual screenshot PNG route slab composition before Stage A approval")
    blob = evidence_blob(data)
    assert data["evidence_category"] == "visual"
    assert "png screenshots" in blob
    assert "visual diagnosis" in blob


def test_gameplay_evidence():
    data = packet("Fix gameplay bug where player inventory mission pickup fails")
    blob = evidence_blob(data)
    assert data["evidence_category"] == "gameplay"
    assert "integration tests" in blob or "smoke test" in blob


def test_gameplay_mvp_loop_smoke_routes_to_gameplay_qa():
    data = packet(
        "Add or harden full playable Crash Site MVP loop smoke test covering spawn, "
        "resource collection, crafting, Galaxabrain Scout combat, component retrieval, "
        "save point, beacon activation, victory, defeat, and save continuation"
    )
    blob = evidence_blob(data)
    assert data["detected_task_category"] == "gameplay_bug"
    assert data["evidence_category"] == "gameplay"
    assert data["primary_agent"] == "gameplay_engineer"
    assert "qa_lead" in data["secondary_agents"]
    assert "integration tests" in blob
    assert "mission smoke test" in blob


def test_agent_studio_gameplay_smoke_not_governance():
    data = packet("Agent Studio routing for gameplay smoke test and integration test tasks")
    assert data["detected_task_category"] == "gameplay_bug"
    assert data["evidence_category"] == "gameplay"
    assert data["primary_agent"] == "gameplay_engineer"

def test_asset_evidence():
    data = packet("Import asset OBJ with provenance licence source URL hash and audition")
    blob = evidence_blob(data)
    assert data["evidence_category"] == "asset"
    for expected in ["source url", "licence", "hash", "audition screenshot"]:
        assert expected in blob, f"missing asset evidence: {expected}"


def test_vague_verdicts_not_approved():
    data = packet("Review visual screenshot PNG route slab composition")
    approved = {item.lower() for item in data["approved_final_verdicts"]}
    assert not (approved & FORBIDDEN_VAGUE), approved


def test_deterministic_output():
    task = "Agent Studio governance routing rehearsal before PR"
    first = run([task, "--json"])
    second = run([task, "--json"])
    assert first == second


def test_bespoke_stage_a_art_dry_run_packet():
    expected = json.loads(REHEARSAL_PACKET.read_text(encoding="utf-8"))
    actual = packet(expected["task_description"])
    assert actual == expected
    assert actual["primary_agent"] == "art_director"
    assert "PNG screenshots" in actual["required_evidence"]
    assert "PASS" in actual["approved_final_verdicts"]


def test_bespoke_stage_a_art_prompt_uses_packet_gates():
    text = GENERATED_PROMPT.read_text(encoding="utf-8")
    lowered = text.lower()
    for expected in [
        "task packet summary",
        "forbidden scope",
        "required memories",
        "required skills",
        "required evidence",
        "visual screenshot gate",
        "gameplay preservation",
        "approved final verdicts",
    ]:
        assert expected in lowered, f"missing prompt section: {expected}"
    assert "png screenshots" in lowered
    assert "preserve" in lowered and "gameplay" in lowered


def main() -> int:
    for test in [
        test_human_output,
        test_json_parses,
        test_visual_evidence,
        test_gameplay_evidence,
        test_gameplay_mvp_loop_smoke_routes_to_gameplay_qa,
        test_agent_studio_gameplay_smoke_not_governance,
        test_asset_evidence,
        test_vague_verdicts_not_approved,
        test_deterministic_output,
        test_bespoke_stage_a_art_dry_run_packet,
        test_bespoke_stage_a_art_prompt_uses_packet_gates,
    ]:
        test()
    print("Agent preflight tests passed: 11 checks")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
