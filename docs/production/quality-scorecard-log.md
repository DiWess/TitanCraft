# Quality Scorecard Log

Running, append-only history of the 10-axis quality scorecard defined in
`studio/decisions/quality_benchmark_v1.md`. Each PR that touches a benchmarked axis adds one entry below —
this file is the trend line; the ADR is the fixed target.

Do not edit or delete prior entries. If a score was wrong, add a new entry that corrects it and say why.

## How to add an entry

1. Copy the table below.
2. Fill in today's score per axis (0–10) and the peer target from the ADR.
3. Fill `Δ` with the change since the previous entry (`+`, `-`, or `=`).
4. Cite the evidence for every score that changed — a file, log, test run, or screenshot path. No evidence,
   no score change.
5. Append it under "Entries", newest last.

```
### <date> — <PR link or commit sha>

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop |  | 9.0 |  |  |
| 2 | Combat & enemy AI |  | 9.0 |  |  |
| 3 | Movement & controls |  | 9.5 |  |  |
| 4 | Crafting & progression |  | 8.5 |  |  |
| 5 | World / level design |  | 8.5 |  |  |
| 6 | Visual art & presentation |  | 9.0 |  |  |
| 7 | Audio & feedback |  | 8.5 |  |  |
| 8 | Technical stability |  | 8.0 |  |  |
| 9 | Content volume / replayability |  | 9.0 |  |  |
| 10 | Process integrity of studio claims | | n/a |  |  |

**Composite (axes 1–9):**
**Note:**
```

## Entries

### 2026-07-06 — baseline (repo audit, no PR)

| # | Axis | Score /10 | Peer target | Δ | Evidence |
|---|---|---:|---:|---|---|
| 1 | Core gameplay loop | 6.0 | 9.0 | — | `CrashSiteMissionState` + tests: full collect→craft→fight→recover→beacon path wired and passing. |
| 2 | Combat & enemy AI | 3.0 | 9.0 | — | `GalaxabrainScout` 4-state FSM tested; no human feel pass exists. |
| 3 | Movement & controls | 3.0 | 9.5 | — | `FirstPersonController`/`FirstPersonMovement` tested; `20260703_windows_manual_validation_blocked.md` records feel as BLOCKED. |
| 4 | Crafting & progression | 5.0 | 8.5 | — | One recipe (`MechanicalArmRecipe`), consumes resources correctly per tests. |
| 5 | World / level design | 3.0 | 8.5 | — | One ~150×150 m scene; no verified layout critique on file. |
| 6 | Visual art & presentation | 2.0 | 9.0 | — | `docs/production/known-blockers.md`: Stage A custom art still unapproved; scenes lean on third-party kit assets. |
| 7 | Audio & feedback | 2.0 | 8.5 | — | Non-priority per README; only an `AudioCue` hook exists. |
| 8 | Technical stability | 7.0 | 8.0 | — | `dotnet build`: 0 warnings/errors. `dotnet test`: 71/71 gdUnit4 tests pass. Windows CI smoke-launch exits 0. |
| 9 | Content volume / replayability | 2.0 | 9.0 | — | One map, one enemy, one weapon, ~10–30 min, one ending, by MVP design. |
| 10 | Process integrity of studio claims | 2.0 | n/a | — | `studio/tasks/PRE_BETA_AUDIT_COMPLETE.md` asserts unverifiable FPS/draw-call/GPU numbers and a Windows human playthrough with no supporting artifact; contradicts `known-blockers.md` from the same day. |

**Composite (axes 1–9):** 3.7 / 10 (peer average ≈8.8 / 10)
**Note:** This entry is the baseline `studio/decisions/quality_benchmark_v1.md` was built from. Future entries
track movement from here — the goal is the Δ column trending toward the peer target column, with cited
evidence for every change.
