# Definition of Done

## Gameplay done

- The change is inside the locked Crash Site MVP scope.
- Build/test/import evidence is recorded.
- Runtime or integration evidence is recorded when gameplay systems interact.
- No unrequested visual, asset, or scene changes are included.

## Asset done

- Source recipe or approved `.blend` exists.
- GLB export exists.
- Asset manifest is updated when applicable.
- Review PNGs exist.
- Provenance and sha256/hash evidence are recorded.
- Art Taste Pack compliance is reviewed.
- Human or visual-reviewer approval is separate from generation.

## Scene visual done

- Scene edits are explicitly authorized by scope and stage gate.
- Before/after PNGs or review captures exist.
- Visual diagnosis is recorded.
- Human or visual-reviewer verdict is recorded.
- Gameplay collisions and route readability are not regressed.

## CI done

- Workflow or tooling changes are minimal and scoped.
- Commands produce logs or downloadable artifacts.
- Failure modes are explicit and fail closed.
- Existing pipelines are preserved unless the task asked for safe extension.

## Documentation done

- Docs match `README.md`, `AGENTS.md`, and actual repository paths.
- Docs do not authorize new MVP scope.
- Links point to current operating references.
- Validation such as `git diff --check` and Agent Studio checks are recorded.

## Release/export done

- Export commands and artifact metadata are recorded.
- Signing/secrets status is explicit when relevant.
- Offline Windows launch/playtest evidence exists before Windows readiness is claimed.
- Release, beta, or production readiness uses approved fail-closed verdicts only.
