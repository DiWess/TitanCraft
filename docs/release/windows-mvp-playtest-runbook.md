# Windows MVP Manual Playtest Runbook

This runbook starts only after an export artifact exists and is hashed. It does not replace `docs/testing/windows-offline-mvp-playtest.md` or the evidence template in `docs/testing/windows-playtest-evidence-template.md`.

## Inputs

- Exported artifact: `builds/Windows/TitanCraft.exe`.
- Artifact SHA-256 produced by `python3 tools/release/hash_artifact.py builds/Windows/TitanCraft.exe`.
- A clean Windows machine or real Windows VM.
- Offline or network-blocked test state.

## Manual run sequence

1. Copy the exported build folder to the Windows test environment.
2. Record the artifact path, file size, and SHA-256 hash in a copy of `docs/testing/windows-playtest-evidence-template.md`.
3. Disconnect the Windows machine or block network access.
4. Launch `TitanCraft.exe` directly, without the Godot editor.
5. Follow every milestone in `docs/testing/windows-offline-mvp-playtest.md`.
6. Record pass/fail/blocker notes, bugs, media paths, and final playtest verdict in the evidence record.

## Readiness wording

Do not claim Windows readiness, MVP readiness, public demo readiness, or production readiness from export evidence alone. Readiness language requires completed manual Windows evidence and any other release gates required by the current repository governance.
