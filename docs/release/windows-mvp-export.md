# Windows MVP Export Evidence Workflow

This workflow creates export evidence for the locked Crash Site MVP Windows build. It does not claim Windows readiness, manual playtest success, public demo readiness, signing readiness, or production readiness.

## Scope guard

- Stay within the solo, offline-first, Windows-first Crash Site MVP described in `README.md`.
- Do not modify gameplay, `Main.tscn`, art assets, scenes, materials, mission order, victory condition, or save semantics as part of export evidence collection.
- Treat Linux headless export evidence as build-process evidence only. Manual Windows launch/playtest evidence is separate.

## Required commands

Run from the repository root. The temporary audio cues are generated artifacts, so materialize them before direct Godot import or export commands:

```bash
python3 tools/prepare_audio_assets.py
godot --headless --path . --import
mkdir -p builds/Windows
godot --headless --path . --export-release "Windows Desktop" builds/Windows/TitanCraft.exe 2>&1 | tee builds/Windows/export.log
python3 tools/release/hash_artifact.py builds/Windows/TitanCraft.exe
```

## Evidence to record

Create or update a file under `docs/release/evidence/` with:

- exact export command;
- `builds/Windows/TitanCraft.exe` artifact path;
- artifact size in bytes;
- SHA-256 hash from `tools/release/hash_artifact.py`;
- export log path;
- export warning/error summary;
- whether `python3 tools/prepare_audio_assets.py` ran before export;
- explicit statement that Windows manual playtest was not claimed unless a real Windows evidence record exists.

## Warning handling

If the export command exits successfully but `builds/Windows/export.log` contains warnings or errors, record the exact warning/error summary and use a warning-bearing export evidence verdict. Do not hide warnings and do not convert the artifact into a readiness claim.
