# Phase 7.4: Scene Structure & Property Reference

**Quick reference for scene tree structure and all property configurations**

---

## Final Scene Tree Structure (Complete)

```
Main (Node3D or root node)
│
├── [Your existing environment and game nodes...]
│   ├── Terrain
│   ├── Wreckage
│   ├── Environment objects
│   └── etc...
│
├── FirstPersonController (CharacterBody3D)
│   └── [Player components...]
│
├── GalaxabrainScout (CharacterBody3D)
│   ├── [Scout components...]
│   └── [Script property: CombatTutorialPath = ../UICanvas/CombatTutorialUI] ← MODIFIED
│
├── Workbench (Node3D)
│   ├── MeshInstance3D
│   │   └── [Material with StandardMaterial3D]
│   └── [Script: InteractiveObjectHighlight.cs] ← NEW
│       ├── [Property: PlayerPath = ../FirstPersonController]
│       ├── [Property: HighlightDistance = 25.0]
│       └── [Property: UsePulseAnimation = true]
│
├── Beacon (Node3D)
│   ├── MeshInstance3D
│   │   └── [Material with StandardMaterial3D]
│   └── [Script: InteractiveObjectHighlight.cs] ← NEW
│       ├── [Property: PlayerPath = ../FirstPersonController]
│       ├── [Property: HighlightDistance = 25.0]
│       └── [Property: UsePulseAnimation = true]
│
├── [Other game objects...]
│   ├── Resources (all tagged with "pickable_resource" group) ← TAGGED
│   ├── Spawn points
│   ├── Obstacles
│   └── etc...
│
└── UICanvas (CanvasLayer) ← NEW
    ├── CombatTutorialUI (Control) ← NEW
    │   └── [Script: CombatTutorialHint.cs]
    │       ├── [Property: DisplayDurationSeconds = 8.0]
    │       └── [Property: FadeOutDurationSeconds = 1.5]
    │   └── HintLabel (Label) ← NEW
    │       ├── [Position: Center-upper area]
    │       ├── [Font Size: 24]
    │       ├── [Color: White (255, 255, 255)]
    │       └── [Alignment: Center]
    │
    └── ResourceLocatorUI (Control) ← NEW
        ├── [Script: ResourceLocatorUI.cs]
        │   ├── [Property: PlayerPath = ../FirstPersonController]
        │   ├── [Property: IndicatorLabelPath = IndicatorLabel]
        │   ├── [Property: ActivationRangeMeters = 15.0]
        │   └── [Property: DeactivationRangeMeters = 20.0]
        │
        └── IndicatorLabel (Label) ← NEW
            ├── [Position: Top-center of screen]
            ├── [Font Size: 18]
            ├── [Color: White (255, 255, 255)]
            └── [Alignment: Center]
```

---

## Property Configuration Checklist

### GalaxabrainScout Node

**Existing node - ADD THIS PROPERTY:**

| Property | Value | Notes |
|---|---|---|
| CombatTutorialPath | `../UICanvas/CombatTutorialUI` | Points to Combat Tutorial UI node |

**Exact Steps:**
1. Select GalaxabrainScout in scene tree
2. Inspector → CombatTutorialPath
3. Enter: `../UICanvas/CombatTutorialUI`
4. Press Enter to confirm

---

### UICanvas Node (NEW - CanvasLayer)

| Property | Value | Notes |
|---|---|---|
| Name | `UICanvas` | Rename after creation |
| Type | `CanvasLayer` | Create via Add Child Node |
| Position | (0, 0) | Leave default |

**Exact Steps:**
1. Select Main node (root)
2. Right-click → Add Child Node
3. Search: `CanvasLayer`
4. Create it
5. Rename to `UICanvas`

---

### CombatTutorialUI Node (NEW - Control)

| Property | Value | Notes |
|---|---|---|
| Name | `CombatTutorialUI` | Rename after creation |
| Type | `Control` | Create under UICanvas |
| Script | `res://src/Player/CombatTutorialHint.cs` | Attach via Inspector |
| DisplayDurationSeconds | `8.0` | How long hint stays (in seconds) |
| FadeOutDurationSeconds | `1.5` | Fade-out duration (in seconds) |
| Anchor Layout | All 0.0 | Let parent handle positioning |
| Size | Any (parent covers) | Parent handles layout |

**Exact Steps:**
1. Select UICanvas
2. Right-click → Add Child Node
3. Search: `Control`
4. Create it
5. Rename to `CombatTutorialUI`
6. Attach script: `res://src/Player/CombatTutorialHint.cs`
7. In Inspector, configure DisplayDurationSeconds and FadeOutDurationSeconds

**Script Properties:**
```
Script: res://src/Player/CombatTutorialHint.cs
├── DisplayDurationSeconds: 8.0
└── FadeOutDurationSeconds: 1.5
```

---

### HintLabel Node (NEW - Label, child of CombatTutorialUI)

| Property | Value | Notes |
|---|---|---|
| Name | `HintLabel` | EXACT name - script looks for this |
| Type | `Label` | Create under CombatTutorialUI |
| **Layout** | | |
| Anchor Left | 0.5 | Center horizontally |
| Anchor Top | 0.25 | Upper quarter of screen |
| Anchor Right | 0.5 | Center horizontally |
| Anchor Bottom | 0.35 | Slight height below top anchor |
| Offset Left | -200 | Extend 200px left from center |
| Offset Top | 0 | At top anchor position |
| Offset Right | 200 | Extend 200px right from center |
| Offset Bottom | 50 | Height of 50px |
| **Text Properties** | | |
| Text | (empty) | Script sets this automatically |
| Alignment | Center | Both horizontal and vertical |
| **Font Properties** | | |
| Font Size | 24 | Large, readable text |
| Font | Default | Built-in Godot font OK |
| **Color** | | |
| Font Color | (255, 255, 255, 255) | White, fully opaque |

**Exact Steps:**
1. Select CombatTutorialUI
2. Right-click → Add Child Node
3. Search: `Label`
4. Create it
5. Rename to `HintLabel` (case-sensitive, script looks for this)
6. In Inspector, set all properties as table above

**Color Configuration:**
```
Inspector → Label → Theme → Theme Overrides → Colors → Font Color
Set to: Color(255, 255, 255, 255) [White]
Or: Select white color from color picker
```

---

### ResourceLocatorUI Node (NEW - Control)

| Property | Value | Notes |
|---|---|---|
| Name | `ResourceLocatorUI` | Rename after creation |
| Type | `Control` | Create under UICanvas |
| Script | `res://src/World/ResourceLocatorUI.cs` | Attach via Inspector |
| ActivationRangeMeters | `15.0` | Detection range (in meters) |
| DeactivationRangeMeters | `20.0` | Drop-off range (in meters) |
| UpdateIntervalSeconds | `0.5` | Update frequency (in seconds) |
| PlayerPath | `../FirstPersonController` | Path to player node |
| IndicatorLabelPath | `IndicatorLabel` | Path to indicator label |

**Exact Steps:**
1. Select UICanvas
2. Right-click → Add Child Node
3. Search: `Control`
4. Create it
5. Rename to `ResourceLocatorUI`
6. Attach script: `res://src/World/ResourceLocatorUI.cs`
7. In Inspector, configure all Script properties (see below)

**Script Properties:**
```
Script: res://src/World/ResourceLocatorUI.cs
├── ActivationRangeMeters: 15.0
├── DeactivationRangeMeters: 20.0
├── UpdateIntervalSeconds: 0.5
├── PlayerPath: ../FirstPersonController
└── IndicatorLabelPath: IndicatorLabel
```

---

### IndicatorLabel Node (NEW - Label, child of ResourceLocatorUI)

| Property | Value | Notes |
|---|---|---|
| Name | `IndicatorLabel` | Rename after creation |
| Type | `Label` | Create under ResourceLocatorUI |
| **Layout** | | |
| Anchor Left | 0.5 | Center horizontally |
| Anchor Top | 0.0 | Top of screen |
| Anchor Right | 0.5 | Center horizontally |
| Anchor Bottom | 0.05 | Small height at top |
| Offset Left | -100 | Extend 100px left |
| Offset Top | 10 | 10px from top |
| Offset Right | 100 | Extend 100px right |
| Offset Bottom | 40 | 40px height |
| **Text Properties** | | |
| Text | (empty) | Script sets this automatically |
| Alignment | Center | Both horizontal and vertical |
| **Font Properties** | | |
| Font Size | 18 | Medium-sized text |
| Font | Default (monospace preferred) | Built-in Godot font OK |
| **Color** | | |
| Font Color | (255, 255, 255, 255) | White, fully opaque |

**Exact Steps:**
1. Select ResourceLocatorUI
2. Right-click → Add Child Node
3. Search: `Label`
4. Create it
5. Rename to `IndicatorLabel`
6. In Inspector, set all properties as table above

---

### Workbench Node (EXISTING - ADD SCRIPT)

| Property | Value | Notes |
|---|---|---|
| Name | `Workbench` | Existing node, don't rename |
| Script | `res://src/World/InteractiveObjectHighlight.cs` | Attach via Inspector |
| BaseEmissionStrength | `1.5` | Minimum glow when outside range |
| HighlightEmissionStrength | `3.0` | Maximum glow when in range |
| HighlightDistance | `25.0` | Distance at which glow is visible (meters) |
| PulseSpeed | `1.5` | Pulse frequency (pulses per second) |
| UsePulseAnimation | `true` | Enable smooth pulsing effect |
| TargetMeshPath | (empty) | Defaults to self (MeshInstance3D) |
| PlayerPath | `../FirstPersonController` | Path to player node |

**Exact Steps:**
1. Select Workbench in scene tree
2. Inspector → Script field
3. Click folder icon (or type directly)
4. Navigate to `res://src/World/InteractiveObjectHighlight.cs`
5. Double-click to attach
6. In Inspector, configure Script properties:
   - BaseEmissionStrength: 1.5
   - HighlightEmissionStrength: 3.0
   - HighlightDistance: 25.0
   - PulseSpeed: 1.5
   - UsePulseAnimation: true
   - TargetMeshPath: (leave empty)
   - PlayerPath: `../FirstPersonController`

**Script Properties:**
```
Script: res://src/World/InteractiveObjectHighlight.cs
├── BaseEmissionStrength: 1.5
├── HighlightEmissionStrength: 3.0
├── HighlightDistance: 25.0
├── PulseSpeed: 1.5
├── UsePulseAnimation: true
├── TargetMeshPath: (empty)
└── PlayerPath: ../FirstPersonController
```

**Material Verification:**
1. Select Workbench → MeshInstance3D (child)
2. Inspector → Material section
3. Verify StandardMaterial3D is assigned
4. If not, create one: Click "New StandardMaterial3D"

---

### Beacon Node (EXISTING - ADD SCRIPT)

**Identical to Workbench:**

| Property | Value | Notes |
|---|---|---|
| Name | `Beacon` | Existing node, don't rename |
| Script | `res://src/World/InteractiveObjectHighlight.cs` | Same script as Workbench |
| BaseEmissionStrength | `1.5` | Same as Workbench |
| HighlightEmissionStrength | `3.0` | Same as Workbench |
| HighlightDistance | `25.0` | Same as Workbench |
| PulseSpeed | `1.5` | Same as Workbench |
| UsePulseAnimation | `true` | Same as Workbench |
| TargetMeshPath | (empty) | Same as Workbench |
| PlayerPath | `../FirstPersonController` | Same as Workbench |

**Exact Steps:**
1. Select Beacon in scene tree
2. Attach script and configure properties (identical to Workbench steps above)

---

### All Resource Nodes (EXISTING - ADD TO GROUP)

**Action: Tag each resource pickup with group**

| Node Type | Group Tag | How to Add |
|---|---|---|
| Metal Pickups | `pickable_resource` | Inspector → Node → Groups → Add |
| Biomass Pickups | `pickable_resource` | Inspector → Node → Groups → Add |
| Electronics Pickups | `pickable_resource` | Inspector → Node → Groups → Add |

**Exact Steps (repeat for EACH resource node):**
1. Select a metal/biomass/electronics pickup node
2. Inspector → Switch to **Node** tab (next to History)
3. Find **Groups** section
4. Type `pickable_resource` in text field
5. Press Enter
6. Verify it appears in the Groups list
7. **REPEAT FOR ALL RESOURCE NODES**

**Verification:**
```
[Any resource node]
Node Tab → Groups:
  └── pickable_resource ✓
```

---

## Property Quick Reference by System

### System 1: Combat Tutorial

```yaml
UICanvas:
  Type: CanvasLayer
  Position: (0, 0)

CombatTutorialUI:
  Type: Control
  Parent: UICanvas
  Script: res://src/Player/CombatTutorialHint.cs
  Properties:
    DisplayDurationSeconds: 8.0
    FadeOutDurationSeconds: 1.5

HintLabel:
  Type: Label
  Parent: CombatTutorialUI
  Name: HintLabel (EXACT - case-sensitive)
  Layout:
    Anchors: (0.5, 0.25, 0.5, 0.35)
    Offsets: (-200, 0, 200, 50)
  Font:
    Size: 24
    Color: White (255, 255, 255)
  Alignment: Center

GalaxabrainScout (existing):
  CombatTutorialPath: ../UICanvas/CombatTutorialUI
```

### System 2: Resource Locator

```yaml
ResourceLocatorUI:
  Type: Control
  Parent: UICanvas
  Script: res://src/World/ResourceLocatorUI.cs
  Properties:
    ActivationRangeMeters: 15.0
    DeactivationRangeMeters: 20.0
    UpdateIntervalSeconds: 0.5
    PlayerPath: ../FirstPersonController
    IndicatorLabelPath: IndicatorLabel

IndicatorLabel:
  Type: Label
  Parent: ResourceLocatorUI
  Name: IndicatorLabel
  Layout:
    Anchors: (0.5, 0.0, 0.5, 0.05)
    Offsets: (-100, 10, 100, 40)
  Font:
    Size: 18
    Color: White (255, 255, 255)
  Alignment: Center

All Resource Nodes (existing):
  Group: pickable_resource (add to each)
```

### System 3: Interactive Highlight

```yaml
Workbench (existing):
  Script: res://src/World/InteractiveObjectHighlight.cs
  Properties:
    BaseEmissionStrength: 1.5
    HighlightEmissionStrength: 3.0
    HighlightDistance: 25.0
    PulseSpeed: 1.5
    UsePulseAnimation: true
    TargetMeshPath: (empty)
    PlayerPath: ../FirstPersonController

Beacon (existing):
  Script: res://src/World/InteractiveObjectHighlight.cs
  Properties:
    BaseEmissionStrength: 1.5
    HighlightEmissionStrength: 3.0
    HighlightDistance: 25.0
    PulseSpeed: 1.5
    UsePulseAnimation: true
    TargetMeshPath: (empty)
    PlayerPath: ../FirstPersonController
```

---

## Node Path Quick Reference

When setting `PlayerPath` or other path properties, use relative paths:

| From | To | Path |
|---|---|---|
| CombatTutorialUI | FirstPersonController | `../FirstPersonController` |
| ResourceLocatorUI | FirstPersonController | `../FirstPersonController` |
| Workbench | FirstPersonController | `../FirstPersonController` |
| Beacon | FirstPersonController | `../FirstPersonController` |
| ResourceLocatorUI | IndicatorLabel (its own child) | `IndicatorLabel` |
| CombatTutorialUI | HintLabel (its own child) | `HintLabel` |

**Path Resolution Logic:**
```
UICanvas/CombatTutorialUI needs to find ../FirstPersonController
  UICanvas/CombatTutorialUI
  ├── .. = UICanvas's parent = Main
  └── Main/FirstPersonController ✓ Found!

Workbench needs to find ../FirstPersonController
  Workbench (at Main level)
  ├── .. = Main
  └── Main/FirstPersonController ✓ Found!
```

---

## Verification Checklist

After completing integration, verify:

**Scene Tree Structure:**
- [x] UICanvas exists under Main (CanvasLayer)
- [x] CombatTutorialUI exists under UICanvas (Control)
- [x] HintLabel exists under CombatTutorialUI (Label)
- [x] ResourceLocatorUI exists under UICanvas (Control)
- [x] IndicatorLabel exists under ResourceLocatorUI (Label)
- [x] Workbench has InteractiveObjectHighlight script attached
- [x] Beacon has InteractiveObjectHighlight script attached

**Scripts Attached:**
- [x] CombatTutorialUI → `CombatTutorialHint.cs` ✓
- [x] ResourceLocatorUI → `ResourceLocatorUI.cs` ✓
- [x] Workbench → `InteractiveObjectHighlight.cs` ✓
- [x] Beacon → `InteractiveObjectHighlight.cs` ✓

**Properties Configured:**
- [x] CombatTutorialUI DisplayDurationSeconds: 8.0
- [x] ResourceLocatorUI PlayerPath: `../FirstPersonController`
- [x] ResourceLocatorUI IndicatorLabelPath: `IndicatorLabel`
- [x] Workbench PlayerPath: `../FirstPersonController`
- [x] Beacon PlayerPath: `../FirstPersonController`
- [x] GalaxabrainScout CombatTutorialPath: `../UICanvas/CombatTutorialUI`

**Resource Tags:**
- [x] All resource nodes have `pickable_resource` group tag

**Save & Build:**
- [x] Main.tscn saved (Ctrl+S)
- [x] `dotnet build` completes with 0 errors

---

## Common Configuration Mistakes to Avoid

| Mistake | Problem | Fix |
|---|---|---|
| HintLabel named "Label" instead of "HintLabel" | Script can't find the label | Rename to exact name: `HintLabel` |
| PlayerPath: `FirstPersonController` (missing `../`) | Path doesn't resolve | Use relative path: `../FirstPersonController` |
| Forgot to tag resources with group | Indicator never appears | Add `pickable_resource` group to each resource |
| Beacon script attached but PlayerPath empty | Glow won't work | Set PlayerPath: `../FirstPersonController` |
| CombatTutorialPath wrong on scout | Hint never shows | Set to: `../UICanvas/CombatTutorialUI` |
| HintLabel has Font Size 8 | Text too small to read | Change to 24pt |
| IndicatorLabel positioned at bottom | Shows in wrong place | Set Anchor Top: 0.0, Anchor Bottom: 0.05 |

---

## Quick Copy-Paste Property Values

Use these exact values if you want to copy-paste:

**CombatTutorialUI Properties:**
```
DisplayDurationSeconds: 8.0
FadeOutDurationSeconds: 1.5
```

**ResourceLocatorUI Properties:**
```
ActivationRangeMeters: 15.0
DeactivationRangeMeters: 20.0
UpdateIntervalSeconds: 0.5
PlayerPath: ../FirstPersonController
IndicatorLabelPath: IndicatorLabel
```

**HintLabel Layout Properties:**
```
Anchor Left: 0.5
Anchor Top: 0.25
Anchor Right: 0.5
Anchor Bottom: 0.35
Offset Left: -200
Offset Top: 0
Offset Right: 200
Offset Bottom: 50
```

**IndicatorLabel Layout Properties:**
```
Anchor Left: 0.5
Anchor Top: 0.0
Anchor Right: 0.5
Anchor Bottom: 0.05
Offset Left: -100
Offset Top: 10
Offset Right: 100
Offset Bottom: 40
```

**Workbench/Beacon InteractiveObjectHighlight Properties:**
```
BaseEmissionStrength: 1.5
HighlightEmissionStrength: 3.0
HighlightDistance: 25.0
PulseSpeed: 1.5
UsePulseAnimation: true
TargetMeshPath: (empty)
PlayerPath: ../FirstPersonController
```

---

This reference should be everything you need to configure Phase 7.4 systems. Refer back to it while working in the Godot editor.

