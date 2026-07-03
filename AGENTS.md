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
4. produce a plan of three to seven steps;
5. modify the minimum necessary files;
6. avoid gameplay code unless explicitly requested and permitted by `README.md`;
7. run applicable validation commands;
8. report evidence, tests, manual checks, limitations, and final verdict.

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

## 8. Validation

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

## 9. Pull Request Rule

Pull requests must include evidence, tests, manual verification, risks, and a final verdict. A PR must not claim memories are exhaustive or imply gameplay scope has changed.
