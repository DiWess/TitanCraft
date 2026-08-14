# Fast Lane Checklist

Use only when the task is inside Polish Mode Fast Lane (see `docs/production/polish-mode.md`).

## Before editing

- [ ] Task is a small bug fix, feel/juice tweak, HUD/prompt/audio tweak, or docs-only update
- [ ] Task does **not** change scene structure, collisions, production assets, or scope
- [ ] No new systems, enemies, maps, or resources

## During work

- [ ] Change is minimal and limited to the listed files
- [ ] No unrequested visual or architecture changes

## Before claiming done

- [ ] `dotnet build` (or equivalent) succeeds
- [ ] Relevant unit/integration tests pass if behavior changed
- [ ] Short manual check noted if feel/combat/movement is affected
- [ ] Verdict is `PASS` or `FAIL_REPO_OWNED` only

## Exit Fast Lane if any of these appear

- Visual claim or PNG review needed
- Scene / collision / navmesh change
- Asset import or Stage gate
- Release or quality-benchmark claim
- Any scope expansion beyond Crash Site MVP
