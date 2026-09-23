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

## 11. Ownership Rights

`studio/indexes/ownership.yml` is the machine-readable source of truth for which agent owns which repository path. Prose statements in `studio/agents/*.md` describe an agent's domain; the index decides file authority.

Resolve ownership before editing:

```bash
python3 tools/agent_ownership.py <path>
```

### Rights Vocabulary

- `agent_write`: the owning agent may author changes to the path inside an authorized task, then satisfy the listed reviewers before claiming `PASS`.
- `human_approval_required`: no agent may change the path without explicit human instruction in the task. The listed owner acts as steward and reviewer only. `README.md`, `AGENTS.md`, `CLAUDE.md`, and `PROJECT_DIRECTOR_START_HERE.md` are human-owned.

### Rules

1. One accountable owner per path. Reviewers may block; reviewers may not silently author.
2. An agent must not write a path owned by another agent. Request the change from the owner and escalate to the Producer if the owner disagrees.
3. When several patterns match, the most specific path wins. `scenes/UI/**` outranks `scenes/**`.
4. An owner may not be listed as its own reviewer.
5. A path that matches no entry is unowned. Route it to the Producer and add it to the index before changing it; do not assume ownership by editing.
6. Review-only roles own no paths. Owning nothing is a valid, enforced state, not a gap to fill.
7. Ownership grants write authority only. It never grants scope authority: an owner may not expand MVP scope beyond `README.md` inside a path it owns.

### Enforcement

Every file in `studio/agents/` must declare an `## Owned Paths` section listing exactly the paths the index assigns to it. `python3 tools/validate_agent_studio.py` fails when an agent file and the index disagree, when an owner or reviewer does not exist, when a path is duplicated, or when a human-owned path is opened to agent writes.

Changing ownership is a governance change: update `studio/indexes/ownership.yml` and the affected agent files in the same commit, then run `python3 tools/validate_agent_studio.py` and `python3 tools/test_agent_ownership.py`.
