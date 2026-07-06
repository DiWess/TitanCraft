# Phase 7.4: Scene Integration Guide — Comprehensive Instructions

**Purpose:** Complete visual guide for integrating Phase 7.4 code systems into Main.tscn

**Audience:** Developer with Godot 4 editor access  
**Duration:** 30–60 minutes  
**Difficulty:** Intermediate (node attachment, property configuration)  

---

## Pre-Integration Checklist

Before opening Godot:

- [x] Phase 7.4 code complete (all 3 files committed and in src/)
- [x] Project compiles with 0 errors, 0 warnings
- [x] Main.tscn exists and loads without errors
- [x] Backup Main.tscn (copy to Main.tscn.backup)
- [x] Have this guide open while working

**If any of these fail:** Stop and run `dotnet build` to verify compilation first.

---

## Overview: Three Systems to Integrate

Phase 7.4 adds three independent systems to the game. Each requires:

```
System → Script Attachment → Node Configuration → Property Setup → Testing
```

| System | Script | Where | Node Type | Parent |
|---|---|---|---|---|
| **1. Combat Tutorial** | `src/Player/CombatTutorialHint.cs` | Canvas layer | Control | CanvasLayer |
| **2. Resource Locator UI** | `src/World/ResourceLocatorUI.cs` | HUD/Canvas | Control | CanvasLayer |
| **3. Interactive Highlight** | `src/World/InteractiveObjectHighlight.cs` | Scene nodes | (self) | Workbench, Beacon |

---

## System 1: Combat Tutorial Hint

### Overview

**What it does:** Shows "THREAT DETECTED" hint when scout is first detected  
**Visual:** Text overlay on screen, fades out after 8 seconds  
**Location:** Canvas layer (UI system)  
**Complexity:** Low (single Control node + Label child)

### Step-by-Step Integration

#### Step 1.1: Open Main Scene

1. Open Godot editor
2. Load `scenes/Main/Main.tscn` (double-click in FileSystem)
3. You should see your game world with player, environment, etc.
4. Verify scene loads without errors (no red error messages in editor)

#### Step 1.2: Create CanvasLayer

1. In Scene tree (left panel), select the **root node** (Main or equivalent)
2. Right-click → **Add Child Node**
3. Search for **CanvasLayer**
4. Click to create (now you have a CanvasLayer node under Main)
5. **Rename** the CanvasLayer to **`UICanvas`** (right-click → Rename)

**Expected Scene Tree After:**
```
Main
├── [existing nodes...]
└── UICanvas (CanvasLayer) ← NEW
```

#### Step 1.3: Create Control Node (CombatTutorialUI)

1. Select the **UICanvas** node (click on it)
2. Right-click → **Add Child Node**
3. Search for **Control**
4. Click to create
5. **Rename** to **`CombatTutorialUI`**

**Expected Scene Tree After:**
```
Main
├── [existing nodes...]
└── UICanvas (CanvasLayer)
    └── CombatTutorialUI (Control) ← NEW
```

#### Step 1.4: Attach CombatTutorialHint Script

1. Select **CombatTutorialUI** node
2. In Inspector (right panel), find **Script** section
3. Click the folder icon next to Script field
4. Navigate to `res://src/Player/CombatTutorialHint.cs`
5. Double-click to attach the script
6. **Verify:** Inspector should show the script name and [Export] properties below

**Expected in Inspector:**
```
Script: res://src/Player/CombatTutorialHint.cs ✓
DisplayDurationSeconds: 8.0
FadeOutDurationSeconds: 1.5
```

#### Step 1.5: Create HintLabel Child

1. Select **CombatTutorialUI** node
2. Right-click → **Add Child Node**
3. Search for **Label**
4. Click to create
5. **Rename** to **`HintLabel`** (exact name - script looks for this)

**Expected Scene Tree After:**
```
Main
├── [existing nodes...]
└── UICanvas (CanvasLayer)
    └── CombatTutorialUI (Control)
        └── HintLabel (Label) ← NEW
```

#### Step 1.6: Configure HintLabel (Appearance)

1. Select **HintLabel** node
2. In Inspector, configure these properties:

**Layout:**
- Layout → Anchor Left: 0.5 (center horizontally)
- Layout → Anchor Top: 0.25 (upper 1/4 of screen)
- Layout → Anchor Right: 0.5
- Layout → Anchor Bottom: 0.35
- Layout → Offset Left: -200 (extend left from center)
- Layout → Offset Top: 0
- Layout → Offset Right: 200 (extend right from center)
- Layout → Offset Bottom: 50

**Text:**
- Text → Alignment: Center (both horizontal and vertical)
- Text → Text: (leave empty - script sets this)

**Font:**
- Label → Theme → Default Font Size: 24
- Label → Font → (leave default)

**Appearance (Optional Background):**
- Add a `PanelContainer` as parent if you want a dark background behind text
- Or skip this step (text-only is fine)

**Color:**
- Label → Theme Override Colors → Font Color: White (255, 255, 255, 255)

**Expected Visual:**
- Large white text centered on upper portion of screen
- Will show: "THREAT DETECTED\nMaintain distance and attack from range\nMove: WASD | Attack: Mouse Click | Reload: R\n[Hint will auto-dismiss]"

#### Step 1.7: Save Scene

Press **Ctrl+S** (Cmd+S on Mac) to save Main.tscn

**Expected:** No error messages, scene saves successfully

---

## System 2: Resource Locator UI

### Overview

**What it does:** Shows "🟠 METAL NEARBY" indicator when resources are within 15m  
**Visual:** Text indicator on screen (colored emoji + text)  
**Location:** HUD/Canvas layer (same as Combat Tutorial)  
**Complexity:** Low (Control node + Label child)

### Step-by-Step Integration

#### Step 2.1: Create Control Node (ResourceLocatorUI)

1. Select **UICanvas** node (from System 1)
2. Right-click → **Add Child Node**
3. Search for **Control**
4. Click to create
5. **Rename** to **`ResourceLocatorUI`**

**Expected Scene Tree After:**
```
Main
├── [existing nodes...]
└── UICanvas (CanvasLayer)
    ├── CombatTutorialUI (Control)
    │   └── HintLabel (Label)
    └── ResourceLocatorUI (Control) ← NEW
```

#### Step 2.2: Attach ResourceLocatorUI Script

1. Select **ResourceLocatorUI** node
2. In Inspector, find **Script** field
3. Click folder icon, navigate to `res://src/World/ResourceLocatorUI.cs`
4. Double-click to attach
5. **Verify:** Script name appears in Inspector with [Export] properties

**Expected in Inspector:**
```
Script: res://src/World/ResourceLocatorUI.cs ✓
ActivationRangeMeters: 15.0
DeactivationRangeMeters: 20.0
UpdateIntervalSeconds: 0.5
PlayerPath: (empty, will configure)
IndicatorLabelPath: (empty, will configure)
```

#### Step 2.3: Create IndicatorLabel Child

1. Select **ResourceLocatorUI** node
2. Right-click → **Add Child Node**
3. Search for **Label**
4. Click to create
5. **Rename** to **`IndicatorLabel`**

**Expected Scene Tree After:**
```
ResourceLocatorUI (Control)
└── IndicatorLabel (Label) ← NEW
```

#### Step 2.4: Configure IndicatorLabel (Appearance)

1. Select **IndicatorLabel** node
2. In Inspector, configure:

**Layout:**
- Layout → Anchor Left: 0.5 (center horizontally)
- Layout → Anchor Top: 0.0 (top of screen)
- Layout → Anchor Right: 0.5
- Layout → Anchor Bottom: 0.05
- Layout → Offset Left: -100
- Layout → Offset Top: 10
- Layout → Offset Right: 100
- Layout → Offset Bottom: 40

**Text:**
- Text → Alignment: Center (both)
- Text → Text: (leave empty - script sets this)

**Font:**
- Label → Theme → Default Font Size: 18
- Label → Theme → Default Font: (use monospace if available for emoji alignment)

**Color:**
- Label → Theme Override Colors → Font Color: White (255, 255, 255)

**Expected Visual:**
- Shows indicator like "🟠 METAL NEARBY" at top-center of screen
- Only visible when resources within 15m

#### Step 2.5: Configure ResourceLocatorUI Script Properties

1. Select **ResourceLocatorUI** node
2. In Inspector, find the Script section with [Export] properties
3. Configure:

**PlayerPath:**
- Click the path selector icon (or type directly)
- Enter: `../FirstPersonController`
- This tells the script where the player is located (for distance calculations)
- **Note:** Adjust path if your FirstPersonController has a different name/location

**IndicatorLabelPath:**
- Click the path selector icon
- Enter: `IndicatorLabel`
- This points to the label we just created

**Result:**
```
PlayerPath: ../FirstPersonController ✓
IndicatorLabelPath: IndicatorLabel ✓
```

#### Step 2.6: Tag Resources with "pickable_resource" Group

This is crucial - the script looks for nodes in the "pickable_resource" group.

1. In Scene tree, find your **metal resource nodes** (e.g., MetalOre, MetalPickup, etc.)
2. Select the first metal resource node
3. In Inspector, go to the **Node** tab (next to History tab)
4. Find **Groups** section
5. Type `pickable_resource` in the text field and press Enter
6. **Repeat for ALL resource nodes:**
   - All metal pickups
   - All biomass pickups
   - All electronics pickups

**Expected Result:**
- Each resource node shows `pickable_resource` in its Groups list
- Script will detect these automatically

**Verification:**
```
[Example Metal Ore node]
Groups: pickable_resource ✓

[Example Biomass node]
Groups: pickable_resource ✓

[Example Electronics node]
Groups: pickable_resource ✓
```

#### Step 2.7: Save Scene

Press **Ctrl+S** to save Main.tscn

---

## System 3: Interactive Object Highlight

### Overview

**What it does:** Makes workbench and beacon glow with pulsing orange light  
**Visual:** Orange emission effect visible at 25m, pulses smoothly  
**Location:** Attached to workbench and beacon nodes  
**Complexity:** Medium (mesh reference, material modification)

### Step-by-Step Integration

#### Step 3.1: Locate Workbench Node

1. In Scene tree, find the **Workbench** node
2. **Click on it** to select it
3. Verify it has a **MeshInstance3D** child (for the 3D model)

**Expected Scene Structure:**
```
Workbench (Node3D or similar)
└── [MeshInstance3D] (the visual model)
```

#### Step 3.2: Attach InteractiveObjectHighlight to Workbench

1. Select the **Workbench** node
2. In Inspector, find **Script** field
3. Click folder icon, navigate to `res://src/World/InteractiveObjectHighlight.cs`
4. Double-click to attach script
5. **Verify:** Script appears in Inspector

**Expected in Inspector:**
```
Script: res://src/World/InteractiveObjectHighlight.cs ✓
BaseEmissionStrength: 1.5
HighlightEmissionStrength: 3.0
HighlightDistance: 25.0
PulseSpeed: 1.5
UsePulseAnimation: true
TargetMeshPath: (empty)
PlayerPath: (empty)
```

#### Step 3.3: Configure Workbench Script Properties

1. Select **Workbench** node (script already attached)
2. In Inspector, configure [Export] properties:

**TargetMeshPath:**
- Option A (easiest): Leave empty (defaults to self)
- Option B: Set to relative path to MeshInstance3D child
- For most cases: Leave empty

**PlayerPath:**
- Enter: `../FirstPersonController`
- Or adjust based on where FirstPersonController is relative to Workbench

**Result:**
```
TargetMeshPath: (empty - defaults to self)
PlayerPath: ../FirstPersonController
HighlightDistance: 25.0
UsePulseAnimation: true
```

#### Step 3.4: Verify Workbench Material

The script modifies the material's emission property. Verify the mesh has a material:

1. Select **Workbench** → **MeshInstance3D** child node
2. In Inspector, look for **Material** section
3. Verify there's a material assigned (should show StandardMaterial3D)
4. **If no material:** Create one by clicking "New StandardMaterial3D"

**Expected:**
```
Material: StandardMaterial3D ✓
```

#### Step 3.5: Locate and Configure Beacon Node

**Repeat Steps 3.1–3.4 for the Beacon:**

1. Find **Beacon** node in Scene tree
2. Select it
3. Attach **InteractiveObjectHighlight** script
4. Configure same properties as Workbench:
   - TargetMeshPath: (empty)
   - PlayerPath: `../FirstPersonController`
   - HighlightDistance: 25.0 (or adjust if beacon is far away)
   - UsePulseAnimation: true

**Expected After Beacon Config:**
```
Beacon (Node3D)
  Script: InteractiveObjectHighlight.cs ✓
  TargetMeshPath: (empty)
  PlayerPath: ../FirstPersonController
  HighlightDistance: 25.0
  UsePulseAnimation: true
```

#### Step 3.6: Save Scene

Press **Ctrl+S** to save Main.tscn

---

## System Integration Summary

### Final Scene Tree Structure

After all three systems integrated, your scene should look approximately like:

```
Main (Node3D root)
├── [Existing environment, player, etc...]
├── UICanvas (CanvasLayer)
│   ├── CombatTutorialUI (Control)
│   │   └── HintLabel (Label)
│   └── ResourceLocatorUI (Control)
│       └── IndicatorLabel (Label)
├── FirstPersonController (or player node)
├── GalaxabrainScout (with CombatTutorialPath configured)
├── Workbench (Node3D with InteractiveObjectHighlight script)
│   └── [MeshInstance3D with material]
├── Beacon (Node3D with InteractiveObjectHighlight script)
│   └── [MeshInstance3D with material]
└── [Other game nodes...]
```

### Export Property Configuration Summary

| Node | Script | Key Properties |
|---|---|---|
| **CombatTutorialUI** | CombatTutorialHint | DisplayDurationSeconds: 8.0 |
| **ResourceLocatorUI** | ResourceLocatorUI | PlayerPath: `../FirstPersonController`, IndicatorLabelPath: `IndicatorLabel` |
| **Workbench** | InteractiveObjectHighlight | PlayerPath: `../FirstPersonController`, HighlightDistance: 25.0 |
| **Beacon** | InteractiveObjectHighlight | PlayerPath: `../FirstPersonController`, HighlightDistance: 25.0 |

---

## Important Configuration Details

### Node Path Resolution

When you set `PlayerPath: ../FirstPersonController`, the script resolves this path:

```
CurrentNode (e.g., ResourceLocatorUI)
  └── .. (parent, UICanvas)
      └── .. (grandparent, Main)
          └── FirstPersonController ✓
```

**If path fails:**
- Error message in console: "Could not find node at path..."
- Solution: Verify the actual path in your scene tree
- Use relative paths (../) or full paths from root

**How to debug paths:**
1. Select the node in Scene tree
2. Look at its actual location
3. Count how many levels up you need to go
4. Use `../` for each level

**Example Scenarios:**

*If FirstPersonController is sibling of Main:*
```
FirstPersonController
  └── PlayerPath: .  (or just type "FirstPersonController")
```

*If FirstPersonController is child of Main:*
```
Main
  └── FirstPersonController
    └── PlayerPath: ../FirstPersonController
```

### Resource Grouping for ResourceLocatorUI

The script automatically detects resources in the `pickable_resource` group.

**To verify groups are set correctly:**

1. Select any resource node
2. Inspector → Node tab → Groups
3. Should show: `pickable_resource`
4. If missing: Add it using the Groups UI

**Resource naming convention (recommended):**
- Metal pickups: `MetalOre_01`, `MetalScrap_02`, etc.
- Biomass pickups: `Biomass_01`, `PlantMatter_02`, etc.
- Electronics pickups: `Electronics_01`, `CircuitBoard_02`, etc.

Script detects resource type from node name automatically.

---

## Testing Phase 7.4 Integration

### Pre-Test Checklist

Before running the game:

- [x] All three scripts attached to correct nodes
- [x] All node paths configured correctly
- [x] HintLabel created as child of CombatTutorialUI
- [x] IndicatorLabel created as child of ResourceLocatorUI
- [x] All resource nodes tagged with `pickable_resource` group
- [x] Main.tscn saved (Ctrl+S)
- [x] No error messages in Godot output console

### Test 1: Combat Tutorial (5 min)

1. Play the scene (click Play button or F5)
2. Walk toward the scout (trigger detection)
3. **Expected:** Text overlay appears on screen:
   ```
   THREAT DETECTED
   Maintain distance and attack from range
   Move: WASD | Attack: Mouse Click | Reload: R
   [Hint will auto-dismiss]
   ```
4. Text should stay for ~8 seconds, then fade out
5. **Result:** ✓ PASS if text appears and auto-dismisses

**If not working:**
- Check CombatTutorialPath on GalaxabrainScout (should point to CombatTutorialUI)
- Verify HintLabel exists as child of CombatTutorialUI
- Check script is attached and enabled

### Test 2: Resource Locator UI (5 min)

1. In Play mode, walk toward a resource pickup
2. Get within 15m of a metal, biomass, or electronics resource
3. **Expected:** Indicator appears at top of screen:
   ```
   🟠 METAL NEARBY
   ```
   (or BIOMASS/ELECTRONICS depending on resource)
4. Move away >20m
5. **Expected:** Indicator disappears
6. **Result:** ✓ PASS if indicator appears/disappears correctly

**If not working:**
- Verify PlayerPath points to FirstPersonController
- Verify IndicatorLabelPath points to IndicatorLabel
- Verify all resources have `pickable_resource` group tag
- Check console for errors (should not appear)

### Test 3: Interactive Object Highlight (5 min)

1. In Play mode, approach the workbench from far away (~30m)
2. Walk toward it
3. **Expected at 25m:** Orange glow appears on workbench, pulsing smoothly
4. Walk closer and around it
5. **Expected behavior:** Glow continues pulsing, intensity stays consistent
6. Walk >25m away
7. **Expected:** Glow fades or disappears
8. **Repeat for beacon** (same expected behavior)
9. **Result:** ✓ PASS if glow visible, smooth pulsing, correct distance behavior

**If not working:**
- Verify InteractiveObjectHighlight script attached to Workbench
- Verify PlayerPath configured correctly
- Check HighlightDistance (25.0 is default)
- Verify workbench MeshInstance3D has a material assigned
- If glow too dim: Increase BaseEmissionStrength and HighlightEmissionStrength in script properties

### Full MVP Loop Test (15 min)

**After all three systems pass individual tests, run a full game loop:**

1. Start new game
2. Collect 3–4 resources (verify Resource Locator indicator appears when nearby)
3. Craft mechanical arm
4. Trigger and fight scout (verify Combat Tutorial appears on first detection)
5. Defeat scout
6. Approach beacon to activate
7. See victory screen
8. **Result:** ✓ FULL PASS if all three systems work together without errors

---

## Troubleshooting Guide

### Issue: "Could not find node at path"

**Symptom:** Error in console when game starts

**Solution:**
1. Check the actual path in Scene tree
2. Count parent levels (use `../` for each)
3. Verify node name spelling (case-sensitive)
4. Re-enter path in Inspector

**Example fix:**
- If error says can't find `../FirstPersonController`
- Verify FirstPersonController exists at that level
- Try using full path from root: `/root/Main/FirstPersonController`

### Issue: Combat Tutorial Hint Never Appears

**Symptom:** Scout detected (alert plays) but no text overlay

**Solution:**
1. Verify CombatTutorialPath is set on GalaxabrainScout node
2. Value should point to CombatTutorialUI node
3. Verify CombatTutorialUI has CombatTutorialHint script attached
4. Verify HintLabel exists as direct child of CombatTutorialUI
5. Verify HintLabel has text color set to white/visible

**Debug:**
```gdscript
# In GalaxabrainScout.cs, line 218:
if (GetNodeOrNull<Player.CombatTutorialHint>(CombatTutorialPath) is { } tutorial)
{
    tutorial.ShowCombatTutorial();  // This line should execute
}
```

### Issue: Resource Indicator Never Appears

**Symptom:** No "METAL NEARBY" text even when standing on a resource

**Solution:**
1. Verify PlayerPath points to FirstPersonController (check path resolves)
2. Verify IndicatorLabelPath points to IndicatorLabel (check path resolves)
3. **Critical:** Verify all resource nodes have `pickable_resource` group tag
   - Select each resource
   - Inspector → Node → Groups
   - Add `pickable_resource` if missing
4. Verify resource node names contain "metal", "biomass", or "electronics" (lowercase)

**Debug:**
```csharp
// In ResourceLocatorUI.cs, line 154:
public void ShowAllResourcesDebug()
{
    if (_player == null) return;
    var allResources = _player.GetTree().GetNodesInGroup("pickable_resource");
    GD.Print($"Found {allResources.Count} resources in scene");
}
```

To use this: In Play mode, call this method and check console output.

### Issue: Workbench/Beacon Glow Not Visible

**Symptom:** No orange glow on interactive objects

**Solution:**
1. Verify InteractiveObjectHighlight script attached to Workbench/Beacon
2. Verify PlayerPath resolves correctly
3. Verify workbench MeshInstance3D has a StandardMaterial3D
4. Check emission is not disabled on material
5. Increase BaseEmissionStrength (try 2.0–5.0)
6. Check lighting: Add a light if area is too dark

**Debug:**
```csharp
// Verify material is created:
// In InteractiveObjectHighlight.cs, line 104–109:
// Should create a StandardMaterial3D with emission
```

**Material verification steps:**
1. Select Workbench → MeshInstance3D child
2. Inspector → Material section
3. Verify StandardMaterial3D is assigned
4. Check Emission color is orange (not black)
5. Check Emission Energy Multiplier is >0

### Issue: Scene Won't Save or Shows Errors

**Solution:**
1. Press Ctrl+S (or Cmd+S on Mac)
2. Check Godot output console for error messages
3. Common errors:
   - Script not found: Verify file paths are correct
   - Node reference invalid: Re-check node paths
   - Circular dependency: Unlikely for UI systems
4. If still failing: Close Godot and check Main.tscn file is readable

---

## Property Fine-Tuning

### Combat Tutorial Properties

**If hint displays too long/short:**
- `DisplayDurationSeconds`: Change from 8.0 to desired value (in seconds)
- `FadeOutDurationSeconds`: Change from 1.5 to desired value (fade-out duration)

**If text is hard to read:**
- Increase HintLabel → Theme → Font Size (try 28–32)
- Change font color to bright color (white, yellow, cyan)

### Resource Locator Properties

**If indicator updates too frequently/infrequently:**
- `UpdateIntervalSeconds`: Change from 0.5 (updates every 0.5 sec)
- Decrease for more frequent updates (0.2)
- Increase for less frequent (1.0)

**If detection range is too close/far:**
- `ActivationRangeMeters`: Change from 15.0 (in meters)
- Increase to 20+ for more forgiving distance
- Decrease to 10 for tighter detection

**If indicator position is wrong:**
- Select IndicatorLabel
- Adjust Anchor and Offset properties in Layout
- Move up: Decrease Anchor Top value
- Move down: Increase Anchor Top value
- Move left/right: Adjust Anchor Left/Right or Offset values

### Interactive Highlight Properties

**If glow is too bright/dim:**
- `BaseEmissionStrength`: Default 1.5 (minimum glow when outside range)
- `HighlightEmissionStrength`: Default 3.0 (maximum glow when pulsing)
- Increase both for brighter effect
- Decrease for subtler effect

**If pulsing is too fast/slow:**
- `PulseSpeed`: Default 1.5 (pulses per second)
- Increase to 2.0–3.0 for faster pulsing
- Decrease to 0.5–1.0 for slower pulsing

**If glow is visible too far/near:**
- `HighlightDistance`: Default 25.0 (in meters)
- Increase to 35+ to see glow from farther away
- Decrease to 15 for closer range only

**If you want static glow (no pulsing):**
- `UsePulseAnimation`: Set to `false`
- Glow will stay at HighlightEmissionStrength (no variation)

---

## Post-Integration Cleanup

### After Integration Complete

1. **Save Main.tscn** (Ctrl+S)
2. **Close and reopen scene** to verify it loads correctly
3. **Commit changes** to git:
   ```bash
   git add scenes/Main/Main.tscn
   git commit -m "feat: Phase 7.4 scene integration - combat tutorial, resource locator, highlight systems"
   git push
   ```
4. **Verify build** still compiles:
   ```bash
   dotnet build
   ```

### Documentation

- Update `docs/PHASE-7-4-EXECUTION.md` with actual completion time
- Mark scene integration as complete ✓
- Ready for Phase 7.4 testing

---

## Quick Reference: Node Paths

If you're uncertain about paths, use this quick reference:

```
Scene Root
├── Main (root node)
│   ├── UICanvas (CanvasLayer)
│   │   ├── CombatTutorialUI (Control)
│   │   │   └── HintLabel (Label)
│   │   └── ResourceLocatorUI (Control)
│   │       └── IndicatorLabel (Label)
│   ├── FirstPersonController
│   ├── GalaxabrainScout
│   ├── Workbench (with InteractiveObjectHighlight script)
│   │   └── [MeshInstance3D]
│   └── Beacon (with InteractiveObjectHighlight script)
│       └── [MeshInstance3D]

Path Resolution:
• From ResourceLocatorUI → FirstPersonController: ../FirstPersonController
• From CombatTutorialUI → [itself]: . (or leave empty for self-reference)
• From Workbench → FirstPersonController: ../FirstPersonController
```

---

## Timeline Estimate

| Task | Time | Cumulative |
|---|---|---|
| System 1: Combat Tutorial | 10 min | 10 min |
| System 2: Resource Locator | 10 min | 20 min |
| System 3: Interactive Highlight | 10 min | 30 min |
| Testing all systems | 15 min | 45 min |
| **Total** | **45 min** | |

**If you encounter issues:** Add 15–20 min for troubleshooting.

---

## Success Criteria

Phase 7.4 scene integration is complete when:

- [x] All three scripts attached to correct nodes
- [x] All node paths configured and verified
- [x] All label nodes created and positioned
- [x] All resource nodes tagged with `pickable_resource`
- [x] Combat Tutorial hint appears on first scout detection
- [x] Resource Locator indicator appears within 15m of resources
- [x] Interactive Highlight glow visible on workbench and beacon
- [x] Full MVP loop completes without errors
- [x] Main.tscn saved and builds clean

---

## Next Steps After Integration

1. ✓ **Scene Integration Complete**
2. → **Phase 7.4 Gameplay Testing** (30–60 min)
   - Play through full MVP loop
   - Verify all three systems work together
   - Document any issues
3. → **Phase 7.5 Platform Testing** (4–8 hours)
   - GPU compatibility testing
   - Resolution scaling
   - Install/uninstall verification

---

**Good luck! This guide covers every step. Take your time, follow the sections in order, and you'll have all three systems integrated smoothly.** ✓

