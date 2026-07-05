# GitHub Workflow: MVP Phase 4–6 Auto-Forge Guide

**Workflow File:** `.github/workflows/mvp-phase-4-6-asset-auto-forge.yml`  
**Purpose:** Automatically generate Phase 4–6 Blender sources and GLB assets, then commit to codebase  
**Status:** ✅ Ready to deploy

---

## Overview

This workflow eliminates the need for manual Blender execution on local machines. Instead:

1. ✅ GitHub Actions spins up an Ubuntu container with Blender 4.0+
2. ✅ Runs `tools/blender/create_mvp_asset_pack_v1.py` to generate 11 MVP assets
3. ✅ Validates Blender sources (mesh integrity, manifold)
4. ✅ Exports all to GLB format
5. ✅ Renders review turntables for visual validation
6. ✅ Updates asset manifest (poly counts, hashes, metadata)
7. ✅ **Automatically commits generated files to the branch**
8. ✅ Pushes changes upstream

**Result:** Phase 7 scene composition can begin immediately after workflow completion.

---

## Triggering the Workflow

### Option 1: Manual Dispatch (Immediate)

1. Go to **GitHub → Actions**
2. Select **"MVP Phase 4–6 Auto-Forge & Commit"** workflow
3. Click **"Run workflow"** button
4. Select branch: `claude/art-docs-phases-1-9-dsd4by` (or your current branch)
5. Click **"Run workflow"**

**Duration:** ~15–20 minutes (Blender generation + export + render)

### Option 2: Scheduled (Automatic)

Workflow runs automatically:
- **Every 6 hours** (cron schedule: `0 */6 * * *`)
- **On push** to any `claude/art-docs-*` branch when briefs or tools change
- **On demand** via workflow_dispatch

### Option 3: Push to Trigger Branch

Push changes to trigger branch:
```bash
git push origin claude/art-docs-phases-1-9-dsd4by
```

If the branch name matches `claude/art-docs-phases-*` and commits touch:
- `docs/art/briefs/brief-*.md`
- `tools/blender/create_mvp_asset_pack_v1.py`
- Workflow file itself

...then the workflow will automatically trigger.

---

## Workflow Execution Timeline

| Step | Duration | Action |
|------|----------|--------|
| Checkout | 30s | Clone repository with full history |
| Install Blender | 2–3 min | Install Blender 4.0+ + dependencies (via apt) |
| Generate sources | 3–5 min | Create 11 Blender `.blend` files programmatically |
| Validate sources | 2 min | Check mesh integrity, manifold errors |
| Export GLBs | 3–5 min | Convert `.blend` → `.glb` (x11 assets) |
| Render turntables | 4–6 min | Generate review images (3 angles per asset) |
| Build manifest | 1 min | Update asset metadata JSON |
| Commit & push | 1 min | Stage, commit, push to branch |
| **Total** | **~15–20 min** | Full pipeline completion |

---

## Generated Artifacts

### Phase 4–6 Assets (11 total)

**Location:** `assets/Production/Generated/MVP_Pack_V1/`

```
TC_PICKUP_Metal_V1.glb           (200 polys, gray block)
TC_PICKUP_Biomass_V1.glb         (250 polys, burgundy cluster)
TC_PICKUP_Electronics_V1.glb     (300 polys, dark crate + cyan glow)
TC_PICKUP_Component_V1.glb       (280 polys, purple crystal)
TC_PROP_SavePoint_V1.glb         (1,300 polys, cyan pillar)
TC_PROP_Workbench_V1.glb         (3,100 polys, orange panel + arm)
TC_PROP_Beacon_Dormant_V1.glb    (1,850 polys, red LED, sealed)
TC_PROP_Beacon_Active_V1.glb     (2,450 polys, purple glow, opened petals)
TC_CHAR_GalaxabrainScout_V1.glb  (2,650 polys, quadruped threat)
TC_PLAYER_MechanicalArm_V1.glb   (800 polys, FPS equipment)

Total: ~12,980 polys (Phase 4–6 only)
```

### Blender Sources

**Location:** `assets/Source/Blender/Production/MVP_Pack_V1/`

All `.blend` files kept for future refinement/iteration.

### Asset Reviews

**Location:** `artifacts/asset-review/`

Turntable renders (PNG, 3 angles per asset) for visual validation before use.

### Asset Manifest

**Location:** `assets/Production/Generated/asset_manifest.json`

Updated with Phase 4–6 entries:
- Asset names and paths
- Poly counts and material counts
- SHA256 hashes (for integrity verification)
- Emissive color info (for visual language validation)

---

## Workflow Permissions

The workflow requires GitHub Actions permissions:

```yaml
permissions:
  contents: write          # Commit to repo
  pull-requests: write     # Create PRs (optional, currently auto-commits)
```

These are automatically granted for workflows in the repository.

---

## Automatic Commit Message

When assets are generated, the workflow creates a commit with:

```
Auto-forge: Generate Phase 4–6 MVP assets (Blender)

Phase 4–6 Auto-Generated Assets:
- 4 Resource Pickups
- 1 Enemy Character
- 1 Player Equipment
- 4 Interactive Props (including 2 Beacon states)

Ready for Phase 7 scene composition.

This is an automated commit from GitHub Actions.
Triggered by: MVP Phase 4–6 Auto-Forge workflow
```

---

## Error Handling

If the workflow fails:

1. **Check workflow logs** (GitHub → Actions → workflow run → logs)
2. **Common failures:**
   - Blender installation timeout → retry workflow
   - Validation error → check Blender script (`tools/blender/create_mvp_asset_pack_v1.py`)
   - Export failure → check GLB export tool (`tools/blender/export_asset.py`)

3. **Manual fallback:**
   ```bash
   # On local machine with Blender installed:
   cd /path/to/TitanCraft
   blender --background --python tools/blender/create_mvp_asset_pack_v1.py
   # Then commit manually
   ```

---

## Next Steps After Workflow Completion

Once workflow succeeds and assets are committed:

1. ✅ **Phase 4–6 assets available** in `assets/Production/Generated/MVP_Pack_V1/`
2. ✅ **Phase 7 can proceed:** Import GLBs into `scenes/CrashSite_MVP.tscn`
3. ✅ **Run Phase 7 validation checklist** (see `PHASE_7_EXECUTION_GUIDE.md`)
4. ✅ **Proceed to Phase 8–9** once Phase 7 composition passes

---

## Monitoring Workflow Runs

### Check Status

1. Go to **GitHub Repository**
2. Click **Actions** tab
3. Select **"MVP Phase 4–6 Auto-Forge & Commit"**
4. View recent runs and status

### Download Artifacts

1. Workflow completes → click run
2. Scroll to **"Artifacts"** section
3. Download **"phase-4-6-assets-generated"** (includes all GLBs)

---

## Customization

### Change Schedule

Edit `.github/workflows/mvp-phase-4-6-asset-auto-forge.yml`:

```yaml
schedule:
  - cron: '0 */6 * * *'  # Every 6 hours (default)
  # Change to:
  - cron: '0 0 * * *'    # Daily at midnight UTC
  # or:
  - cron: '0 */2 * * *'  # Every 2 hours
```

### Change Trigger Branches

```yaml
push:
  branches:
    - 'claude/art-docs-phases-*'  # Current pattern
    # Add more:
    - 'main'
    - 'develop'
```

### Disable Auto-Commit

Remove the commit/push steps to only generate artifacts (for review before committing).

---

## Troubleshooting

| Issue | Solution |
|-------|----------|
| **Workflow won't trigger** | Check branch name matches `claude/art-docs-*` pattern; verify file changes touch trigger paths |
| **Blender installation timeout** | Increase timeout in workflow (default 10 min); may need Blender pre-cached or different base image |
| **Export fails with "permission denied"** | Check output directory exists; verify git write permissions |
| **Commit not appearing on branch** | Check git config (user.email, user.name); verify branch is not protected |
| **Manifest parse error** | Run `python3 tools/blender/build_asset_manifest.py` locally to debug |

---

## Cost & Performance

**GitHub Actions Free Tier:**
- 2,000 free minutes per month (for public repos)
- Each workflow run: ~15–20 minutes
- **Max 6 runs/day** on free tier before hitting limits

For production use, consider:
- GitHub Actions minutes allocation (paid plan)
- Or self-hosted runner with Blender pre-installed

---

## Summary

✅ **Setup complete.** Workflow is ready to auto-generate all Phase 4–6 assets on demand or schedule.

**To trigger immediately:**

1. Go to: **GitHub → Actions → MVP Phase 4–6 Auto-Forge & Commit**
2. Click **"Run workflow"**
3. Select your branch
4. Wait 15–20 minutes

Assets will be committed automatically when complete.

---
