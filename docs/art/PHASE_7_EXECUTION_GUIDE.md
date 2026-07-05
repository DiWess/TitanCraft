# Phase 7 Execution Guide: Scene Composition
## Crash Site MVP Complete Diorama Assembly

**Phase:** 7  
**Scope:** Integrate Phases 1–6 assets into single gameplay-ready scene  
**Effort:** ~2 hours (terrain placement, prop positioning, camera setup, visual validation)  
**Status:** `READY_FOR_COMPOSITION` (blocked on Phase 3–6 completion)  
**Next Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` (after composition validation)

---

## Prerequisites

All Phase 1–6 assets must be available before composition begins:

**Phase 1: Terrain**
- `TC_TERRAIN_CrashBasin_V1.glb` — Base terrain geometry (8,000–12,000 polys)

**Phase 2: Hull**
- `TC_CRASH_HullMk1_V1.glb` — Crashed spacecraft hull (hero asset)
- `TC_HeavyCrashHull_V1.glb` — Additional hull variant (optional secondary)

**Phase 3: Interactables**
- `TC_PROP_Workbench_V1.glb` — Crafting station (orange accent)
- `TC_PROP_Beacon_Dormant_V1.glb` — Beacon dormant state (red LED)
- `TC_PROP_Beacon_Active_V1.glb` — Beacon active state (purple glow)
- `TC_PROP_SavePoint_V1.glb` — Checkpoint marker (cyan glow)

**Phase 4–6: Interactive Props & Characters**
- `TC_PICKUP_Metal_V1.glb`, `Biomass_V1.glb`, `Electronics_V1.glb`, `Component_V1.glb`
- `TC_CHAR_GalaxabrainScout_V1.glb` — Enemy antagonist
- `TC_PLAYER_MechanicalArm_V1.glb` — Player equipment (mounted on avatar)

---

## Scene Composition Strategy

### Visual Hierarchy & Placement Zones

The Crash Site scene is organized in **visual layers** from background to foreground:

```
BACKGROUND (Distant Reference):
  • Ridge Rim terrain (defines basin edge, 60–80m away)
  • Hull silhouette (far side of basin)

MIDGROUND (Gameplay Navigation):
  • Terrain ash floor and basalt formations
  • Basalt foreground/midground rocks
  • Fractured/broken terrain (impact zone)

FOREGROUND (Interactive & Immediate):
  • Workbench (central hub, ~30m from spawn)
  • Save Point (near workbench, visible access)
  • Beacon (far side of basin, ~50m visibility goal)
  • Pickups (scattered resource clusters)

PLAYER:
  • Spawn location (~0, 0, player_height)
  • First-person camera (1.7m eye height)
  • Mechanical arm (FPS POV, lower-right corner)
```

### Route Landmarks (Gameplay Flow)

Terrain generation already defines route markers. Scene composition confirms spatial relationships:

```
SPAWN → RESOURCE_CLUSTER_1 → WORKBENCH/SAVE_POINT
             ↓
        RESOURCE_CLUSTER_2 → ARENA → BEACON
```

**Design Intent:** Player spawns, explores for resources, upgrades at workbench, attempts arena combat, activates beacon for escape.

---

## Composition Workflow

### Step 1: Terrain Placement (20 min)

**Objective:** Import and position terrain as base layer

```gdscript
# In Godot (pseudo-code):
var terrain = load("res://assets/glb/MVP_Pack_V1/TC_TERRAIN_CrashBasin_V1.glb")
terrain.position = Vector3(0, 0, 0)  # Origin
add_child(terrain)
```

**Validation:**
- [ ] Terrain imports without errors
- [ ] Y-axis vertical (0, 0, Z = up)
- [ ] Ash floor is flat and navigable
- [ ] Basalt formations are distributed across visible terrain
- [ ] Ridge rim visible at horizon distance (~60–80m)
- [ ] Impact crater (fractured ground) is obvious centerpoint

### Step 2: Hull Placement (15 min)

**Objective:** Position crashed hull as primary landmark

**Placement Strategy:**
- **Location:** Opposite far side of basin from player spawn
- **Distance:** ~50m from spawn (long-distance visual destination)
- **Orientation:** Tilted crash angle (not upright) for visual interest
- **Height:** Partially embedded in terrain (suggests impact history)

```gdscript
var hull = load("res://assets/glb/MVP_Pack_V1/TC_CRASH_HullMk1_V1.glb")
hull.position = Vector3(45, 30, 5)  # Far side, elevated terrain
hull.rotation_degrees = Vector3(15, -30, 8)  # Crash tilt
add_child(hull)
```

**Validation:**
- [ ] Hull visible from spawn (long-distance silhouette obvious)
- [ ] Positioning avoids clipping with terrain
- [ ] Crash angle suggests impact (tilted, not upright)
- [ ] Secondary hull variant (if used) is offset from primary

### Step 3: Interactive Props Placement (30 min)

#### 3A: Workbench (Central Hub)

**Location:** Central basin area, ~25–30m from spawn  
**Orientation:** Facing player approach direction (usually spawn → resources)

```gdscript
var workbench = load("res://assets/glb/MVP_Pack_V1/TC_PROP_Workbench_V1.glb")
workbench.position = Vector3(0, 25, 0)  # Central, medium distance
workbench.rotation_degrees = Vector3(0, 180, 0)  # Face toward spawn
add_child(workbench)
```

**Validation:**
- [ ] Accessible from multiple angles (not wedged between rocks)
- [ ] On level terrain (not tilted or partially sunken)
- [ ] Orange panel visible from approach angle
- [ ] Arm silhouette readable (not obscured by rocks)
- [ ] Distance ~10m+ from spawn (requires navigation)

#### 3B: Save Point (Checkpoint Anchor)

**Location:** Near workbench, visible as safety/refuge  
**Orientation:** Vertical (Z-axis up)

```gdscript
var save_point = load("res://assets/glb/MVP_Pack_V1/TC_PROP_SavePoint_V1.glb")
save_point.position = Vector3(-8, 22, 0)  # Left of workbench
save_point.rotation_degrees = Vector3(0, 0, 0)  # Vertical
add_child(save_point)
```

**Validation:**
- [ ] Positioned as "refuge" (close to workbench, not isolated)
- [ ] Cyan glow visible from approach
- [ ] Not obscured by props or terrain
- [ ] Scale feels personal and non-threatening

#### 3C: Beacon (Victory Objective)

**Location:** Far side of basin, elevated or prominent  
**Orientation:** Vertical, commanding presence

```gdscript
# Dormant state (shown during exploration)
var beacon_dormant = load("res://assets/glb/MVP_Pack_V1/TC_PROP_Beacon_Dormant_V1.glb")
beacon_dormant.position = Vector3(50, 35, 5)  # Far side, elevated
beacon_dormant.rotation_degrees = Vector3(0, 45, 0)  # Slight angle for interest
add_child(beacon_dormant)

# Active state (instantiated on trigger)
# Note: Godot script swaps dormant → active on player activation
```

**Validation:**
- [ ] Visible from spawn at distance (~20m+)
- [ ] Elevated position emphasizes objective status
- [ ] Red dormant glow visible at distance
- [ ] Clear sightline from workbench area (suggests progression path)

### Step 4: Pickup Scatter (20 min)

**Objective:** Distribute resource pickups across terrain

**Pickup Clusters:** ~5–8 total pickups, grouped by proximity

**Cluster 1: Near Spawn (Early Exploration)**
- 1–2 pickups, mix of types
- Distance: ~8–12m from spawn
- Purpose: First resource discovery

**Cluster 2: Midfield (Resource Zone)**
- 2–3 pickups, near basalt formations
- Distance: ~20–25m from spawn
- Purpose: Main resource gathering area

**Cluster 3: Arena/Danger Zone (Challenge)**
- 1–2 pickups, near fractured terrain/impact crater
- Distance: ~35–45m from spawn
- Purpose: High-risk resource collection (near enemy spawn)

**Placement Strategy:**
```gdscript
var pickups = [
  # Cluster 1
  {asset: "TC_PICKUP_Metal_V1.glb", pos: Vector3(5, 8, 0.1)},
  {asset: "TC_PICKUP_Electronics_V1.glb", pos: Vector3(-3, 10, 0.1)},
  # Cluster 2
  {asset: "TC_PICKUP_Biomass_V1.glb", pos: Vector3(-15, 20, 0.1)},
  {asset: "TC_PICKUP_Component_V1.glb", pos: Vector3(12, 22, 0.1)},
  # Cluster 3
  {asset: "TC_PICKUP_Metal_V1.glb", pos: Vector3(25, 40, 0.2)},
]

for pickup in pickups:
  var asset = load("res://assets/glb/MVP_Pack_V1/" + pickup.asset)
  asset.position = pickup.pos
  add_child(asset)
```

**Validation:**
- [ ] Pickups grouped by proximity (visible as clusters)
- [ ] Mix of types distributed (no single type dominates)
- [ ] Accessible without traversing danger zones initially
- [ ] Slight elevation variance (natural resting on terrain)

### Step 5: Enemy Placement (10 min)

**Objective:** Position Scout antagonist(s) in arena zone

**Scout Placement Strategy:**
- **Location:** Arena/impact crater zone, ~35–50m from spawn
- **Spawn Height:** Slightly elevated terrain, visible when player approaches
- **Quantity:** 1 (MVP single-enemy focus)
- **Initial State:** Dormant or distant (becomes active when player approaches arena)

```gdscript
var scout = load("res://assets/glb/MVP_Pack_V1/TC_CHAR_GalaxabrainScout_V1.glb")
scout.position = Vector3(35, 45, 0.5)  # Arena zone
scout.rotation_degrees = Vector3(0, 0, 0)  # Neutral stance
add_child(scout)
# Godot: Attach AI controller for threat behavior (separate from mesh)
```

**Validation:**
- [ ] Positioned in high-risk zone (arena, not near spawn)
- [ ] Visible from a distance (player can see threat approaching)
- [ ] Purple glow visible (threat indicator)
- [ ] Spindly legs do not clip terrain

### Step 6: Player Avatar Setup (15 min)

**Objective:** Configure first-person player with mechanical arm equipment

**Player Configuration:**
```gdscript
var player = CharacterBody3D.new()
player.position = Vector3(0, 0, 1.7)  # Spawn point (eye height)

# First-person camera
var camera = Camera3D.new()
camera.position = Vector3(0, 0, 0)  # At eye level
player.add_child(camera)
player.set_current_camera_active()

# Mechanical arm (lower-right FPS view)
var arm = load("res://assets/glb/MVP_Pack_V1/TC_PLAYER_MechanicalArm_V1.glb")
arm.position = Vector3(0.15, -0.3, -0.5)  # Lower-right corner, centered FOV
arm.parent = camera  # Attach to camera (moves with view)
camera.add_child(arm)

# Player movement/input (handled by Godot scripts)
add_child(player)
```

**Validation:**
- [ ] Camera at 1.7m height (standing eye level)
- [ ] Arm visible in lower-right corner (not blocking vision)
- [ ] Arm does not disappear when rotating camera
- [ ] Arm scale appropriate (menacing, not oversized)

---

## Camera & Viewport Setup

### First-Person Perspective (Gameplay)

```gdscript
# Godot Camera3D settings
var camera = Camera3D.new()
camera.fov = 75  # Moderate FOV (immersive but not distorted)
camera.near = 0.1
camera.far = 1000  # Draw distance covers 50m+ to beacon
```

### Review/Turntable Cameras (Documentation)

Create secondary cameras for review artifact capture:

```gdscript
# Overhead camera (shows scene layout)
var overhead = Camera3D.new()
overhead.position = Vector3(25, 25, 50)  # Above basin center
overhead.look_at(Vector3(25, 25, 0), Vector3(0, 0, 1))

# Hero angle (cinematic view)
var hero_cam = Camera3D.new()
hero_cam.position = Vector3(15, -20, 10)  # Approach angle
hero_cam.look_at(Vector3(25, 25, 5), Vector3(0, 0, 1))
```

---

## Composition Validation Checklist

### Geometry & Placement

- [ ] Terrain base layer imports and displays correctly
- [ ] Hull positioned on far side, visible from spawn
- [ ] Workbench accessible from spawn (~25–30m distance)
- [ ] Save point near workbench, obvious refuge
- [ ] Beacon on far side, ~50m away (long-distance objective)
- [ ] Pickups grouped in three clusters with variety
- [ ] Scout in arena zone, not near spawn or workbench
- [ ] No clipping between assets and terrain
- [ ] No floating geometry (all items rest on terrain or elevated naturally)

### Visual Hierarchy & Readability

- [ ] Terrain silhouette clear (ash floor, basalt formations, ridge rim)
- [ ] Hull is second-largest silhouette (after terrain, before props)
- [ ] Workbench is focal point (central, approach-able)
- [ ] Save point reads as "refuge" (near workbench)
- [ ] Beacon is distant objective (far side, elevated)
- [ ] Pickups read as "resource clusters" (grouped, not scattered randomly)
- [ ] Scout is obvious threat (arena zone, menacing stance)

### Material & Lighting

- [ ] All materials display correctly (no black spots or corruption)
- [ ] Orange (workbench) glow visible
- [ ] Red (beacon dormant) LED visible
- [ ] Cyan (save point) glow visible
- [ ] Purple (scout) threat core visible
- [ ] Emissive strengths appropriate (visible at gameplay distance)
- [ ] Color language supports visual distinction (orange ≠ red ≠ cyan ≠ purple)

### First-Person Gameplay

- [ ] Player spawn at origin, eye height 1.7m
- [ ] Camera FOV feels natural (75° recommended)
- [ ] Mechanical arm visible in lower-right corner (not intrusive)
- [ ] Scene draws to 50m+ (beacon visible from spawn)
- [ ] Navigation paths are clear (no impossible terrain)
- [ ] Interactive props are reachable without glitching
- [ ] Scout is visible from distance (threat imminent)

### Distance Visibility (Recap of Phase 3)

- [ ] Workbench readable at 10+ meters
- [ ] Beacon visible at 20+ meters
- [ ] Save point readable at 10+ meters
- [ ] All three props visually distinct
- [ ] Pickups identifiable from 5–10 meters
- [ ] Scout menacing from 15+ meters

---

## Asset Size & Performance Budget

| Asset | Poly Count | Estimated Size | Notes |
|-------|-----------|----------------|-------|
| Terrain (Phase 1) | ~10,000 | ~2–3 MB | Large, persistent |
| Hull (Phase 2) | ~12,000 | ~3–4 MB | Far-side landmark |
| Workbench (Phase 3) | ~3,100 | ~5 MB | Central interactive |
| Beacon Dormant (Phase 3) | ~1,850 | ~3 MB | Distant objective |
| Beacon Active (Phase 3) | ~2,450 | ~5 MB | (Swapped in on activation) |
| Save Point (Phase 3) | ~1,300 | ~2 MB | Central safety zone |
| Pickups (Phase 4) | ~1,030 total | ~2–3 MB | Scattered resources |
| Scout (Phase 5) | ~2,650 | ~4 MB | Arena threat |
| Mech Arm (Phase 6) | ~1,800 | ~2 MB | FPS overlay |
| **TOTAL (Static)** | **~35,130** | **~30 MB** | Guideline: ≤50M tri for MVP |

**Performance Note:** Total poly count is moderate for a modern gaming engine. Godot can easily handle this with LOD culling for distant assets (terrain, hull).

---

## Execution Checklist

### Pre-Composition

- [ ] All Phase 1–6 GLB files available and validated
- [ ] Asset manifest updated with all entries and hashes
- [ ] Review artifacts (turntables, PNGs) captured for all assets
- [ ] Phase 3 visibility testing completed (all props readable at distance)

### Composition Phase

- [ ] Create new Godot scene: `Main.tscn` or `CrashSite_MVP.tscn`
- [ ] Import terrain (Phase 1)
- [ ] Import and position hull (Phase 2)
- [ ] Import and position interactive props (Phase 3)
- [ ] Scatter pickups (Phase 4)
- [ ] Place Scout enemy (Phase 5)
- [ ] Configure player avatar with mechanical arm (Phase 6)
- [ ] Set up first-person camera
- [ ] Validate no clipping/floating geometry
- [ ] Test from player first-person POV

### Validation Phase

- [ ] Geometry & placement validation (checklist above)
- [ ] Visual hierarchy review
- [ ] Distance visibility confirmation (all objects readable)
- [ ] Material/glow display check
- [ ] FPS camera test (natural FOV, proper height)
- [ ] Navigation path test (can player move freely?)
- [ ] Performance check (frame rate stable?)

### Documentation Phase

- [ ] Capture overhead turntable (scene layout)
- [ ] Capture hero angle (cinematic approach view)
- [ ] Capture first-person POV (gameplay perspective)
- [ ] Document any visual issues or refinements needed
- [ ] Record final scene file and path

---

## Expected Scene Structure (Godot)

```
Main.tscn (Scene Root)
├── Terrain (Node3D)
│   └── TC_TERRAIN_CrashBasin_V1 (mesh import)
├── Hull (Node3D)
│   └── TC_CRASH_HullMk1_V1 (mesh import)
├── Props (Node3D)
│   ├── Workbench
│   │   └── TC_PROP_Workbench_V1 (mesh import)
│   ├── SavePoint
│   │   └── TC_PROP_SavePoint_V1 (mesh import)
│   ├── Beacon
│   │   ├── Dormant (TC_PROP_Beacon_Dormant_V1, initially active)
│   │   └── Active (TC_PROP_Beacon_Active_V1, hidden, swapped on trigger)
├── Pickups (Node3D)
│   ├── Cluster_1 (Node3D)
│   │   ├── Metal_01
│   │   └── Electronics_01
│   ├── Cluster_2 (Node3D)
│   │   ├── Biomass_01
│   │   └── Component_01
│   └── Cluster_3 (Node3D)
│       └── Metal_02
├── Enemies (Node3D)
│   └── Scout_01
│       └── TC_CHAR_GalaxabrainScout_V1 (mesh import + AI script)
└── Player (CharacterBody3D)
    ├── Camera3D
    │   └── MechanicalArm
    │       └── TC_PLAYER_MechanicalArm_V1 (mesh import, FPS overlay)
    ├── Collider (CapsuleShape3D)
    └── InputHandler (script for WASD, interact key, etc.)
```

---

## Next Steps

After Phase 7 Scene Composition `ASSET_IMPLEMENTATION_PASS`:

1. **Phase 8: Lighting & Materials Refinement** (~1.5 hours)
   - Add directional lights (sun), ambient lighting
   - Adjust material emissive strengths based on scene lighting
   - Optional: Add fog/atmospheric effects

2. **Phase 9: Visual Artifact Factory** (~1 hour)
   - Render final turntable PNGs (multiple angles)
   - Capture gameplay screenshots (first-person views)
   - Create video flythrough (optional)
   - Generate asset manifest final report

3. **Final Gate:** `VISUAL_SLICE_GAMEPLAY_SAFE` — Scene ready for gameplay integration and testing

---

## Troubleshooting: Common Composition Issues

| Issue | Cause | Solution |
|-------|-------|----------|
| **Clipping between props** | Objects too close or overlapping | Increase separation; check placement coordinates |
| **Floating geometry** | Asset position above terrain | Lower Y/Z coordinate; verify terrain height |
| **Material looks wrong** | Lighting not configured | Add ambient light; check emissive strength |
| **Pickups not visible** | Too small or obscured by rocks | Increase size slightly; reposition to clear zones |
| **Scout legs clipping** | Terrain has sharp peaks | Smooth terrain locally or adjust scout height |
| **Camera view too narrow** | FOV too high | Reduce FOV to 75°; check near/far clipping planes |
| **Performance degradation** | Too many polys or draw distance | Enable LOD; reduce draw distance; check scene complexity |

---

## Success Criteria

✅ **Phase 7 is `ASSET_IMPLEMENTATION_PASS` when:**

1. All Phase 1–6 assets imported and positioned in single scene
2. Terrain forms navigable base with visible landmarks
3. Hull visible as far-side objective
4. Workbench accessible from spawn, interactive zone clear
5. Save point positioned as refuge near workbench
6. Beacon dormant state visible at distance, ready for activation
7. Pickups clustered and accessible (mixed types)
8. Scout placed in arena/danger zone
9. Player spawns at origin with functional first-person camera
10. Mechanical arm visible in FPS corner (not intrusive)
11. All materials display correctly (no corruption)
12. Emissive glows (orange, red, cyan, purple) visible at gameplay distance
13. No clipping or floating geometry
14. Scene navigable without collision issues
15. Performance stable (target: 60 FPS on target hardware)
16. Visual hierarchy clear (terrain → hull → props → player)
17. Turntable/hero angle screenshots captured
18. Scene file saved: `scenes/CrashSite_MVP.tscn`

---

## Timeline & Dependencies

```
Phase 3: Visibility Testing (1 hour, parallel with Phase 4–6 automation)
Phase 4–6: Asset Generation (15 min, automated workflow)
    ↓
Phase 7: Scene Composition (2 hours, sequential)
    ↓
Phase 8: Lighting/Materials (1.5 hours, sequential)
    ↓
Phase 9: Visual Artifacts (1 hour, sequential)
    ↓
FINAL GATE: VISUAL_SLICE_GAMEPLAY_SAFE
```

**Estimated total elapsed time after Phase 3 begins:** ~5–6 hours (including automation wait)
