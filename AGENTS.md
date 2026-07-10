# AGENTS.md — TitanCraft Agent Studio Constitution

## 1. Source of Truth

- Read `README.md` before every task.
- `README.md` is the Genesis Source of Truth for product scope, gameplay, platforms, and MVP boundaries.
- If a request conflicts with `README.md`, stop and report the contradiction before changing files.
- `AGENTS.md` defines execution behavior only; it must never expand gameplay scope.

## 2. Current Product Boundary

TitanCraft is a solo, offline-first, Windows-first FPS built with Godot 4 .NET and C#. The current playable scope is the MVP **Crash Site**.

Forbidden MVP work includes:

- multiplayer;
- grappling hook;
- wall running;
- procedural world;
- voxels;
- large mech;
- complete rocket;
- multiple maps;
- multiple enemy types;
- cloud services;
- remote telemetry.

## 3. Global Agent Behavior

All agents must operate as conservative repo maintainers:

1. read the task;
2. read relevant files;
3. verify `README.md` constraints;
4. run or simulate `python3 tools/agent_preflight.py "<task description>"` and read the packet before editing files;
5. produce a plan of three to seven steps;
6. modify the minimum necessary files;
7. avoid gameplay code unless explicitly requested and permitted by `README.md`;
8. run applicable validation commands;
9. report the task packet summary, evidence, tests, manual checks, limitations, and final verdict.

Agents must not invent requirements, claim exhaustive knowledge, fake evidence, bypass failing gates, or mark release readiness without machine-readable proof.

## 4. File Authority

- `studio/agents/` defines specialized operating roles.
- `studio/memory/` stores indexed atomic memory cards; memories are curated working context, not exhaustive truth.
- `studio/skills/` stores repeatable workflows.
- `studio/decisions/` stores Architecture Decision Records.
- `studio/prompts/` stores reusable prompts.
- `studio/checklists/` stores review and validation checklists.
- `studio/templates/` stores reusable governance templates.

## 5. Required Agent File Contract

Every file in `studio/agents/` must declare:

- mission;
- authority;
- forbidden actions;
- inputs;
- outputs;
- verdicts.

Allowed verdict vocabulary for governance artifacts:

- `PASS`
- `FAIL_REPO_OWNED`
- `HUMAN_BLOCKED`
- `ENVIRONMENT_BLOCKED`
- `EXTERNAL_SECRET_BLOCKED`
- `INTENTIONAL_GATE`
- `DRY_RUN_ONLY`
- `NOT_GO`
- `GO`

## 6. Required Memory File Contract

Every file in `studio/memory/` must use indexed atomic memory cards. Each card must include:

- stable ID;
- topic;
- atomic statement;
- source reference;
- confidence;
- last reviewed date.

Memory files are not exhaustive and must not override `README.md`.

## 7. Required Skill File Contract

Every file in `studio/skills/` must define a repeatable workflow with:

- purpose;
- trigger;
- inputs;
- ordered steps;
- outputs;
- validation;
- verdict.

## 8. Agent Studio Preflight

Before editing files, agents must run or simulate the Agent Studio preflight packet:

```bash
python3 tools/agent_preflight.py "<task description>"
```

If command execution is unavailable, agents must generate the same packet by using `tools/agent_task_router.py` and the checked-in Studio indexes. Agents may not ignore scope warnings from the packet. Missing packet evidence requires a blocking verdict rather than speculative implementation.

The final report must include a task packet summary with task category, primary agent, required memories, required skills, required evidence, validation commands run, and final verdict.

Packet evidence rules:

- Visual tasks must require PNG evidence and visual diagnosis before visual success is claimed.
- Gameplay tasks must require gameplay validation, including relevant integration or smoke test evidence.
- Asset-related tasks must require provenance evidence, including source, licence, hash, and audition evidence.
- Agents may not use vague verdicts such as `done`, `improved`, `looks good`, `should be fine`, or `tests passed`.
- Final verdicts must use only the approved verdict vocabulary in this file and the routed packet.

## 9. Validation

Run only commands applicable to the files changed. Preferred checks are:

```bash
dotnet restore
dotnet build
godot --headless --path . --import
git status --short
```

For markdown-only governance changes, run markdown lint if available. If markdown lint is unavailable, run:

```bash
git diff --check
```

## 10. Pull Request Rule

Pull requests must include evidence, tests, manual verification, risks, and a final verdict. A PR must not claim memories are exhaustive or imply gameplay scope has changed.

<!-- ISSAABUU_COMPANY_OS_BRIDGE_V1 -->
## Issaabuu Company OS Bridge

**Effective:** 2026-07-10  
**Authority:** Founder-approved cross-repository governance

This repository participates in the Issaabuu Company OS.

### Authority boundary

- Repository-local `README.md`, `AGENTS.md`, tests, schemas, ADRs, and release artifacts remain authoritative for product scope, implementation, validation, and technical truth.
- OneDrive `/Issaabuu_Management_Inc/` is authoritative for company strategy, capital allocation, cross-product priorities, legal/financial/partnership records, company KPIs, and external claims.
- GitHub is authoritative for code, commits, pull requests, tests, deployments, and technical evidence.
- Conversations, model memory, generated summaries, and local notes are never canonical evidence.

### Required Company OS references

For material cross-company decisions, read through an authorized OneDrive connector or synchronized folder:

1. `00_Master_Index/Issaabuu_Constitution_v2_0_Canonical.md`
2. `00_Master_Index/SOURCE_OF_TRUTH_REGISTRY.md`
3. `00_Master_Index/AI_OPERATING_PROTOCOL.md`
4. `COMPANY_STATE.json`
5. `MASTER_BRIEFING.md` and `MASTER_TASKS.md`

If those files are unavailable, do not reconstruct them from memory. Continue only with repository-local technical work and mark company-level conclusions `SOURCE_UNAVAILABLE`.

### Execution contract

- One agent per branch/worktree. Never let Codex and Claude Code edit the same branch concurrently.
- Business-document proposals go to the appropriate OneDrive agent staging lane; code changes go through reviewable GitHub branches and pull requests.
- Separate repository capability, environment proof, production proof, and market proof.
- Never convert tests or repository artifacts into user, revenue, partnership, or production claims without the required external evidence.
- Never store secret values in repository instructions, prompts, logs, evidence, or agent memory.
- Material plans must include a falsifiable thesis, evidence, definition of done, rollback, and a ten-step dependency horizon. Only the first unblocked step becomes active.

### Conflict rule

When repository instructions and Company OS state appear to conflict, freeze the disputed cross-company action, preserve repository safety, identify the exact contradiction, and escalate it through the Company OS decision and evidence ledgers. Do not silently choose a source.
<!-- /ISSAABUU_COMPANY_OS_BRIDGE_V1 -->
