# Phase 1 Execution Guide: Crash Basin Terrain
## Step-by-Step Implementation

**Status:** Ready to Execute  
**Brief Reference:** `docs/art/brief-terrain-crash-basin.md`  
**Estimated Time:** ~2 hours  
**Success Gate:** `ASSET_IMPLEMENTATION_PASS`  

---

## Prerequisites Checklist

Before starting Phase 1, confirm:

- [ ] Blender 4.0+ installed on your machine
- [ ] Godot 4 .NET available for validation
- [ ] Git access to clone/push changes
- [ ] ~2 hours of continuous work time
- [ ] Brief specifications understood (`docs/art/brief-terrain-crash-basin.md`)

---

## Three Execution Paths

### **Path A: Blender Python Script (Recommended)**

This is the fastest path if Blender is available.

#### Step 1: Run Terrain Generation Script

```bash
cd /home/user/TitanCraft
blender --background --python tools/blender/create_terrain_crash_basin_v1.py
```

**What this does:**
- Generates 9,000–12,000 poly terrain mesh
- Creates 6 material zones (Ash Floor, Basalt rocks, Ridge Rim, Scorch/Dust)
- Exports to GLB: `assets/Production/Generated/Terrain/TC_TERRAIN_CrashBasin_V1.glb`
- Generates manifest entry with SHA256 hash
- Reports status and next steps

**Expected output:**
```
============================================================
TitanCraft Phase 1: Crash Basin Terrain Generation
============================================================

=== Generating Crash Basin Terrain ===

Generating ash floor...
  Ash floor: 1089 vertices, 2048 faces
Generating basalt foreground...
  Basalt foreground: 1200 vertices
...
✓ Total geometry: 9100 vertices, 9200 faces
  Target: 8,000–12,000 polys → Achieved: ~9200

=== Exporting to GLB ===

✓ Exported: assets/Production/Generated/Terrain/TC_TERRAIN_CrashBasin_V1.glb

=== Generating Manifest Entry ===

Asset ID: TC_TERRAIN_CrashBasin_V1
File size: 456789 bytes
SHA256: a1b2c3d4e5f6...
Poly count: 9100

✓ Manifest updated: assets/Production/Generated/asset_manifest.json
  Total assets in manifest: 1

============================================================
✓ PHASE 1 TERRAIN: ASSET_IMPLEMENTATION_PASS
============================================================
```

#### Step 2: Validate Import in Godot

```bash
cd /home/user/TitanCraft
godot --headless --path . --import
```

**Check for:**
- No import errors or warnings
- No shader/material mismatches
- GLB loads successfully

**Expected output:**
```
[INFO] Importing scene: res://assets/Production/Generated/Terrain/TC_TERRAIN_CrashBasin_V1.glb
[INFO] Successfully imported; asset ready for use
```

If there are errors, check:
- Material node graph compatibility
- Texture path resolution
- GLB export settings (format, materials, normals)

#### Step 3: Capture Neutral-Gray Turntable PNG

In Godot Editor or via script:

```gdscript
# From a test scene or viewer
var terrain = preload("res://assets/Production/Generated/Terrain/TC_TERRAIN_CrashBasin_V1.glb")
var scene = terrain.instantiate()
add_child(scene)

# Capture screenshot (6 angles)
for angle in range(0, 360, 60):
    # Rotate camera to angle
    camera.rotation.y = deg_to_rad(angle)
    await get_tree().process_frame
    # Save screenshot with neutral gray rendering
    get_viewport().get_texture().get_image().save_png(...)
```

Or use Blender:

```bash
blender --background assets/models/terrain/TC_TERRAIN_CrashBasin_V1.glb \
  --render-frame 1 \
  --render-output "artifacts/review/terrain-v1/TC_TERRAIN_CrashBasin_turntable_"
```

**Output:**
- `artifacts/review/terrain-v1/TC_TERRAIN_CrashBasin_turntable_001.png` (0°)
- `artifacts/review/terrain-v1/TC_TERRAIN_CrashBasin_turntable_002.png` (60°)
- ... (360° total)

#### Step 4: Validate Route Readability

Load terrain in Godot test scene:

```gdscript
# Create test scene with terrain
var terrain_scene = preload("res://assets/Production/Generated/Terrain/TC_TERRAIN_CrashBasin_V1.glb").instantiate()
var player_scene = preload("res://scenes/Player/Player.tscn").instantiate()

add_child(terrain_scene)
add_child(player_scene)

# Validate walkability
# - Spawn area accessible? ✓
# - Resource trail visible? ✓
# - Workbench approach clear? ✓
# - Arena entry readable? ✓
# - Beacon area visible? ✓
# - No stuck/clipping? ✓
```

**Checklist:**
- [ ] Player can walk spawn → resources
- [ ] Player can reach workbench area
- [ ] Arena entrance is clear
- [ ] No collision issues
- [ ] Route silhouettes are readable (no ambiguous navigation)

#### Step 5: Report Phase 1 Outcome

Once validated, create a Phase 1 completion report:

```bash
git status  # Confirm changes
git diff --stat  # Review file changes

# Commit Phase 1
git add -A
git commit -m "feat: Complete Phase 1 terrain asset (Crash Basin Mk1)

- Terrain geometry: 9,100 polys across 6 material zones (Ash Floor, Basalt Foreground/Midground, Fractured Ground, Ridge Rim, Scorch/Dust)
- Generated via: tools/blender/create_terrain_crash_basin_v1.py
- Export: assets/models/terrain/TC_TERRAIN_CrashBasin_V1.glb (456KB)
- Manifest: Updated with SHA256 a1b2c3d4...
- Validation: Godot import successful, no errors
- Route readability: All waypoints (spawn→resources→workbench→arena→beacon) verified
- Status: ASSET_IMPLEMENTATION_PASS

Next: Phase 2 (Crashed Hull hero asset)

Co-Authored-By: Claude Haiku 4.5 <noreply@anthropic.com>"

git push origin claude/application-upgrade-requirements-93suhg
```

---

### **Path B: Manual Blender Authoring**

If you prefer to model in Blender GUI:

#### Step 1: Open Blender and Create New Project

```
File → New → General
```

#### Step 2: Follow Brief Specifications

Reference: `docs/art/brief-terrain-crash-basin.md`

Build geometry:
1. **Ash Floor** (150m × 150m, ~1,000 polys)
   - Plane with subdivisions
   - Subtle height variation (sine wave depression)
   - Material: Dusty gray

2. **Basalt Foreground** (left/right framing rocks, ~2,500 polys)
   - UV Spheres or boxes with heavy bevels
   - Placed at ±40m X, 0m Z
   - Dark gray material

3. **Basalt Midground** (route markers, ~2,000 polys)
   - Placed at resource zones, arena entry
   - Varying scales and rotation for natural look

4. **Fractured Ground** (impact zone, ~1,500 polys)
   - Radial broken terrain geometry
   - Center at 0, –5
   - Irregular heights

5. **Ridge Rim** (basin edge, ~1,500 polys)
   - Arc from –75m to +75m X, ~80m Z
   - Height rising to 25m
   - Distant backdrop visual

6. **Scorch/Dust** (detail accents, ~500 polys)
   - Small patches scattered on ash floor
   - Dark brown burn marks

#### Step 3: Apply Materials

Create materials per brief:

| Material | Albedo | Roughness | Metalness |
|----------|--------|-----------|-----------|
| Ash Floor | (140, 140, 140) | 0.8 | 0.0 |
| Basalt | (80, 80, 90) | 0.7 | 0.0 |
| Scorch | (60, 50, 40) | 0.85 | 0.0 |
| Dust Accent | (180, 180, 175) | 0.9 | 0.0 |

#### Step 4: Export to GLB

```
File → Export → glTF 2.0 (.glb/.gltf)
Filename: TC_TERRAIN_CrashBasin_V1.glb
Location: assets/models/terrain/

Options:
  - Format: Binary (.glb)
  - Include Animations: OFF
  - Include All Bone Influences: OFF
  - Export Materials: ON
  - Export Textures: ON (if external textures used)
```

#### Step 5: Validate & Manifest

Follow Steps 2–5 from **Path A** (Godot validation, turntable capture, manifest generation).

---

### **Path C: Procedural Generation (Advanced)**

If you want to extend the Python script or use Godot GDScript:

#### Option C1: Extend Python Script

Edit `tools/blender/create_terrain_crash_basin_v1.py` to add:
- Perlin noise for natural height variation
- Better rock cluster geometry
- Custom material properties

#### Option C2: Godot GDScript Generation

Create `src/World/TerrainGenerator.cs`:

```csharp
using Godot;

public class TerrainGenerator : Node3D
{
    [Export]
    public int PolyBudget = 9000;

    [Export]
    public Vector3 BasinSize = new Vector3(150, 30, 150);

    public void Generate()
    {
        // Create mesh
        var mesh = new ArrayMesh();

        // Add ash floor surface
        AddAshFloor(mesh);

        // Add basalt rocks
        AddBasaltFormations(mesh);

        // Add fractured ground
        AddFracturedGround(mesh);

        // Export and save
        var trimesh = mesh.CreateTrimeshShape();
        // ... export logic
    }

    private void AddAshFloor(ArrayMesh mesh)
    {
        // Implementation
    }

    // ... other methods
}
```

Then call from scene setup:

```gdscript
# In Main.tscn script
@onready var terrain_gen = $TerrainGenerator
func _ready():
    terrain_gen.Generate()
```

---

## Validation Checklist

Mark as you complete each step:

**Geometry:**
- [ ] Poly count: 8,000–12,000 ✓
- [ ] Material zones defined: 6 zones ✓
- [ ] No overlapping faces or internal geometry ✓
- [ ] Collision-clean walkable surfaces ✓

**Godot Import:**
- [ ] GLB imports without errors ✓
- [ ] Textures/materials resolve ✓
- [ ] No visual corruption ✓
- [ ] Player can walk all surfaces ✓

**Route Readability:**
- [ ] Spawn area accessible ✓
- [ ] Resource trail visible ✓
- [ ] Workbench approach clear ✓
- [ ] Arena entry readable ✓
- [ ] Beacon zone accessible ✓

**Visual Quality:**
- [ ] Terrain does not read flat/plate-like ✓
- [ ] Basalt rocks provide framing ✓
- [ ] Ash/basalt distinction clear ✓
- [ ] Silhouette readable in neutral gray ✓

**Asset Management:**
- [ ] GLB exported with valid format ✓
- [ ] Manifest entry created ✓
- [ ] SHA256 hash recorded ✓
- [ ] Turntable PNG captured (6 angles) ✓

---

## Troubleshooting

### Problem: GLB import fails in Godot

**Cause:** Material nodes not compatible, texture paths broken

**Solution:**
1. In Blender, remove custom node graphs
2. Use only Principled BSDF material
3. Bake textures if using procedural materials
4. Re-export with "Include Textures" enabled

### Problem: Collision is broken (player falls through terrain)

**Cause:** Mesh collision not auto-generated

**Solution:**
```gdscript
# In Godot, add collision manually:
var mesh_instance = $CrashBasinTerrain
var static_body = StaticBody3D.new()
var collision = CollisionShape3D.new()
collision.shape = mesh_instance.mesh.create_trimesh_shape()
static_body.add_child(collision)
add_child(static_body)
```

### Problem: Poly count exceeds budget

**Cause:** Too many subdivisions

**Solution:**
1. Reduce subdivision level on large surfaces
2. Combine similar rocks into single geometry
3. Remove unnecessary detail from distant areas
4. Target: 8,000–10,000 polys max

### Problem: Route not visually readable

**Cause:** Terrain too flat or landmarks not obvious

**Solution:**
1. Increase height variation on ash floor (±1–2m instead of ±0.5m)
2. Scale basalt rocks larger (height 8–10m instead of 5m)
3. Place fractured ground more obviously in center
4. Ensure distinct material zones (ash vs. basalt contrast)

---

## Success Criteria

✅ **Phase 1 Complete when:**

1. ✓ GLB file generated and committed
2. ✓ Manifest entry created with SHA256 hash
3. ✓ Godot imports without errors
4. ✓ Poly count in target range (8,000–12,000)
5. ✓ Route landmarks all identifiable
6. ✓ Player walkability verified
7. ✓ Turntable PNG captured (minimum 3 angles)
8. ✓ Neutral-gray silhouette readable
9. ✓ No collision regressions
10. ✓ Committed to branch with clear commit message

**Gate:** `ASSET_IMPLEMENTATION_PASS` issued

**Next:** Proceed to Phase 2 (Crashed Hull hero asset)

---

## Next Steps (After Phase 1 Complete)

1. Push Phase 1 to remote branch
2. Review Phase 2 brief: `docs/art/brief-crash-hull-mk1.md`
3. Begin Phase 2 terrain/hull composition study (how hull fits in basin)
4. Start Phase 2 Blender work (2–3 hours)

**Timeline:** Phase 1 complete → Phase 2 start (immediate, no gate blocking)

---

## Support & Questions

If you hit blockers:
1. Check the brief: `docs/art/brief-terrain-crash-basin.md`
2. Review master plan: `docs/art/crash-site-worldclass-visual-master-plan.md`
3. Consult visual identity: `docs/art/titancraft-visual-identity.md`
4. Post-execution feedback to: `docs/art/VISUAL_UPGRADE_STATUS_2026-07-05.md`

Good luck! Report Phase 1 completion once:
- GLB exported
- Godot validated
- Turntable captured
- Manifest updated
