# Workflow Integration Guide - Assets to Codebase

**Purpose:** Automated pipeline that triggers assets through validation, import, and integration  
**Status:** ✅ Ready  
**Created:** 2026-07-05

---

## 🎯 Complete Workflow System

```
┌─────────────────────────────────────────────────────────────────────────┐
│                    TITANCRAFT ASSET & CODE WORKFLOWS                     │
└─────────────────────────────────────────────────────────────────────────┘

                            PR / Push Trigger
                                  │
                    ┌─────────────┴─────────────┐
                    │                           │
                    ▼                           ▼
        ┌────────────────────┐        ┌───────────────────┐
        │  ASSET CHANGES?    │        │  CODE CHANGES?    │
        │  (*.gltf/*.glb)    │        │  (*.cs/*.tscn)    │
        └─────────┬──────────┘        └─────────┬─────────┘
                  │                             │
                  ▼                             ▼
    ┌──────────────────────────┐    ┌──────────────────────────┐
    │   ASSET PIPELINE         │    │   CI PIPELINE            │
    │   asset-pipeline.yml     │    │   ci.yml                 │
    ├──────────────────────────┤    ├──────────────────────────┤
    │ 1. Validate              │    │ 1. Restore Dependencies  │
    │    - Format              │    │    - NuGet packages      │
    │    - Size                │    │ 2. Build                 │
    │    - Naming              │    │    - Debug + Release     │
    │    - Materials           │    │ 3. Test                  │
    │                          │    │    - Unit tests (41)     │
    │ 2. Import               │    │ 4. Godot Import          │
    │    - Godot headless      │    │    - Asset validation    │
    │    - Generate metadata   │    │ 5. Headless smoke test   │
    │                          │    │ 6. Export                │
    │ 3. Test Integration     │    │    - Windows             │
    │    - Unit tests (41)     │    │    - Linux               │
    │    - Godot tests         │    │                          │
    │    - Scene loading       │    │ Duration: ~10 min        │
    │                          │    │ Runs on: Linux + Windows │
    │ 4. Generate Reports     │    │                          │
    │    - Inventory           │    │                          │
    │    - Metrics             │    │                          │
    │                          │    │                          │
    │ 5. Notify Status        │    │                          │
    │    - PR comments         │    │                          │
    │    - Summaries           │    │                          │
    │                          │    │                          │
    │ Duration: ~20 min        │    │                          │
    │ Runs on: Ubuntu Latest   │    │                          │
    └──────────┬───────────────┘    └──────────┬───────────────┘
               │                               │
               └───────────────┬───────────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │  BOTH PASS? ✅       │
                    │                      │
                    │  • Assets validated  │
                    │  • Assets imported   │
                    │  • Tests passing     │
                    │  • Build successful  │
                    │  • Export ready      │
                    └──────────┬───────────┘
                               │
                               ▼
                    ┌──────────────────────┐
                    │  READY FOR MERGE!    │
                    │                      │
                    │  All checks passed ✅ │
                    │  Ready for review    │
                    │  Ready for gameplay  │
                    └──────────────────────┘
```

---

## 📊 Workflow Comparison

### Asset Pipeline (asset-pipeline.yml)
**Triggered By:** Push/PR to `assets/models/**/*.gltf` or `assets/Materials/**/*.tres`

```yaml
Stages:
1. validate-assets (2 min)
   - GLTF format validation
   - File size checking
   - Material format validation
   - Naming convention check

2. import-assets (5 min) [depends: validate-assets]
   - Download Godot 4.2
   - Run headless import
   - Verify metadata

3. integration-test (10 min) [depends: import-assets]
   - Setup .NET
   - Build solution
   - Run 41 unit tests
   - Run Godot integration
   - Validate scenes

4. asset-report (2 min) [depends: integration-test]
   - Create inventory
   - Calculate metrics
   - Upload artifacts
   - Comment on PR

5. notify-status (1 min) [depends: all]
   - Check job results
   - Log final status
   - Output success/failure

Total Duration: ~20 minutes
Runners: Ubuntu Latest (single runner)
Parallel Jobs: Sequential (5 stages)
```

### CI Pipeline (ci.yml)
**Triggered By:** Push/PR to `src/**/*.cs` or `scenes/**/*.tscn` or `tests/**`

```yaml
Build Job (5 min):
  - Restore NuGet packages
  - Build Debug + Release
  - 0 warnings, 0 errors

Test Job (5 min) [depends: build]:
  - Run 41 unit tests
  - All passing ✅

Godot Job (5 min) [depends: build]:
  - Download Godot 4.2
  - Headless import
  - Smoke test
  - Export test

Windows Job (5 min) [depends: build]:
  - Run on Windows Latest
  - Build + Test
  - Export for Windows

Total Duration: ~10 minutes (parallel)
Runners: Ubuntu Latest + Windows Latest
Parallel Jobs: Yes (multiple runners)
```

---

## 🔄 Workflow Integration Points

### 1. Asset Change Flow
```
Art Director commits GLTF to repo
  │
  ├─→ GitHub detects asset file change
  │
  ├─→ Triggers asset-pipeline.yml
  │
  ├─→ Asset Pipeline runs (20 min)
  │   ├─ Validate format, size, naming
  │   ├─ Import to Godot
  │   ├─ Test integration
  │   ├─ Generate report
  │   └─ Notify status
  │
  └─→ PR comment with results
      • Asset inventory
      • Pass/Fail status
      • Next steps
```

### 2. Code Change Flow
```
Gameplay Engineer commits code
  │
  ├─→ GitHub detects code file change
  │
  ├─→ Triggers ci.yml
  │
  ├─→ CI Pipeline runs (10 min)
  │   ├─ Build .NET solution
  │   ├─ Run 41 unit tests
  │   ├─ Import assets in Godot
  │   ├─ Smoke test
  │   └─ Export (Windows + Linux)
  │
  └─→ Check status on PR
      • Build: Pass/Fail
      • Tests: Pass/Fail
      • Export: Success/Failure
```

### 3. Combined Flow (Assets + Code)
```
Both assets and code changes in same PR
  │
  ├─→ Asset Pipeline (20 min)
  │   ✅ Validates new assets
  │   ✅ Tests asset integration
  │   └─→ Reports asset status
  │
  ├─→ CI Pipeline (10 min)
  │   ✅ Builds with new assets
  │   ✅ Runs all tests
  │   ✅ Exports with new assets
  │   └─→ Reports build status
  │
  ├─→ Both pipelines complete
  │
  └─→ PR is ready for review if both pass ✅
```

---

## 🎬 Usage Examples

### Example 1: Art Director Adds New Asset

**Scenario:** Art Director delivers mechanical arm model

```bash
# 1. Receive GLTF file
# Art Director exports: TC_PLAYER_MechanicalArm_V2.gltf

# 2. Add to repository
git checkout -b feature/mechanical-arm-v2
cp ~/Downloads/TC_PLAYER_MechanicalArm_V2.gltf assets/models/mvp_pack_v1/

# 3. Commit
git add assets/models/mvp_pack_v1/TC_PLAYER_MechanicalArm_V2.gltf
git commit -m "Add mechanical arm V2 model"

# 4. Push
git push origin feature/mechanical-arm-v2

# 5. Create PR
# GitHub Actions automatically:
# - Validates GLTF format
# - Imports to Godot
# - Tests scenes that use it
# - Reports status in PR comment
# - Asset is ready for gameplay code integration
```

**Workflow Output:**
```
✅ Asset Pipeline Started
  ✅ Validate: GLTF format OK (3.2 MB)
  ✅ Import: Asset imported to Godot
  ✅ Test: Player scene loads successfully
  ✅ Report: Asset ready for gameplay

Asset is now in codebase and ready for code integration!
```

---

### Example 2: Gameplay Engineer Uses New Asset

**Scenario:** Gameplay engineer integrates new mechanical arm into player

```bash
# 1. Check out feature/mechanical-arm-v2
git checkout feature/mechanical-arm-v2

# 2. Modify Player.tscn to use new model
# In Godot: Link to TC_PLAYER_MechanicalArm_V2.gltf

# 3. Commit gameplay changes
git add scenes/Player/Player.tscn src/Player/FirstPersonController.cs
git commit -m "Integrate mechanical arm V2 into player controller"

# 4. Push
git push origin feature/mechanical-arm-v2

# 5. Update PR (now has asset + code changes)
# GitHub Actions runs:
# - Asset Pipeline: Validates model again
# - CI Pipeline: Builds with new code + asset
# - Tests: All 41 tests pass with new integration
# - Export: Windows + Linux exports succeed
```

**Workflow Output:**
```
✅ Asset Pipeline
  ✅ Asset already validated
  ✅ Integration tests pass with new code

✅ CI Pipeline
  ✅ Build: Success (0 warnings)
  ✅ Tests: 41/41 passing
  ✅ Godot Import: OK
  ✅ Export Windows: Success
  ✅ Export Linux: Success

Both pipelines pass! Ready for merge.
```

---

### Example 3: Manual Workflow Trigger

**Scenario:** Need to re-validate all assets

```bash
# 1. Open GitHub Actions tab
# 2. Select "Asset Pipeline"
# 3. Click "Run workflow"
# 4. Select branch: main
# 5. Click "Run workflow"

# OR via CLI:
gh workflow run asset-pipeline.yml -r main

# Workflow starts immediately:
# - Validates all assets in assets/models/
# - Imports all to Godot
# - Tests integration
# - Reports results
```

---

## ✅ Quality Gates

### For Merging Code with Assets

**Both Must Pass:**

1. **Asset Pipeline** ✅
   - Asset validation: OK
   - Asset import: OK
   - Integration tests: OK
   - Reports: Generated

2. **CI Pipeline** ✅
   - Build: Success
   - Unit tests: 41/41 pass
   - Godot import: OK
   - Export: Success (both platforms)

**Only merge when both show ✅ across all checks**

---

## 🔍 Monitoring & Troubleshooting

### Check Asset Pipeline Status

**Via GitHub:**
1. Go to repository → Actions tab
2. Select "Asset Pipeline"
3. View recent runs
4. Click specific run for details

**Via CLI:**
```bash
# List recent runs
gh run list --workflow=asset-pipeline.yml

# View specific run
gh run view <run-id> --log

# Download artifacts
gh run download <run-id> -n asset-reports
```

---

### Check CI Pipeline Status

**Via GitHub:**
1. Go to repository → Actions tab
2. Select "CI" workflow
3. View matrix (Ubuntu + Windows)
4. Check individual job results

**Via CLI:**
```bash
# List recent runs
gh run list --workflow=ci.yml

# View specific run
gh run view <run-id> --log

# Check export artifacts
gh run download <run-id> -n artifacts
```

---

### Common Issues & Solutions

#### Asset Pipeline Fails at Validation
```
❌ Asset validation failed: File size 75 MB (limit 50 MB)

Solution:
1. Optimize GLTF in 3D tool
2. Compress textures
3. Remove unnecessary data
4. Re-export
5. Commit optimized version
```

#### CI Pipeline Fails at Build
```
❌ Build failed: CS0001 error in FirstPersonController.cs

Solution:
1. Check compile errors
2. Fix code issues locally
3. Run: dotnet build
4. Commit fix
5. Push to trigger workflow again
```

#### Integration Tests Fail
```
❌ Integration test failed: Player scene load timeout

Solution:
1. Run locally: godot --headless
2. Check Player.tscn for issues
3. Verify asset paths
4. Verify collision layers
5. Commit fix and re-push
```

---

## 📈 Performance & Optimization

### Total Pipeline Duration
- **Asset changes only:** ~20 min (asset-pipeline.yml)
- **Code changes only:** ~10 min (ci.yml)
- **Both:** ~20 min (parallel, limited by asset pipeline)

### Optimization Tips
- Keep GLTF files < 50MB
- Use texture compression
- Batch asset commits (multiple files = one run)
- Use manual triggers for re-validation

---

## 🎯 Next Steps

1. **Test Asset Pipeline:**
   ```bash
   # Modify an asset or material file
   git push origin my-branch
   
   # Watch Actions tab for asset-pipeline.yml run
   ```

2. **Test CI Pipeline:**
   ```bash
   # Modify code
   git push origin my-branch
   
   # Watch Actions tab for ci.yml run
   ```

3. **Monitor PR:**
   - Check both workflow status
   - Review reports/artifacts
   - Ensure all checks pass
   - Merge when green ✅

---

## 📚 Related Files

- **Asset Pipeline:** `.github/workflows/asset-pipeline.yml`
- **Asset Workflow Guide:** `.github/ASSET_WORKFLOW.md`
- **CI Pipeline:** `.github/workflows/ci.yml`
- **Project Manifest:** `PROJECT.md`
- **Execution Guide:** `docs/art/execution-guides/phase-4-6-execution-guide.md`

---

## 🔗 Quick Links

| Workflow | File | Docs |
|----------|------|------|
| **Asset** | `.github/workflows/asset-pipeline.yml` | `.github/ASSET_WORKFLOW.md` |
| **CI** | `.github/workflows/ci.yml` | `docs/testing.md` |
| **Project** | `PROJECT.md` | `.github/PROJECT_OVERVIEW.md` |

---

## ✨ Summary

The **Workflow Integration System** provides:

✅ **Automated Asset Validation**
- Format, size, naming, material checks
- Runs immediately on asset changes

✅ **Seamless Import**
- Godot headless import
- Metadata generation
- Caching for faster builds

✅ **Comprehensive Testing**
- 41 unit tests
- Godot integration tests
- Scene loading validation

✅ **Clear Quality Gates**
- Both pipelines must pass to merge
- Asset + Code changes validated together
- Exports verified on all platforms

✅ **Full Transparency**
- PR comments show results
- Artifact reports available
- Easy troubleshooting

**Result:** Assets flow smoothly from Art Director → GitHub → Codebase → Tests → Production 🎉

---

**Created:** 2026-07-05  
**Status:** ✅ Active  
**Maintainers:** Engine Architect + Claude Code
