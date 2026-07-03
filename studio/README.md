# TitanCraft Agent Studio

TitanCraft Agent Studio is the repository-local operating system for agent governance, routing, memory retrieval, skills, decisions, prompts, checklists, and evidence gates. It is a production knowledge base, not gameplay code.

`README.md` remains the product source of truth. Root `AGENTS.md` remains the global execution constitution. Studio memories are curated and non-exhaustive.

## Structure

| Path | Purpose |
|---|---|
| `agents/` | Specialist role contracts with authority, required memories, required skills, rejection rules, and escalation rules. |
| `memory/` | Indexed atomic memory cards for scope, visual failures, Godot safety, C# safety, assets, CI, prompts, gates, architecture, and debugging. |
| `skills/` | Practical repeatable workflows with required inputs, procedures, automatic failures, output format, and evidence. |
| `indexes/` | Routing maps for agents, memories, skills, verdict vocabulary, and evidence requirements. |
| `decisions/` | Architecture Decision Record templates. |
| `prompts/` | Reusable task prompts with placeholders for scope, evidence, tests, and verdicts. |
| `checklists/` | Short fail-closed checklists for task start, scene edits, visual claims, commits, PRs, merges, assets, gameplay, and release readiness. |
| `templates/` | Reusable templates for new agents, memory cards, and skills. |

## How Agents Select Memories

1. Read the task and identify signals such as `screenshot`, `Godot scene`, `asset import`, `MVP scope`, `Stage A`, or `runtime flags`.
2. Open `studio/indexes/memory_routing.yml` and load the listed memory cards.
3. Load additional memory packs only when the changed files or task language require them.
4. Treat memory cards as operational lessons, not exhaustive truth; verify against `README.md` and actual repository files.

## How Agents Select Skills

1. Classify the task type.
2. Open `studio/indexes/skill_routing.yml`.
3. Execute each listed skill workflow in order.
4. If a required input is missing, return a blocking verdict rather than inventing evidence.

## How Task Routing Works

Use `studio/indexes/agent_routing.yml` to select one primary agent and the relevant secondary reviewers. The primary agent owns the verdict for its domain, while secondary agents identify risks in adjacent domains.

Examples:

- Visual scene composition routes to Art Director, Visual Reviewer, Technical Director, and QA Lead.
- Gameplay bugs route to Gameplay Engineer, QA Lead, and Technical Director.
- Build failures route to Build Release Engineer, Tools Engineer, and QA Lead.

## How Evidence Gates Work

`studio/indexes/evidence_requirements.yml` defines the minimum evidence by category. A task cannot receive `PASS` if required evidence is missing. Runtime correctness, visual quality, asset provenance, and release readiness are separate gates.

Forbidden vague verdicts are defined in `studio/indexes/verdicts.yml` and include `done`, `improved`, `looks good`, `should be fine`, and `tests passed`.

## Mandatory Agent Preflight

Agents must run or generate an Agent Studio preflight packet before editing files for any future task:

```bash
python3 tools/agent_preflight.py "Review Stage A screenshots before Stage B starts"
```

For deterministic machine-readable output, run:

```bash
python3 tools/agent_preflight.py "Review Stage A screenshots before Stage B starts" --json
```

The preflight command imports the checked-in router logic from `tools/agent_task_router.py`, then adds the before-editing checklist, forbidden actions, forbidden scope, validation expectations, and final-report requirements. It does not call a network service and does not replace `README.md` review.

### When Preflight Is Mandatory

Preflight is mandatory before real work begins, including documentation-only governance tasks. If an agent cannot run the command, it must simulate the packet from `tools/agent_task_router.py` and the files in `studio/indexes/` before editing. Agents may not ignore scope warnings; if required evidence cannot be produced, use an approved blocking verdict.

### How to Paste the Packet into PRs

1. Run the preflight command before editing files.
2. Copy the packet summary into the PR template section `Agent Studio Task Packet`.
3. Include task category, primary agent, required memories, required skills, required evidence, validation commands run, and final verdict.
4. Keep the final verdict inside the approved packet vocabulary and avoid vague verdicts.

### How Codex Should Use the Packet

Codex should treat the packet as a pre-edit gate: read `README.md`, generate the packet, load the listed memories and skills, apply the listed checklists, and only then modify the minimum necessary files. The final report must summarize the packet and show how validation satisfied the packet.

### Preflight Examples

Visual task:

```bash
python3 tools/agent_preflight.py "Review visual screenshot PNG route slab composition before Stage A approval"
```

Expected evidence includes PNG screenshots and a visual diagnosis naming focal point, route readability, silhouette, scale, and material coherence.

Gameplay task:

```bash
python3 tools/agent_preflight.py "Fix gameplay bug where player inventory mission pickup fails"
```

Expected evidence includes gameplay validation such as integration tests when systems interact or a mission smoke test/manual procedure.

Asset task:

```bash
python3 tools/agent_preflight.py "Import asset OBJ with provenance licence source URL hash and audition"
```

Expected evidence includes source URL, licence, file hash, audition screenshot, and production/placeholder/rejected/reference-only classification.

## How to Add a New Memory

1. Add one atomic card to the most specific file in `studio/memory/`, or create a new pack if no pack fits.
2. Include all required fields: `id`, `title`, `tags`, `applies_when`, `memory`, `avoid`, `required_action`, `evidence_required`, `related_agents`, and `related_skills`.
3. Add the pack to `studio/memory/index.yml` if it is new.
4. Add retrieval signals to `studio/indexes/memory_routing.yml` when agents should find it automatically.
5. Run `python3 tools/validate_agent_studio.py`.

## How to Add a New Skill

1. Create a markdown file in `studio/skills/`.
2. Include required headings: `purpose`, `when_to_use`, `required_inputs`, `procedure`, `automatic_failures`, `output_format`, `evidence_required`, `example_good_output`, and `example_bad_output`.
3. Add it to `studio/skills/index.yml`.
4. Add task routing in `studio/indexes/skill_routing.yml`.
5. Run `python3 tools/validate_agent_studio.py`.

## How to Add a New Agent

1. Create a markdown file in `studio/agents/`.
2. Include mission, authority, forbidden actions, required inputs, required outputs, required memories, required skills, review questions, automatic rejection conditions, approved verdicts, and escalation rules.
3. Add or update routes in `studio/indexes/agent_routing.yml`.
4. Run `python3 tools/validate_agent_studio.py`.

## Validation

Run:

```bash
python3 tools/validate_agent_studio.py
python3 tools/test_agent_task_router.py
python3 tools/test_agent_preflight.py
git diff --check
```

If markdown lint exists locally, run it before opening a PR.

## How to Run the Task Router

Use the router before editing files when a task might need Agent Studio routing:

```bash
python3 tools/agent_task_router.py "Review Stage A screenshots before Stage B starts"
```

The router reads only checked-in Studio indexes and uses simple keyword matching. It does not call a network service and is not an AI system.

## How to Interpret a Task Packet

A task packet contains:

- `detected_task_category`: the route selected from `studio/indexes/agent_routing.yml`.
- `primary_agent` and `secondary_agents`: the required review roles.
- `required_memory_packs_cards`: memory cards to load before planning.
- `required_skills`: workflows to execute before claiming progress.
- `required_checklists`: short gates to apply before editing, committing, or reviewing.
- `required_evidence`: minimum artifacts needed for the category.
- `forbidden_verdicts` and `approved_final_verdicts`: allowed language for final status.
- `minimum_validation_commands`: the smallest command set expected for the routed task.
- `scope_warnings`: likely failure modes to avoid.

Future Codex prompts should paste or reference the task packet before implementation. If the packet requires evidence that is unavailable, the agent should return a blocking verdict instead of editing files or inventing proof.

## How to Add a Rehearsal

1. Add a markdown file under `studio/rehearsals/` with the required `expected_*` sections.
2. Make the task realistic and tied to a TitanCraft failure lesson.
3. Generate its expected packet:

```bash
python3 tools/agent_task_router.py "TASK DESCRIPTION" > studio/rehearsals/expected_packets/name.json
```

4. Run:

```bash
python3 tools/test_agent_task_router.py
```

The router test must assert behavior, not just file existence.
