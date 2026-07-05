# Asset Pipeline Workflow - Complete Guide

**Purpose:** Automated validation, import, and testing of art assets  
**Status:** ✅ Active  
**Last Updated:** 2026-07-05

---

## 🎯 Workflow Overview

The **Asset Pipeline Workflow** is a GitHub Actions automation that:

1. **Validates** assets (format, size, naming conventions)
2. **Imports** assets into Godot (generates metadata)
3. **Tests** asset integration (unit + integration tests)
4. **Reports** results (inventory, metrics, status)
5. **Notifies** stakeholders (PR comments, summaries)

---

## 📋 Workflow Stages

### Stage 1: Validate Assets
**Job:** `validate-assets`  
**Runner:** Ubuntu Latest  
**Duration:** ~2 minutes

**Steps:**
1. Validate GLTF/GLB file format
2. Check file sizes (warn if > 50MB)
3. Validate Material (.tres) files
4. Check asset naming conventions (TC_TYPE_Name_V#.gltf)
5. Generate validation report

**Success Criteria:**
- ✅ All GLTF files present and valid
- ✅ File sizes reasonable (< 50MB)
- ✅ Materials have valid Godot format
- ✅ Naming follows convention

**Output:**
```
✅ GLTF Files Validated
✅ File Sizes Checked
✅ Materials Verified
✅ Naming Conventions Validated
```

---

### Stage 2: Import Assets
**Job:** `import-assets`  
**Runner:** Ubuntu Latest  
**Duration:** ~3-5 minutes
**Depends On:** `validate-assets`

**Steps:**
1. Download Godot 4.2 binary
2. Run `godot --headless --import` to import all assets
3. Verify .godot import metadata generated
4. Generate import report

**Success Criteria:**
- ✅ Godot downloads successfully
- ✅ `--headless --import` completes without errors
- ✅ Godot metadata (.godot directories) created
- ✅ All resources catalogued

**Output:**
```
✅ Godot Version 4.2 Downloaded
✅ Assets Imported Successfully
✅ Metadata Generated
✅ Resources Catalogued
```

**Behind the Scenes:**
- Godot reads each GLTF file
- Extracts meshes, materials, skeletons
- Generates binary .scn/.res files
- Creates import metadata for caching
- Validates texture paths and dependencies

---

### Stage 3: Integration Testing
**Job:** `integration-test`  
**Runner:** Ubuntu Latest  
**Duration:** ~5-10 minutes
**Depends On:** `import-assets`

**Steps:**
1. Setup .NET 8.0 environment
2. Download Godot binary
3. Restore .NET dependencies
4. Build solution (Debug)
5. Run unit tests (41 tests)
6. Run Godot integration tests
7. Validate asset loading in scenes

**Success Criteria:**
- ✅ All 41 unit tests pass
- ✅ Integration tests pass
- ✅ Asset scenes load without errors
- ✅ Collision shapes valid
- ✅ Materials apply correctly

**Test Coverage:**
- Asset scene loading
- Material application
- Collision & physics
- Gameplay integration
- HUD binding
- Save/load functionality

**Output:**
```
✅ Unit Tests: 41/41 Passing
✅ Integration Tests: PASSED
✅ Scene Loading: OK
✅ Material Application: OK
```

---

### Stage 4: Generate Reports
**Job:** `asset-report`  
**Runner:** Ubuntu Latest  
**Duration:** ~2 minutes
**Depends On:** All previous jobs

**Steps:**
1. Create asset inventory (list all assets with sizes)
2. Calculate asset metrics (counts, total size, status)
3. Upload reports to artifacts
4. Comment on PR with results
5. Generate final status summary

**Reports Generated:**
- `asset-inventory.txt` — List of all assets
- `asset-metrics.txt` — Counts and metrics
- GitHub PR comment — Inline results

**Sample Output:**
```
# TitanCraft Asset Inventory

## GLTF Models
- TC_PROP_Beacon_Active_V1.gltf (2.5 MB)
- TC_PROP_Beacon_Dormant_V1.gltf (2.3 MB)
- TC_PROP_Workbench_V1.gltf (3.1 MB)
- ...

## Materials
- BeaconActive.tres
- BeaconDormant.tres
- ...

# Asset Metrics Report

- Total GLTF/GLB Models: 10
- Total Materials: 5
- Total Asset Size: 25 MB
- Pipeline Status: ✅ PASSING
```

---

### Stage 5: Notify Status
**Job:** `notify-status`  
**Runner:** Ubuntu Latest  
**Duration:** ~1 minute
**Depends On:** All jobs

**Steps:**
1. Determine overall pipeline status
2. Log status message
3. Output success/failure message

**Status Determination:**
- ✅ PASSED: All jobs succeeded
- ❌ FAILED: Any job failed

**Output:**
```
✅ Asset pipeline completed successfully!

📦 Assets are now:
  ✅ Validated (format, size, naming)
  ✅ Imported (Godot metadata generated)
  ✅ Tested (integration & scene loading)
  ✅ Documented (inventory & metrics)

🎮 Assets are ready for gameplay integration!
```

---

## 🚀 How to Trigger the Workflow

### Automatic Triggers

**Push to any branch:**
```bash
git add assets/models/mvp_pack_v1/TC_*.gltf
git commit -m "Add new asset: Scout enemy model"
git push origin my-feature-branch
```

**Modify materials:**
```bash
git add assets/Materials/Landmarks/BeaconActive.tres
git commit -m "Update beacon active material"
git push origin my-feature-branch
```

### Manual Trigger

1. Go to **GitHub Actions** → **Asset Pipeline**
2. Click **Run workflow** → Select branch
3. Click **Run workflow** button

Or via GitHub CLI:
```bash
gh workflow run asset-pipeline.yml
```

---

## 📊 Workflow Diagram

```
START
  │
  ├─→ [validate-assets] ──────────────────┐
  │    - Check GLTF format                 │
  │    - Check file sizes                  │
  │    - Validate materials                │
  │    - Check naming                      │
  │    - Duration: ~2 min                  │
  │                                        │
  └────────────────────────────────────────┴─→ [import-assets] ────────────────┐
                                              - Download Godot 4.2             │
                                              - Run headless import            │
                                              - Verify metadata                │
                                              - Duration: ~5 min               │
                                                                               │
                                              ┌─────────────────────────────────┘
                                              │
                                              ├─→ [integration-test] ──────────┐
                                              │    - Setup .NET                 │
                                              │    - Build solution             │
                                              │    - Run tests (41)             │
                                              │    - Run Godot integration      │
                                              │    - Duration: ~10 min          │
                                              │                                 │
                                              │    ┌──────────────────────────┐ │
                                              │    │ [asset-report]         │ │
                                              │    │ - Create inventory     │ │
                                              │    │ - Calculate metrics    │ │
                                              │    │ - Upload artifacts     │ │
                                              │    │ - Comment on PR        │ │
                                              │    │ - Duration: ~2 min     │ │
                                              │    └──────────────────────────┘ │
                                              │    │ [notify-status]        │ │
                                              │    │ - Check all jobs       │ │
                                              │    │ - Log results          │ │
                                              │    │ - Duration: ~1 min     │ │
                                              │    └──────────────────────────┘ │
                                              │                                  │
                                              └──────────────────────────────────┘
                                                       │
                                                       ▼
                                                      END
                                         ✅ PASSED or ❌ FAILED
```

---

## ✅ Validation Rules

### GLTF File Validation
```
✅ File format: .gltf or .glb
✅ File location: assets/models/
✅ File size: < 50MB (warning if larger)
✅ Valid JSON structure
```

### Naming Convention
```
Pattern: TC_TYPE_Name_V#.ext

Examples:
✅ TC_PROP_Workbench_V1.gltf
✅ TC_CHAR_GalaxabrainScout_V1.gltf
✅ TC_PLAYER_MechanicalArm_V1.gltf
✅ TC_PICKUP_Metal_V1.gltf

Invalid:
❌ Workbench.gltf (missing TC_ prefix)
❌ TC_workbench.gltf (lowercase type)
❌ TC_PROP_Workbench.gltf (missing version)
```

### Material Validation
```
✅ File format: .tres (Godot resource)
✅ Header: [gd_resource type="..."
✅ File location: assets/Materials/
```

---

## 🐛 Troubleshooting

### Asset Validation Fails

**Issue:** "❌ GLTF file size exceeds 50MB"

**Solution:**
- Optimize mesh (reduce polygons)
- Compress textures (use WebP or ASTC)
- Remove unnecessary data from GLTF
- Check brief for poly budget

**Example:**
```bash
# Check file size
ls -lh assets/models/mvp_pack_v1/TC_PROP_Workbench_V1.gltf

# If > 50MB, optimize in your 3D tool and re-export
```

---

### Import Fails

**Issue:** "❌ Asset import failed"

**Solution:**
1. Check Godot logs for errors
2. Verify GLTF structure (valid JSON)
3. Check material paths (relative refs)
4. Ensure no missing dependencies
5. Try Godot import with GUI on local machine

**Example:**
```bash
# Local test
godot --headless --path . --import

# Watch for error messages in stdout
```

---

### Integration Tests Fail

**Issue:** "❌ Integration tests failed"

**Solution:**
1. Run tests locally: `dotnet test`
2. Check if asset scene loads: `scenes/Main/Main.tscn`
3. Verify collision layers (layer 2, mask 1)
4. Check material assignments in scenes
5. Review test output for specific failures

**Example:**
```bash
# Run tests locally
dotnet test tests/TitanCraft.Tests.csproj --verbosity normal

# Review output for asset-related failures
```

---

## 📈 Performance Metrics

| Stage | Duration | Status |
|-------|----------|--------|
| Validate Assets | ~2 min | Fast ✅ |
| Import Assets | ~5 min | Medium ⏱️ |
| Integration Test | ~10 min | Slower ⏳ |
| Generate Reports | ~2 min | Fast ✅ |
| Notify Status | ~1 min | Fast ✅ |
| **Total** | **~20 min** | **Acceptable** |

---

## 🔐 Security & Artifact Handling

### Artifacts Uploaded
- `asset-reports/asset-inventory.txt`
- `asset-reports/asset-metrics.txt`

### Artifact Retention
- Default: 90 days (GitHub Actions default)
- Can be manually deleted
- Only contains metadata, not binary assets

### PR Comments
- Automatic (for pull requests)
- Contains inventory & metrics
- Visible to reviewers

---

## 📝 Best Practices

### For Art Directors
1. ✅ Follow naming convention: `TC_TYPE_Name_V#.gltf`
2. ✅ Keep file sizes < 50MB
3. ✅ Use relative material paths
4. ✅ Test export before committing
5. ✅ Check workflow results

### For Developers
1. ✅ Don't commit invalid GLTF files
2. ✅ Always run workflow before merging
3. ✅ Review PR comment for asset status
4. ✅ Ensure tests pass on all platforms
5. ✅ Update material references if paths change

### For QA/Testers
1. ✅ Check artifact reports for inventory
2. ✅ Download reports from artifact store
3. ✅ Verify asset counts match expected
4. ✅ Test asset loading in gameplay
5. ✅ Report asset-related issues with workflow logs

---

## 🔄 Integration with CI/CD

The asset pipeline is **independent** of the main CI pipeline but **complements** it:

**Main CI Pipeline (.github/workflows/ci.yml):**
- Build + Test (C#)
- Godot import validation
- Export artifacts (Windows + Linux)

**Asset Pipeline (.github/workflows/asset-pipeline.yml):**
- Asset-specific validation
- Import + integration testing
- Asset inventory & reporting

**Relationship:**
```
PR with asset changes
  ├─→ Asset Pipeline (validates assets)
  └─→ CI Pipeline (builds project with assets)
      └─→ Integration tests (full gameplay)
```

---

## 🎯 Next Steps

1. **Commit this workflow:** `git add .github/workflows/asset-pipeline.yml`
2. **Test trigger:** Push asset changes or manually trigger
3. **Monitor results:** Check Actions tab → Asset Pipeline
4. **Review reports:** Download artifacts for inventory
5. **Integrate feedback:** Adjust rules based on first runs

---

## 📞 Questions?

- **Workflow Issues:** Check job logs in GitHub Actions
- **Asset Validation:** Review `.github/ASSET_WORKFLOW.md`
- **Godot Import:** Check `.github/workflows/ci.yml`
- **Test Results:** View integration test output

---

**Workflow File:** `.github/workflows/asset-pipeline.yml`  
**Status:** ✅ Active  
**Last Updated:** 2026-07-05  
**Maintained By:** Engine Architect + Claude Code
