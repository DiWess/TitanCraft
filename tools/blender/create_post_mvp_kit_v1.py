#!/usr/bin/env python3
"""Create the TitanCraft post-MVP kit V1 Blender sources (workstreams B and C).

Everything the arm's new modules, the two new Galaxabrain variants and the two
gates need, in the visual language the MVP Asset Pack V1 set (same palette,
same bevelled hard-surface primitives, purple Galaxabrain emissives, orange
human-tech accents) so the new pieces read as the same world:

  1. TC_CHAR_GalaxabrainWarden_V1          heavy six-legged Warden body, red-violet core
  2. TC_CHAR_GalaxabrainWarden_Guard_V1    its frontal carapace guard (animated separately)
  3. TC_CHAR_GalaxabrainWarden_Disabled_V1 collapsed Warden, dimmed core
  4. TC_CHAR_GalaxabrainDrone_V1           small flying swarm drone, braincase core and vanes
  5. TC_PROP_WreckBarricade_V1             hull-plate wreck jammed across a gate
  6. TC_ENV_CrackedGate_V1                 coral-stone gate bricked up, glowing cracks
  7. TC_ENV_CrackedGate_Broken_V1          the same gate after a charged strike: rubble
  8. TC_PICKUP_PowerCore_V1                the beacon's power cell
  9. TC_PLAYER_ArmClamp_V1                 Mk II magnet clamp, fitted around the fist
 10. TC_PLAYER_ArmCoil_V1                  Mk III breaker coil, fitted along the forearm
 11-16. TC_PROP_NoticeBoard / Slate / ToyBoat / FlightRecorder / StrippedMachine / Plaque
        the quarter's six discoveries (workstream D)
 17. TC_ENV_CourtPaving_V1                 4 m tile of worn paving for the two courts

Forward is Blender +Y (glTF/Godot -Z) for the Warden and the drone, so the
gameplay code's look-at convention needs no per-model rotation. The arm
modules share TC_PLAYER_MechanicalArm_V1's local space (forearm along +Y from
the elbow) so they sit on the arm with the arm model's own viewmodel transform.

Run with Blender:
  blender --background --python tools/blender/create_post_mvp_kit_v1.py
  blender --background --python tools/blender/create_post_mvp_kit_v1.py -- --asset TC_PROP_WreckBarricade_V1
"""
from __future__ import annotations

import math
import random
import sys
from pathlib import Path

import bpy

sys.path.insert(0, str(Path(__file__).resolve().parent))
import create_mvp_asset_pack_v1 as mvp  # noqa: E402  (same helpers and palette as the MVP pack)

ROOT = Path(__file__).resolve().parents[2]
SOURCE_DIR = ROOT / "assets/Source/Blender/Production/PostMvp_Kit_V1"

LIME_WASH = (0.858824, 0.831373, 0.760784)
CORAL_STONE = (0.560784, 0.509804, 0.423529)
CORAL_SHADOW = (0.352941, 0.317647, 0.262745)
RUBBLE_CORE = (0.090196, 0.086275, 0.082353)
WARDEN_RED = (1.000, 0.120, 0.300)


def _alien_materials(dim: bool = False):
    carbon = mvp.pbr("TC_MVP_AlienCarbon", (0.030, 0.032, 0.040), rough=0.38, metal=0.55)
    plate = mvp.pbr("TC_MVP_AlienPlate", (0.085, 0.090, 0.105), rough=0.45, metal=0.65)
    purple = mvp.pbr("TC_MVP_PurpleGlow" + ("Dim" if dim else ""), (0.01, 0.0, 0.03), rough=0.3,
                     emission=mvp.PURPLE_GLOW, strength=1.0 if dim else 6.0)
    crystal = mvp.pbr("TC_MVP_PurpleCrystal" + ("Dim" if dim else ""), mvp.PURPLE_CRYSTAL, rough=0.22,
                      emission=mvp.PURPLE_GLOW, strength=0.15 if dim else 1.4)
    return carbon, plate, purple, crystal


def _build_warden(disabled: bool) -> None:
    """Six-legged heavy biomech: low, wide, armoured; the Scout's big sibling.

    The first pass stood the body on long thin legs and the review render read
    as a table, not a heavy: the legs are now short, thick and planted wide,
    and the armour sits low over them.
    """
    carbon, plate, purple, crystal = _alien_materials(dim=disabled)
    core = mvp.pbr("TC_POST_WardenCore" + ("Dim" if disabled else ""), (0.03, 0.0, 0.01), rough=0.3,
                   emission=WARDEN_RED, strength=1.2 if disabled else 7.5)
    rng = random.Random(61)
    z = (lambda h: 0.55 * h) if disabled else (lambda h: h)
    tilt = 14 if disabled else 0
    prefix = "TC_WardenDisabled" if disabled else "TC_Warden"
    # Massive armoured thorax, overlapping carapace shells and a spine ridge.
    mvp.gem(f"{prefix}_Thorax", carbon, (0, 0, z(1.05)), 0.80, (1.10, 1.30, 0.85), rot=(tilt, 0, 0), jitter=0.03, rng=rng)
    mvp.gem(f"{prefix}_Belly", plate, (0, 0.1, z(0.72)), 0.62, (1.0, 1.15, 0.45), jitter=0.02, rng=rng)
    mvp.box(f"{prefix}_ShellFront", plate, (0, 0.38, z(1.58)), (1.45, 0.95, 0.30), rot=(-16 + tilt, 0, 0), bevel=0.05)
    mvp.box(f"{prefix}_ShellMid", plate, (0, -0.15, z(1.68)), (1.35, 0.70, 0.28), rot=(4 + tilt, 0, 0), bevel=0.05)
    mvp.box(f"{prefix}_ShellRear", plate, (0, -0.62, z(1.48)), (1.20, 0.75, 0.30), rot=(18 + tilt, 0, 0), bevel=0.05)
    for side in (-1, 1):
        mvp.box(f"{prefix}_Flank_{side}", plate, (0.78 * side, 0, z(1.15)), (0.22, 1.5, 0.65),
                rot=(0, 18 * side, 0), bevel=0.04)
    for i, y in enumerate((-0.6, -0.25, 0.1, 0.45)):
        mvp.taper(f"{prefix}_Spine_{i}", plate, (0, y, z(1.92 - abs(y) * 0.18)), 0.12, 0.02, 0.38,
                  rot=(-22 + i * 12, 0, 0), vertices=4, bevel=0.01)
    # Head: low, shovel-shaped, braincase glowing through the front.
    mvp.gem(f"{prefix}_Head", carbon, (0, 1.05, z(0.95)), 0.36, (1.3, 0.95, 0.75), jitter=0.015, rng=rng)
    mvp.gem(f"{prefix}_Braincase", crystal, (0, 1.12, z(1.14)), 0.20, (1.1, 0.8, 0.8), jitter=0.01, rng=rng)
    for side in (-1, 1):
        mvp.box(f"{prefix}_Eye_{side}", purple, (0.19 * side, 1.36, z(0.96)), (0.14, 0.03, 0.045),
                rot=(0, 0, 18 * side), bevel=0)
        mvp.strut(f"{prefix}_Tusk_{side}", plate, (0.25 * side, 1.22, z(0.80)), (0.36 * side, 1.62, z(0.55)), 0.08, 0.015)
    # The weak point: a red core under the front shell, flaring when staggered.
    mvp.gem(f"{prefix}_Core", core, (0, 0.72, z(0.80)), 0.22, (1.0, 0.9, 1.0))
    # Six legs: short, thick, armoured, planted wide.
    for i, ang in enumerate((30, 90, 150, 210, 270, 330)):
        rad = math.radians(ang)
        hip = (0.72 * math.cos(rad), 0.80 * math.sin(rad), z(0.95))
        knee = (1.28 * math.cos(rad), 1.38 * math.sin(rad), z(1.22))
        foot = (1.55 * math.cos(rad), 1.65 * math.sin(rad), 0.0)
        mvp.strut(f"{prefix}_LegUpper_{i}", plate, hip, knee, 0.20, 0.15, bevel=0.02)
        mvp.gem(f"{prefix}_Knee_{i}", carbon, knee, 0.17, (1, 1, 1))
        mvp.strut(f"{prefix}_LegLower_{i}", carbon, knee, foot, 0.15, 0.06, scale=(1, 0.75, 1), bevel=0.015)
        mvp.box(f"{prefix}_Greave_{i}", plate, ((knee[0] + foot[0]) / 2 * 1.02, (knee[1] + foot[1]) / 2 * 1.02, z(0.62)),
                (0.16, 0.16, 0.55), rot=(0, 0, ang), bevel=0.02)
        mvp.strut(f"{prefix}_LegGlow_{i}", purple,
                  ((hip[0] + knee[0]) / 2, (hip[1] + knee[1]) / 2, (hip[2] + knee[2]) / 2 + 0.12),
                  (knee[0], knee[1], knee[2] + 0.08), 0.035, 0.015, bevel=0)
    if disabled:
        # The guard torn off and lying beside the body.
        mvp.box(f"{prefix}_FallenGuard", plate, (1.5, 1.2, 0.12), (1.6, 0.18, 1.2), rot=(80, 0, 35), bevel=0.04)


def build_warden() -> None:
    _build_warden(disabled=False)


def build_warden_disabled() -> None:
    _build_warden(disabled=True)


def build_warden_guard() -> None:
    """The frontal guard: a curved carapace shield with purple seams, held before the head."""
    carbon, plate, purple, _ = _alien_materials()
    # Three overlapping curved plates, a central ridge, glowing seams between.
    for i, (x, rot_z, w) in enumerate(((-0.55, 22, 0.75), (0.0, 0.0, 0.9), (0.55, -22, 0.75))):
        mvp.box(f"TC_WardenGuard_Plate_{i}", plate, (x, 1.45, 1.25 - abs(x) * 0.15), (w, 0.16, 1.25),
                rot=(-8, 0, rot_z), bevel=0.05)
    mvp.box("TC_WardenGuard_Ridge", carbon, (0, 1.56, 1.30), (0.16, 0.14, 1.3), rot=(-8, 0, 0), bevel=0.03)
    for i, x in enumerate((-0.30, 0.30)):
        mvp.box(f"TC_WardenGuard_Seam_{i}", purple, (x, 1.53, 1.25), (0.035, 0.05, 1.05),
                rot=(-8, 0, 11 if x > 0 else -11), bevel=0)
    # Clawed edge along the top.
    for i, x in enumerate((-0.7, -0.35, 0.0, 0.35, 0.7)):
        mvp.taper(f"TC_WardenGuard_Claw_{i}", carbon, (x, 1.42, 1.95 - abs(x) * 0.18), 0.07, 0.0, 0.26,
                  rot=(-10, 0, 0), vertices=4, bevel=0.005)


def build_drone() -> None:
    """Swarm drone: a fist-sized braincase in a carapace ring, three vanes, trailing tendrils."""
    carbon, plate, purple, crystal = _alien_materials()
    rng = random.Random(83)
    mvp.gem("TC_Drone_Shell", carbon, (0, 0, 0), 0.30, (1.0, 1.15, 0.75), jitter=0.015, rng=rng)
    mvp.gem("TC_Drone_Braincase", crystal, (0, 0.10, 0.10), 0.17, (1.0, 1.0, 0.9), jitter=0.01, rng=rng)
    mvp.box("TC_Drone_EyeSlit", purple, (0, 0.33, 0.02), (0.16, 0.03, 0.035), bevel=0)
    # Carapace ring around the body.
    bpy.ops.mesh.primitive_torus_add(major_radius=0.40, minor_radius=0.055, major_segments=18, minor_segments=6,
                                     location=(0, 0, -0.02))
    ring = bpy.context.object
    ring.name = "TC_Drone_Ring"
    ring.scale = (1.0, 1.1, 0.7)
    mvp._finalize(ring, plate, bevel=0)
    # Three vanes: the silhouette that reads at distance.
    for i in range(3):
        angle = math.radians(90 + i * 120)
        cx, cy = 0.52 * math.cos(angle), 0.52 * math.sin(angle)
        mvp.box(f"TC_Drone_Vane_{i}", plate, (cx, cy, 0.04), (0.10, 0.36, 0.035),
                rot=(0, 18, math.degrees(angle) - 90), bevel=0.012)
        mvp.box(f"TC_Drone_VaneGlow_{i}", purple, (cx * 1.18, cy * 1.18, 0.05), (0.03, 0.18, 0.02),
                rot=(0, 18, math.degrees(angle) - 90), bevel=0)
    # Tendrils hanging below.
    for i, (x, y) in enumerate(((0.10, 0.06), (-0.09, 0.02), (0.0, -0.12))):
        mvp.cable(f"TC_Drone_Tendril_{i}", carbon,
                  [(x, y, -0.15), (x * 1.3, y * 1.4, -0.35), (x * 0.8, y * 1.8, -0.55), (x * 1.4, y * 2.0, -0.72)], 0.018)


def build_wreck_barricade() -> None:
    """A section of the ship's hull jammed across an alley gate: plates, ribs, scorch."""
    beige = mvp.pbr("TC_MVP_HullBeige", mvp.BEIGE_HULL, rough=0.62, metal=0.12)
    soot = mvp.pbr("TC_MVP_SootMetal", (0.055, 0.050, 0.048), rough=0.75, metal=0.35)
    mid = mvp.pbr("TC_MVP_MidMetal", mvp.MID_METAL, rough=0.45, metal=0.75)
    orange = mvp.pbr("TC_MVP_OrangePaint", mvp.ORANGE_PAINT, rough=0.42)
    cyan = mvp.pbr("TC_MVP_CyanLED", (0.0, 0.02, 0.02), rough=0.3, emission=mvp.CYAN_GLOW, strength=2.5)
    # Main plate leaning across the 2.6 m opening, 2.4 m tall.
    mvp.box("TC_Wreck_PlateMain", beige, (0, 0, 1.15), (2.7, 0.16, 2.2), rot=(6, -6, 4), bevel=0.04)
    mvp.box("TC_Wreck_PlateBent", beige, (0.5, -0.32, 0.55), (1.6, 0.14, 1.1), rot=(-24, 12, -10), bevel=0.04)
    mvp.box("TC_Wreck_Scorch", soot, (-0.8, 0.02, 1.85), (0.9, 0.18, 0.75), rot=(6, -6, 4), bevel=0.02)
    for i, x in enumerate((-0.95, -0.3, 0.35, 1.0)):
        mvp.box(f"TC_Wreck_Rib_{i}", mid, (x, 0.16, 1.1), (0.12, 0.14, 2.3), rot=(6, -6, 4 + i * 2), bevel=0.015)
    mvp.box("TC_Wreck_Marking", orange, (0.35, -0.09, 1.55), (0.9, 0.02, 0.22), rot=(6, -6, 4), bevel=0)
    # A stencilled hull number and a still-blinking panel light, so it reads as
    # the player's ship, torn off in the crash.
    mvp.text_marking("TC_Wreck_Stencil", "TC-07", orange, (-0.85, -0.10, 0.70), 0.22, (96, -6, 4))
    mvp.box("TC_Wreck_PanelLight", cyan, (0.95, -0.10, 0.35), (0.12, 0.02, 0.06), rot=(6, -6, 4), bevel=0)
    # Magnet anchor plate: the obvious place the clamp grabs.
    mvp.cyl("TC_Wreck_Anchor", mid, (0, -0.12, 1.15), 0.22, 0.06, rot=(90, 0, 0), vertices=10, bevel=0.01)
    mvp.cyl("TC_Wreck_AnchorRing", cyan, (0, -0.16, 1.15), 0.15, 0.02, rot=(90, 0, 0), vertices=10, bevel=0)


def _coral_gate(broken: bool) -> None:
    lime = mvp.pbr("TC_MAT_LimeRender", LIME_WASH, rough=0.92)
    stone = mvp.pbr("TC_MAT_CoralRag", CORAL_STONE, rough=0.95)
    shadow = mvp.pbr("TC_MAT_CoralWeathered", CORAL_SHADOW, rough=0.96)
    core = mvp.pbr("TC_MAT_PlasterLossCore", RUBBLE_CORE, rough=0.97)
    purple = mvp.pbr("TC_MVP_PurpleGlow", (0.01, 0.0, 0.03), rough=0.3, emission=mvp.PURPLE_GLOW, strength=4.0)
    crystal = mvp.pbr("TC_MVP_PurpleCrystal", mvp.PURPLE_CRYSTAL, rough=0.22, emission=mvp.PURPLE_GLOW, strength=1.2)
    rng = random.Random(97 if broken else 89)
    # Jambs and a lintel: the doorway itself, which survives the strike.
    for side in (-1, 1):
        mvp.box(f"TC_Gate_Jamb_{side}", stone, (1.55 * side, 0, 1.5), (0.45, 0.6, 3.0), bevel=0.03)
    mvp.box("TC_Gate_Lintel", stone, (0, 0, 3.15), (3.6, 0.65, 0.35), bevel=0.03)
    if not broken:
        # The Galaxabrains' infill: rubble and resin packed into the doorway,
        # cracked through with violet light.
        mvp.box("TC_Gate_Infill", shadow, (0, 0, 1.47), (2.65, 0.5, 2.94), bevel=0.02)
        # Patches of the old render left on the infill, broken up so they read
        # as plaster on rubble rather than a window pane.
        for i, (x, zc, w, h) in enumerate(((-0.55, 2.35, 0.9, 0.8), (0.65, 1.15, 0.8, 1.0), (-0.75, 0.55, 0.6, 0.5))):
            mvp.box(f"TC_Gate_InfillRender_{i}", lime, (x, -0.27, zc), (w, 0.04, h), rot=(0, 4 - i * 5, 0), bevel=0.01)
        cracks = [((-0.6, 0.6), (0.1, 1.4)), ((0.1, 1.4), (-0.2, 2.3)), ((0.1, 1.4), (0.8, 2.0)),
                  ((0.8, 2.0), (1.1, 2.7)), ((-0.2, 2.3), (-0.9, 2.8)), ((-0.6, 0.6), (-1.1, 0.2))]
        for i, ((x1, z1), (x2, z2)) in enumerate(cracks):
            mvp.strut(f"TC_Gate_Crack_{i}", purple, (x1, -0.30, z1), (x2, -0.30, z2), 0.035, 0.02, bevel=0)
        for i in range(5):
            mvp.gem(f"TC_Gate_Resin_{i}", crystal, (rng.uniform(-1.1, 1.1), -0.26, rng.uniform(0.2, 2.8)),
                    rng.uniform(0.07, 0.13), (1, 0.5, 1.4), rot=(0, rng.uniform(-30, 30), 0))
    else:
        # Rubble spilled out of the doorway, the opening clear to walk through.
        for i in range(14):
            x = rng.uniform(-1.6, 1.6)
            y = rng.uniform(-1.8, 1.4)
            size = rng.uniform(0.18, 0.45)
            mvp.gem(f"TC_GateRubble_{i}", rng.choice([shadow, core, stone, lime]), (x, y, size * 0.35), size,
                    (1.2, 1.0, 0.6), rot=(rng.uniform(0, 40), rng.uniform(0, 40), rng.uniform(0, 180)),
                    jitter=0.04, rng=rng)
        for i in range(3):
            mvp.gem(f"TC_GateResinShard_{i}", crystal, (rng.uniform(-1.2, 1.2), rng.uniform(-1.4, 1.0), 0.08),
                    0.09, (1, 0.6, 1.5), rot=(80, 0, rng.uniform(0, 180)))


def build_cracked_gate() -> None:
    _coral_gate(broken=False)


def build_cracked_gate_broken() -> None:
    _coral_gate(broken=True)


def build_power_core() -> None:
    """The beacon's power cell: a human-tech capsule, orange caps, cyan charge window."""
    beige = mvp.pbr("TC_MVP_HullBeige", mvp.BEIGE_HULL, rough=0.62, metal=0.12)
    dark = mvp.pbr("TC_MVP_DarkMetal", mvp.DARK_METAL, rough=0.48, metal=0.65)
    orange = mvp.pbr("TC_MVP_OrangePaint", mvp.ORANGE_PAINT, rough=0.42)
    cyan = mvp.pbr("TC_POST_CoreCyan", (0.0, 0.03, 0.04), rough=0.2, emission=mvp.CYAN_GLOW, strength=6.0)
    mvp.cyl("TC_Core_Body", beige, (0, 0, 0.45), 0.17, 0.62, vertices=12, bevel=0.015)
    mvp.cyl("TC_Core_Window", cyan, (0, 0, 0.45), 0.175, 0.30, vertices=12, bevel=0)
    for i, z in enumerate((0.12, 0.78)):
        mvp.cyl(f"TC_Core_Cap_{i}", orange, (0, 0, z), 0.20, 0.08, vertices=12, bevel=0.01)
    mvp.cyl("TC_Core_Terminal", dark, (0, 0, 0.87), 0.07, 0.10, vertices=8, bevel=0.005)
    for i in range(4):
        angle = math.radians(45 + i * 90)
        mvp.box(f"TC_Core_Rib_{i}", dark, (0.19 * math.cos(angle), 0.19 * math.sin(angle), 0.45), (0.04, 0.04, 0.58),
                rot=(0, 0, math.degrees(angle)), bevel=0.005)
    # Galaxabrain growth clinging to it: it was in their nest.
    carbon, _, _, crystal = _alien_materials()
    mvp.gem("TC_Core_Growth", crystal, (0.16, -0.08, 0.2), 0.08, (1, 0.7, 1.6), rot=(20, -30, 0))
    mvp.gem("TC_Core_GrowthBase", carbon, (0.14, -0.06, 0.08), 0.1, (1, 0.8, 0.5))


def build_arm_clamp() -> None:
    """Mk II magnet clamp: a collar at the wrist and three magnet prongs around the fist."""
    dark = mvp.pbr("TC_MVP_DarkMetal", mvp.DARK_METAL, rough=0.48, metal=0.65)
    mid = mvp.pbr("TC_MVP_MidMetal", mvp.MID_METAL, rough=0.45, metal=0.75)
    cyan = mvp.pbr("TC_MVP_CyanLED", (0.0, 0.02, 0.02), rough=0.3, emission=mvp.CYAN_GLOW, strength=5.0)
    purple = mvp.pbr("TC_MVP_PurpleGlow", (0.01, 0.0, 0.03), rough=0.3, emission=mvp.PURPLE_GLOW, strength=5.0)
    mvp.cyl("TC_Clamp_Collar", dark, (0, 0.62, 0.17), 0.135, 0.07, rot=(90, 0, 0), vertices=12, bevel=0.01)
    mvp.cyl("TC_Clamp_CollarGlow", cyan, (0, 0.585, 0.17), 0.12, 0.012, rot=(90, 0, 0), vertices=12, bevel=0)
    # The Scout's component, fitted on top of the collar: where Mk II's power comes from.
    mvp.gem("TC_Clamp_Component", purple, (0, 0.6, 0.32), 0.05, (0.7, 0.7, 1.3))
    for i in range(3):
        angle = math.radians(90 + i * 120)
        x, z = 0.15 * math.cos(angle), 0.17 + 0.15 * math.sin(angle)
        mvp.strut(f"TC_Clamp_Prong_{i}", mid, (x * 0.8, 0.66, 0.17 + (z - 0.17) * 0.8), (x * 1.15, 0.98, 0.17 + (z - 0.17) * 1.15),
                  0.03, 0.02, bevel=0.005)
        mvp.box(f"TC_Clamp_Pad_{i}", cyan, (x * 1.18, 1.0, 0.17 + (z - 0.17) * 1.18), (0.05, 0.02, 0.05), bevel=0)


def build_arm_coil() -> None:
    """Mk III breaker coil: two coil rings along the forearm and a capacitor pack on top."""
    dark = mvp.pbr("TC_MVP_DarkMetal", mvp.DARK_METAL, rough=0.48, metal=0.65)
    orange = mvp.pbr("TC_MVP_OrangePaint", mvp.ORANGE_PAINT, rough=0.42)
    glow = mvp.pbr("TC_POST_CoilGlow", (0.04, 0.01, 0.0), rough=0.3, emission=mvp.ORANGE_GLOW, strength=5.0)
    for i, y in enumerate((0.20, 0.42)):
        bpy.ops.mesh.primitive_torus_add(major_radius=0.17, minor_radius=0.025, major_segments=16, minor_segments=6,
                                         location=(0, y, 0.17), rotation=(math.radians(90), 0, 0))
        coil = bpy.context.object
        coil.name = f"TC_Coil_Ring_{i}"
        mvp._finalize(coil, glow, bevel=0)
        mvp.cyl(f"TC_Coil_Band_{i}", dark, (0, y + 0.035, 0.17), 0.165, 0.025, rot=(90, 0, 0), vertices=12, bevel=0.004)
    mvp.box("TC_Coil_Capacitor", dark, (0, 0.31, 0.385), (0.12, 0.30, 0.07), bevel=0.012)
    mvp.box("TC_Coil_CapacitorStripe", orange, (0, 0.31, 0.425), (0.10, 0.26, 0.012), bevel=0)
    for i, y in enumerate((0.22, 0.31, 0.40)):
        mvp.box(f"TC_Coil_Cell_{i}", glow, (0.065, y, 0.385), (0.012, 0.05, 0.03), bevel=0)


# --- Workstream D: the quarter's discoveries -----------------------------------
TIMBER = (0.435294, 0.400000, 0.352941)
PAPER = (0.86, 0.83, 0.74)
SLATE = (0.06, 0.065, 0.07)
CHALK = (0.88, 0.88, 0.84)


def build_notice_board() -> None:
    """Harbour notice board: timber frame, lime-washed back, pinned sheets."""
    timber = mvp.pbr("TC_POST_Timber", TIMBER, rough=0.85)
    paper = mvp.pbr("TC_POST_Paper", PAPER, rough=0.95)
    lime = mvp.pbr("TC_MAT_LimeRender", LIME_WASH, rough=0.92)
    ink = mvp.pbr("TC_POST_Ink", (0.08, 0.07, 0.07), rough=0.9)
    for side in (-1, 1):
        mvp.box(f"TC_Notice_Post_{side}", timber, (0.62 * side, 0, 0.9), (0.09, 0.09, 1.8), bevel=0.01)
    mvp.box("TC_Notice_Back", lime, (0, 0.0, 1.25), (1.2, 0.05, 0.85), bevel=0.01)
    mvp.box("TC_Notice_Cap", timber, (0, 0, 1.72), (1.4, 0.18, 0.06), bevel=0.01)
    for i, (x, z, w, h, tilt) in enumerate(((-0.3, 1.3, 0.42, 0.55, -3), (0.25, 1.38, 0.38, 0.42, 4), (0.28, 0.98, 0.34, 0.26, -6))):
        mvp.box(f"TC_Notice_Sheet_{i}", paper, (x, -0.035, z), (w, 0.01, h), rot=(0, tilt, 0), bevel=0)
        for line in range(3):
            mvp.box(f"TC_Notice_Line_{i}_{line}", ink, (x, -0.042, z + h * 0.3 - line * h * 0.22), (w * 0.7, 0.004, 0.018),
                    rot=(0, tilt, 0), bevel=0)


def build_slate() -> None:
    """Net-maker's slate on an easel, chalked with tallies and a drawn tower."""
    timber = mvp.pbr("TC_POST_Timber", TIMBER, rough=0.85)
    slate = mvp.pbr("TC_POST_Slate", SLATE, rough=0.8)
    chalk = mvp.pbr("TC_POST_Chalk", CHALK, rough=0.95)
    for side in (-1, 1):
        mvp.strut(f"TC_Slate_Leg_{side}", timber, (0.35 * side, 0.15, 0.0), (0.3 * side, 0.0, 1.2), 0.03, 0.025)
    mvp.strut("TC_Slate_BackLeg", timber, (0, -0.4, 0.0), (0, 0.0, 1.15), 0.025, 0.02)
    mvp.box("TC_Slate_Board", slate, (0, 0.05, 0.85), (0.8, 0.03, 0.6), rot=(-12, 0, 0), bevel=0.01)
    for i in range(5):
        mvp.box(f"TC_Slate_Tally_{i}", chalk, (-0.28 + i * 0.05, 0.03, 0.98), (0.012, 0.004, 0.12), rot=(-12, 0, 0), bevel=0)
    mvp.box("TC_Slate_TallyCross", chalk, (-0.18, 0.03, 0.98), (0.26, 0.004, 0.012), rot=(-12, 0, 18), bevel=0)
    mvp.box("TC_Slate_TowerBody", chalk, (0.18, 0.03, 0.8), (0.07, 0.004, 0.26), rot=(-12, 0, 0), bevel=0)
    mvp.box("TC_Slate_TowerTop", chalk, (0.18, 0.03, 0.95), (0.13, 0.004, 0.02), rot=(-12, 0, 0), bevel=0)


def build_toy_boat() -> None:
    """A child's carved boat with a cloth sail, left at the end of the lane."""
    timber = mvp.pbr("TC_POST_Timber", TIMBER, rough=0.85)
    cloth = mvp.pbr("TC_POST_SailCloth", (0.615686, 0.325490, 0.211765), rough=0.9)
    paint = mvp.pbr("TC_POST_BoatBlue", (0.12, 0.32, 0.45), rough=0.7)
    mvp.taper("TC_Boat_Hull", timber, (0, 0, 0.08), 0.09, 0.16, 0.16, rot=(0, 0, 0), scale=(1.0, 2.6, 1.0), vertices=8)
    mvp.box("TC_Boat_Stripe", paint, (0, 0, 0.13), (0.24, 0.5, 0.02), bevel=0)
    mvp.cyl("TC_Boat_Mast", timber, (0, 0.03, 0.34), 0.012, 0.42, vertices=6, bevel=0)
    mvp.taper("TC_Boat_Sail", cloth, (0, 0.08, 0.36), 0.16, 0.0, 0.32, rot=(90, 0, 0), scale=(1.0, 1.0, 0.05), vertices=3)


def build_flight_recorder() -> None:
    """The crashed ship's flight recorder, thrown clear: orange case, a blinking light."""
    orange = mvp.pbr("TC_MVP_OrangePaint", mvp.ORANGE_PAINT, rough=0.42)
    dark = mvp.pbr("TC_MVP_DarkMetal", mvp.DARK_METAL, rough=0.48, metal=0.65)
    cyan = mvp.pbr("TC_MVP_CyanLED", (0.0, 0.02, 0.02), rough=0.3, emission=mvp.CYAN_GLOW, strength=5.0)
    soot = mvp.pbr("TC_MVP_SootMetal", (0.055, 0.050, 0.048), rough=0.75, metal=0.35)
    mvp.box("TC_Recorder_Case", orange, (0, 0, 0.16), (0.46, 0.3, 0.3), rot=(0, 8, 14), bevel=0.03)
    mvp.box("TC_Recorder_Scorch", soot, (0.12, -0.08, 0.27), (0.25, 0.2, 0.12), rot=(0, 8, 14), bevel=0.02)
    mvp.cyl("TC_Recorder_Handle", dark, (0, 0, 0.36), 0.02, 0.32, rot=(0, 90, 14), vertices=6, bevel=0)
    mvp.box("TC_Recorder_Light", cyan, (-0.16, -0.16, 0.2), (0.06, 0.02, 0.04), rot=(0, 8, 14), bevel=0)
    mvp.text_marking("TC_Recorder_Label", "TC-07", dark, (0.02, -0.158, 0.13), 0.06, (90, 0, 14))


def build_stripped_machine() -> None:
    """A street water-pump cabinet opened and gutted: every board and cell taken, wires hanging."""
    steel = mvp.pbr("TC_MVP_SteelScrap", mvp.STEEL_SCRAP, rough=0.5, metal=0.8)
    dark = mvp.pbr("TC_MVP_DarkMetal", mvp.DARK_METAL, rough=0.48, metal=0.65)
    wire = mvp.pbr("TC_POST_Wire", (0.45, 0.2, 0.06), rough=0.6, metal=0.6)
    purple = mvp.pbr("TC_MVP_PurpleGlow", (0.01, 0.0, 0.03), rough=0.3, emission=mvp.PURPLE_GLOW, strength=2.0)
    mvp.box("TC_Machine_Body", steel, (0, 0, 0.65), (0.7, 0.45, 1.3), bevel=0.02)
    mvp.box("TC_Machine_Hollow", dark, (0, -0.2, 0.7), (0.56, 0.08, 1.0), bevel=0)
    mvp.box("TC_Machine_Door", steel, (-0.55, -0.45, 0.7), (0.04, 0.6, 1.0), rot=(0, 0, -58), bevel=0.01)
    for i, x in enumerate((-0.18, -0.05, 0.1, 0.2)):
        mvp.cable(f"TC_Machine_Wire_{i}", wire,
                  [(x, -0.22, 1.05), (x * 1.2, -0.3, 0.8), (x * 0.9, -0.38, 0.55), (x * 1.3, -0.45, 0.3)], 0.012)
    # The Galaxabrains' mark: a smear of their violet resin where a board was torn out.
    mvp.gem("TC_Machine_Resin", purple, (0.05, -0.24, 0.9), 0.07, (1.4, 0.4, 0.9))


def build_plaque() -> None:
    """A carved coral-stone plaque set in the tower court wall."""
    stone = mvp.pbr("TC_MAT_CoralRag", CORAL_STONE, rough=0.95)
    shadow = mvp.pbr("TC_MAT_CoralWeathered", CORAL_SHADOW, rough=0.96)
    mvp.box("TC_Plaque_Slab", stone, (0, 0, 0.5), (0.9, 0.08, 0.6), bevel=0.02)
    mvp.box("TC_Plaque_Frame", shadow, (0, 0.01, 0.5), (0.96, 0.06, 0.66), bevel=0.01)
    for i in range(4):
        mvp.box(f"TC_Plaque_Carving_{i}", shadow, (0, -0.045, 0.66 - i * 0.1), (0.62 - (i % 2) * 0.14, 0.01, 0.025), bevel=0)


def build_court_paving() -> None:
    """A 4 m square of worn coral-stone paving slabs, tiled across the courts."""
    stone = mvp.pbr("TC_MAT_CoralRag", CORAL_STONE, rough=0.95)
    shadow = mvp.pbr("TC_MAT_CoralWeathered", CORAL_SHADOW, rough=0.96)
    lime = mvp.pbr("TC_MAT_LimeRender", LIME_WASH, rough=0.92)
    rng = random.Random(113)
    slab = 0.98
    for ix in range(4):
        for iy in range(4):
            mat = rng.choice([stone, stone, shadow, lime])
            height = 0.04 + rng.uniform(-0.012, 0.012)
            mvp.box(f"TC_Paving_{ix}_{iy}", mat, (-1.5 + ix, -1.5 + iy, height / 2.0), (slab, slab, height),
                    rot=(rng.uniform(-0.6, 0.6), rng.uniform(-0.6, 0.6), rng.uniform(-1.5, 1.5)), bevel=0.012)


ASSETS = {
    "TC_CHAR_GalaxabrainWarden_V1": build_warden,
    "TC_CHAR_GalaxabrainWarden_Guard_V1": build_warden_guard,
    "TC_CHAR_GalaxabrainWarden_Disabled_V1": build_warden_disabled,
    "TC_CHAR_GalaxabrainDrone_V1": build_drone,
    "TC_PROP_WreckBarricade_V1": build_wreck_barricade,
    "TC_ENV_CrackedGate_V1": build_cracked_gate,
    "TC_ENV_CrackedGate_Broken_V1": build_cracked_gate_broken,
    "TC_PICKUP_PowerCore_V1": build_power_core,
    "TC_PLAYER_ArmClamp_V1": build_arm_clamp,
    "TC_PLAYER_ArmCoil_V1": build_arm_coil,
    "TC_PROP_NoticeBoard_V1": build_notice_board,
    "TC_PROP_Slate_V1": build_slate,
    "TC_PROP_ToyBoat_V1": build_toy_boat,
    "TC_PROP_FlightRecorder_V1": build_flight_recorder,
    "TC_PROP_StrippedMachine_V1": build_stripped_machine,
    "TC_PROP_Plaque_V1": build_plaque,
    "TC_ENV_CourtPaving_V1": build_court_paving,
}


def main() -> None:
    only = None
    if "--" in sys.argv:
        tail = sys.argv[sys.argv.index("--") + 1:]
        if len(tail) == 2 and tail[0] == "--asset":
            only = tail[1]
        elif tail:
            raise SystemExit("usage: blender --background --python create_post_mvp_kit_v1.py [-- --asset TC_NAME]")
    names = [only] if only else list(ASSETS)
    for name in names:
        if name not in ASSETS:
            raise SystemExit(f"unknown asset: {name}")
        mvp.reset_scene()
        ASSETS[name]()
        output = SOURCE_DIR / f"{name}.blend"
        output.parent.mkdir(parents=True, exist_ok=True)
        bpy.ops.wm.save_as_mainfile(filepath=str(output))
        print(f"POST_MVP_KIT_BLEND_WRITTEN {output}")


if __name__ == "__main__":
    main()
