#!/usr/bin/env python3
"""Generate scenes/Environment/MweziQuarterDistrict.tscn from a declared layout.

Why a generator instead of hand-authored scene text
---------------------------------------------------
The district is ~50 placements whose Y must agree with the runtime-generated
visual terrain (`src/World/ProceduralCrashSiteTerrain.cs`). Hand-placing those
in scene text is unreviewable and drifts the moment a target moves. This script
declares the layout as data, solves each placement's ground height with the
same formula the terrain uses, asserts the gameplay clearances the integration
suite enforces, and emits the scene.

Layout intent (approximate, not survey data)
--------------------------------------------
The quarter's morphology is an approximation of a Swahili-Comorian coastal
stone quarter -- harbour retaining wall on the seaward edge, a dense grain of
narrow alleys behind it, a civic arcaded hall facing an open courtyard, and a
tower landmark visible over the roofs. It is built from architectural
morphology, not from survey data, aerial imagery or photographs, none of which
were available to the authoring environment. It names no real place and
reproduces no real building.

Gameplay contract preserved
---------------------------
Every mission anchor keeps its existing position (README section 14 loop):
spawn, three pickups, workbench, save point, scout encounter, beacon. The
district is dressed AROUND those anchors; the combat plaza is kept clear
because the Galaxabrain Scout steers directly at the player with no navmesh
(`src/Enemies/GalaxabrainScout.cs`), so geometry inside the arena would trap it.

Run:
  python3 tools/level/build_mwezi_quarter_district.py
"""
from __future__ import annotations

import math
from pathlib import Path
from typing import NamedTuple

ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "scenes/Environment/MweziQuarterDistrict.tscn"
MODEL_DIR = "res://assets/models/mwezi_quarter_v1"
MOTION_SCRIPT_ID = "900_motion_player"

# ---------------------------------------------------------------------------
# Terrain height, mirrored from src/World/ProceduralCrashSiteTerrain.cs so that
# building bases meet the visual terrain instead of floating over or sinking
# into it. Keep in sync with that file if the terrain constants change.
# ---------------------------------------------------------------------------
SEED = 31071
GRID_SPACING = 2.0
VISUAL_MARGIN = 18.0
CORRIDOR_WIDTH = 5.0
CORRIDOR_HEIGHT = 0.03
MAX_OUTSIDE_HEIGHT = 4.2

# Mission anchors as authored in scenes/Main/Main.tscn (x, z).
TARGETS = {
    "Player": (0.0, 0.0),
    "ResourceDrop_MetalPickup": (8.0, -4.0),
    "ResourceDrop_BiomassPickup": (-8.0, -4.0),
    "ResourceDrop_ElectronicsPickup": (0.0, -10.0),
    "Placeholder_Workbench": (12.0, -12.0),
    "Placeholder_GalaxabrainScout": (20.0, -16.0),
    "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup": (20.0, -16.0),
    "Placeholder_SavePoint": (-12.0, -12.0),
    "Placeholder_Beacon": (28.0, -20.0),
}

ROUTES = [
    ("Player", "ResourceDrop_MetalPickup"),
    ("Player", "ResourceDrop_BiomassPickup"),
    ("Player", "ResourceDrop_ElectronicsPickup"),
    ("ResourceDrop_MetalPickup", "Placeholder_Workbench"),
    ("ResourceDrop_BiomassPickup", "Placeholder_Workbench"),
    ("ResourceDrop_ElectronicsPickup", "Placeholder_Workbench"),
    ("Placeholder_Workbench", "Placeholder_GalaxabrainScout"),
    ("Placeholder_GalaxabrainScout", "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup"),
    ("Placeholder_GalaxabrainScout/GalaxabrainComponentPickup", "Placeholder_SavePoint"),
    ("Placeholder_SavePoint", "Placeholder_Beacon"),
]


def _bounds() -> tuple[float, float, float, float]:
    xs = [p[0] for p in TARGETS.values()]
    zs = [p[1] for p in TARGETS.values()]
    min_x, max_x = min(xs) - VISUAL_MARGIN, max(xs) + VISUAL_MARGIN
    min_z, max_z = min(zs) - VISUAL_MARGIN, max(zs) + VISUAL_MARGIN
    return (math.floor(min_x / GRID_SPACING) * GRID_SPACING,
            math.ceil(max_x / GRID_SPACING) * GRID_SPACING,
            math.floor(min_z / GRID_SPACING) * GRID_SPACING,
            math.ceil(max_z / GRID_SPACING) * GRID_SPACING)


def _distance_point_segment(p, a, b) -> float:
    ax, az = a
    bx, bz = b
    abx, abz = bx - ax, bz - az
    denominator = abx * abx + abz * abz
    if denominator <= 0.0001:
        return math.dist(p, a)
    t = max(0.0, min(1.0, ((p[0] - ax) * abx + (p[1] - az) * abz) / denominator))
    return math.dist(p, (ax + abx * t, az + abz * t))


def distance_to_route(x: float, z: float) -> float:
    p = (x, z)
    best = min(_distance_point_segment(p, TARGETS[a], TARGETS[b]) for a, b in ROUTES)
    best = min(best, min(math.dist(p, t) - 2.0 for t in TARGETS.values()))
    return max(0.0, best)


def _hash01(x: int, z: int) -> float:
    h = (x * 374761393 + z * 668265263 + SEED * 1442695041) & 0xFFFFFFFF
    h = ((h ^ (h >> 13)) * 1274126177) & 0xFFFFFFFF
    return ((h ^ (h >> 16)) & 0xFFFFFF) / 16777215.0


def _facet_noise(x: float, z: float) -> float:
    return _hash01(math.floor((x + SEED) / 4.0), math.floor((z - SEED) / 4.0)) - 0.5


def _point_in_box(x, z, min_x, max_x, min_z, max_z) -> bool:
    return min_x <= x <= max_x and min_z <= z <= max_z


def _zone(x: float, z: float, distance: float, bounds) -> str:
    min_x, max_x, min_z, max_z = bounds
    if distance <= CORRIDOR_WIDTH * 0.5:
        return "AshRoute"
    if min(x - min_x, max_x - x, z - min_z, max_z - z) < 8.5:
        return "HorizonRidge"
    if _point_in_box(x, z, -18, -5, -14, 4):
        return "SpawnBasaltShelf"
    if _point_in_box(x, z, -9, 13, -11, -2):
        return "ResourceBasaltShelf"
    if _point_in_box(x, z, 6, 18, -27, -18):
        return "WorkbenchRidge"
    if _point_in_box(x, z, 15, 31, -28, -20):
        return "CombatRidge"
    if _point_in_box(x, z, 22, 40, -28, -13):
        return "BeaconBasaltShelf"
    return "CentralPlateau"


ZONE_BASE = {
    "CentralPlateau": (0.10, 0.10),
    "SpawnBasaltShelf": (0.34, 0.22),
    "ResourceBasaltShelf": (0.42, 0.24),
    "WorkbenchRidge": (1.05, 0.45),
    "CombatRidge": (1.25, 0.55),
    "BeaconBasaltShelf": (0.95, 0.38),
}


def terrain_height(x: float, z: float) -> float:
    bounds = _bounds()
    distance = distance_to_route(x, z)
    if distance <= CORRIDOR_WIDTH * 0.5:
        return CORRIDOR_HEIGHT
    zone = _zone(x, z, distance, bounds)
    faceted = _facet_noise(x, z)
    if zone == "AshRoute":
        return CORRIDOR_HEIGHT
    if zone == "HorizonRidge":
        min_x, max_x, min_z, max_z = bounds
        edge = min(x - min_x, max_x - x, z - min_z, max_z - z)
        height = 1.30 + max(0.0, 8.5 - edge) * 0.34 + faceted * 0.28
    else:
        base, spread = ZONE_BASE[zone]
        height = base + faceted * spread
    transition = max(0.0, min(1.0, (distance - CORRIDOR_WIDTH * 0.5) / 9.0))
    blended = CORRIDOR_HEIGHT + (height - CORRIDOR_HEIGHT) * transition
    natural = max(CORRIDOR_HEIGHT, min(blended, MAX_OUTSIDE_HEIGHT))
    paved = natural + (CORRIDOR_HEIGHT - natural) * paved_weight(x, z)
    return paved + (SEA_FLOOR_HEIGHT - paved) * sea_weight(x)


# East of the harbour wall the ground falls away under the sea (workstream D).
# Mirrors ProceduralCrashSiteTerrain.SeaWeight.
SEA_SHORE_X = 36.5
SEA_SHORE_WIDTH = 8.0
SEA_FLOOR_HEIGHT = -1.2


def sea_weight(x: float) -> float:
    t = max(0.0, min(1.0, (x - SEA_SHORE_X) / SEA_SHORE_WIDTH))
    return t * t * (3.0 - 2.0 * t)


# Courts the player walks in that the natural terrain would bury (workstream
# B): the visual terrain has no collision, so ground the player stands on must
# also look like ground there. Mirrors ProceduralCrashSiteTerrain.PavedCourts.
PAVED_COURTS = [
    (-21.5, -12.1, -29.5, -17.8),   # the Net-makers' Yard and its gate front
    (4.8, 20.4, -29.5, -20.5),      # the tower court and its gate front
]
PAVED_BLEND_M = 1.5


def paved_weight(x: float, z: float) -> float:
    weight = 0.0
    for min_x, max_x, min_z, max_z in PAVED_COURTS:
        dx = max(min_x - x, x - max_x, 0.0)
        dz = max(min_z - z, z - max_z, 0.0)
        weight = max(weight, max(0.0, min(1.0, 1.0 - math.hypot(dx, dz) / PAVED_BLEND_M)))
    return weight


# ---------------------------------------------------------------------------
# Layout declaration.
#   (asset, x, z, yaw_degrees, uniform_scale, collision)
# collision is None, or a list of (size_xyz, offset_xyz, pitch_degrees) boxes
# expressed in the placement's local space.
# ---------------------------------------------------------------------------
HOUSE_COLLISION = [((7.2, 6.2, 6.2), (0.0, 3.1, 0.0), 0.0)]
WALL_COLLISION = [((6.2, 3.4, 0.6), (0.0, 1.7, 0.0), 0.0)]
# Arcade piers: the arches stay walkable, the stone does not.
# Arch openings are 2.6 m clear between 0.8 m piers, so the arcade is
# walk-through architecture, not a wall.
ARCADE_PASSAGE_WIDTH = 2.6
ARCADE_COLLISION = [((0.8, 2.4, 1.1), (x, 1.2, 0.0), 0.0, ARCADE_PASSAGE_WIDTH)
                    for x in (-5.15, -1.85, 1.85, 5.15)]
TOWER_COLLISION = [((3.4, 9.0, 3.4), (0.0, 4.5, 0.0), 0.0)]
SEAWALL_COLLISION = [((10.4, 1.7, 1.2), (0.0, 0.85, 0.0), 0.0)]
# Stair cheek ramp plus the platform: a rotated box is a walkable slope for a
# CharacterBody3D, so the terrace is real verticality rather than a wall.
TERRACE_COLLISION = [
    ((4.4, 0.4, 4.6), (0.0, 0.95, -2.5), -27.0),
    ((6.0, 1.9, 5.0), (0.0, 0.95, 1.6), 0.0),
]

LAYOUT = [
    # --- Entry square: the breach the ship tore through the quarter ---------
    ("StoneHouse", -8.5, 4.2, 4.0, 1.0, HOUSE_COLLISION),
    ("StoneHouse", 8.8, 4.6, -6.0, 1.0, HOUSE_COLLISION),
    ("CarvedDoorway", -8.3, 0.9, 0.0, 1.0, None),
    ("CoralWallSegment", -3.4, 5.4, 90.0, 1.0, WALL_COLLISION),
    ("CoralWallSegment", 3.6, 5.8, 90.0, 1.0, WALL_COLLISION),
    ("CoralRubble", 2.6, 1.6, 24.0, 1.15, None),
    ("CoralRubble", -3.2, 2.4, -40.0, 0.9, None),
    ("PalmCluster", 5.4, 0.4, 15.0, 1.0, None),

    # --- West alley to the biomass source and the save-point square ---------
    ("CoralWallSegment", -12.8, -2.6, 0.0, 1.0, WALL_COLLISION),
    ("CarvedDoorway", -12.6, -6.2, 90.0, 1.0, None),
    ("StoneHouse", -17.0, -6.2, 90.0, 1.0, HOUSE_COLLISION),
    ("CoralWallSegment", -6.4, -8.6, 24.0, 1.0, WALL_COLLISION),
    ("CoralRubble", -8.6, -8.4, 62.0, 1.0, None),
    ("StoneHouse", -16.8, -14.2, 90.0, 1.0, HOUSE_COLLISION),
    ("MarketStall", -9.6, -14.2, 18.0, 1.0, None),
    ("MarketStall", -14.0, -10.6, -58.0, 1.0, None),
    ("PalmCluster", -15.4, -16.2, -20.0, 1.0, None),
    # Straightened (was 68 degrees) so the gap beside the house is an alley a
    # player walks up to the Net-makers' Yard gate, not a 0.9 m slot.
    ("CoralWallSegment", -11.2, -17.5, 90.0, 1.0, WALL_COLLISION),

    # --- East alley to the metal source ------------------------------------
    ("CoralWallSegment", 12.4, -3.0, 0.0, 1.0, WALL_COLLISION),
    ("CarvedDoorway", 12.2, -6.4, 90.0, 1.0, None),
    ("StoneHouse", 17.4, -4.4, 90.0, 1.0, HOUSE_COLLISION),
    ("CoralRubble", 5.8, -7.6, -18.0, 1.0, None),
    ("PalmCluster", 10.4, -8.8, 40.0, 1.0, None),

    # --- Central spine and the raised overlook ------------------------------
    ("StairTerrace", -6.6, -20.6, 24.0, 1.0, TERRACE_COLLISION),
    ("CoralRubble", -1.8, -15.4, 8.0, 0.85, None),

    # --- Workbench courtyard, fronted by the civic arcaded hall -------------
    # The arcade forms the courtyard's west face rather than its south side:
    # placed south it put stone piers inside the Scout's arena, which the
    # clearance assert below rejects because the Scout has no navmesh to
    # path around them.
    ("ArcadeBay", 5.4, -17.0, 90.0, 1.0, ARCADE_COLLISION),
    ("StoneHouse", 0.5, -22.5, -12.0, 1.0, HOUSE_COLLISION),
    ("MarketStall", 8.4, -13.9, -14.0, 1.0, None),
    ("PalmCluster", 17.8, -12.6, -35.0, 1.0, None),

    # --- Harbour plaza: kept open for the Scout encounter -------------------
    ("CoralWallSegment", 20.2, -23.2, 0.0, 1.0, WALL_COLLISION),
    ("StoneHouse", 30.2, -8.2, 90.0, 1.0, HOUSE_COLLISION),
    ("CoralRubble", 16.2, -20.8, 70.0, 1.1, None),
    ("PalmCluster", 24.8, -12.2, 10.0, 1.0, None),

    # --- Harbour front: tower landmark and the retaining wall ---------------
    ("QuarterTower", 22.6, -26.6, 12.0, 1.0, TOWER_COLLISION),
    ("SeawallRun", 30.4, -24.8, 22.0, 1.0, SEAWALL_COLLISION),
    ("SeawallRun", 34.6, -17.6, 96.0, 1.0, SEAWALL_COLLISION),
    ("PalmCluster", 31.6, -21.8, -12.0, 1.0, None),
    ("CoralRubble", 26.4, -24.2, -52.0, 1.0, None),

    # --- Motion pass: the quarter has to look lived-in, not abandoned -------
    # Placed on the routes the player actually walks, at the heights they
    # actually look at, because motion the player never sees is wasted.
    ("PalmSway", 6.2, 1.2, 12.0, 1.0, None),
    ("PalmSway", -10.6, -9.4, -28.0, 1.0, None),
    ("PalmSway", 16.4, -11.2, 44.0, 1.0, None),
    ("PalmSway", 25.6, -14.4, -8.0, 1.0, None),
    ("LaundryLine", -9.8, -6.4, 78.0, 1.0, None),
    ("LaundryLine", 9.2, -5.6, 96.0, 1.0, None),
    ("AwningCloth", -8.5, 0.9, 0.0, 1.0, None),
    ("AwningCloth", 12.2, -6.1, 90.0, 1.0, None),
    ("BannerCloth", -12.5, -3.2, 0.0, 1.0, None),
    ("BannerCloth", 20.2, -22.6, 0.0, 1.0, None),

    # --- Workstream B: the Net-makers' Yard (Mk II gate) --------------------
    # Walled on south and east; the quarter's boundary closes north and west.
    # Its one way in is a 2.65 m gap at the south-east corner, jammed with hull
    # wreckage (scenes/World/WreckBarricade.tscn) the magnet clamp pulls clear.
    ("CoralWallSegment", -18.45, -20.4, 0.0, 1.0, WALL_COLLISION),
    ("CoralWallSegment", -12.4, -23.5, 90.0, 1.0, WALL_COLLISION),
    ("CoralWallSegment", -12.4, -27.3, 90.0, 1.0, WALL_COLLISION),
    ("YardCornerPier", -11.8, -20.6, 0.0, 1.0, [((0.9, 3.4, 0.9), (0.0, 1.7, 0.0), 0.0)]),
    # The net-makers' things, left where they were working: stall, drying
    # nets, a palm. Collisionless so the drones and the player move freely.
    ("MarketStall", -19.4, -27.6, 12.0, 1.0, None),
    ("LaundryLine", -17.2, -23.9, 4.0, 1.0, None),
    ("CoralRubble", -20.2, -21.6, 30.0, 0.9, None),
    ("PalmCluster", -13.6, -28.4, -15.0, 1.0, None),

    # --- Workstream B: the tower court (Mk III gate) ------------------------
    # Between the civic hall and the tower, sealed by the Galaxabrains: the
    # gap at x 11.0-13.6 is bricked with a cracked infill the charged strike
    # breaks (scenes/World/CrackedGate.tscn). The south walls sit at z -22.9 so
    # the Scout's arena keeps its 6.5 m clearance.
    ("CoralWallSegment", 7.9, -22.9, 0.0, 1.0, WALL_COLLISION),
    ("CoralWallSegment", 16.7, -22.9, 0.0, 1.0, WALL_COLLISION),
    ("CoralWallSegment", 4.9, -27.4, 90.0, 1.0, WALL_COLLISION),
    ("CoralWallSegment", 20.3, -26.6, 90.0, 1.0, WALL_COLLISION),
    ("CoralRubble", 6.4, -28.6, 15.0, 1.0, None),
    ("CoralRubble", 19.0, -28.8, -40.0, 0.8, None),
    ("BannerCloth", 7.9, -23.3, 180.0, 1.0, None),
]

# The quarter's edge. The visual terrain rises into hills 8.5 m inside its
# bounds but carries no collision, so without this a player walked straight
# into the hillside meshes. Invisible boxes (x, z, size_x, size_z), 4 m tall,
# whose inner faces sit where the hills begin.
BOUNDARY = [
    ("Boundary_West", -22.0, -10.0, 1.0, 40.0),
    # East: stops at the shore where the ground falls under the sea.
    ("Boundary_East", 36.5, -10.0, 1.0, 40.0),
    ("Boundary_North", 8.0, -30.0, 61.0, 1.0),
    ("Boundary_South", 8.0, 10.0, 61.0, 1.0),
]
BOUNDARY_HEIGHT_M = 4.0

# Gameplay anchors inside the courts (authored in scenes/Main/Main.tscn), kept
# clear of the district's collision like the original mission anchors.
COURT_ANCHORS = {
    "yard centre": ((-17.0, -24.5), 2.0),
    "yard gate": ((-14.0, -20.4), 0.0),
    "tower gate": ((12.3, -22.9), 0.0),
    "warden spawn": ((12.6, -26.3), 2.6),
    "power core": ((7.6, -27.6), 1.0),
}

# Walked lines into the courts: they must stay as clear as the mission routes.
# The gates themselves are Main.tscn bodies, not district collision.
COURT_ROUTES = [
    ((-12.0, -12.0), (-12.6, -15.5)),
    ((-12.6, -15.5), (-12.6, -18.6)),
    ((-12.6, -18.6), (-14.0, -19.4)),
    ((-14.0, -19.4), (-14.0, -22.5)),
    ((-14.0, -22.5), (-17.0, -24.5)),
    ((12.0, -12.0), (12.3, -21.4)),
    ((12.3, -21.4), (12.3, -24.5)),
    ((12.3, -24.5), (12.6, -26.3)),
]

# Placements that are collision only (no model of their own).
COLLISION_ONLY_ASSETS = {"YardCornerPier"}

ASSET_FILES = {
    "CoralWallSegment": "TC_ENV_CoralWallSegment_V1",
    "ArcadeBay": "TC_ENV_ArcadeBay_V1",
    "QuarterTower": "TC_ENV_QuarterTower_V1",
    "CarvedDoorway": "TC_ENV_CarvedDoorway_V1",
    "StoneHouse": "TC_ENV_StoneHouse_V1",
    "StairTerrace": "TC_ENV_StairTerrace_V1",
    "SeawallRun": "TC_ENV_SeawallRun_V1",
    "MarketStall": "TC_ENV_MarketStall_V1",
    "PalmCluster": "TC_ENV_PalmCluster_V1",
    "CoralRubble": "TC_ENV_CoralRubble_V1",
    # Animated (skinned) assets from the motion kit. These are placed as
    # EnvironmentMotionPlayer nodes so their imported clip is looped and
    # phase-offset per instance.
    "PalmSway": "TC_ENV_PalmSway_V1",
    "LaundryLine": "TC_ENV_LaundryLine_V1",
    "AwningCloth": "TC_ENV_AwningCloth_V1",
    "BannerCloth": "TC_ENV_BannerCloth_V1",
}

# Assets whose placements carry EnvironmentMotionPlayer instead of a plain
# Node3D. All are visual-only dressing: motion never gains collision.
ANIMATED_ASSETS = {"PalmSway", "LaundryLine", "AwningCloth", "BannerCloth"}

# Sink each placement slightly so the base is buried rather than hovering over
# a facet edge of the vertex-coloured terrain.
BASE_SINK = 0.12

PICKUP_CLEARANCE_M = 1.2
SPAWN_CLEARANCE_M = 1.2
# The Scout steers straight at the player, so nothing with collision may sit
# inside the arena it fights in.
ARENA_CENTER = TARGETS["Placeholder_GalaxabrainScout"]
ARENA_CLEAR_RADIUS_M = 6.5
# Minimum walkable gap either side of a mission route's centre line.
ROUTE_CLEARANCE_M = 0.9
# Player capsule is 0.7 m across; an opening below this is a snag, not a door.
MIN_PASSAGE_WIDTH_M = 1.6


class Footprint(NamedTuple):
    """One collision box's world footprint, for clearance measurement."""

    x: float
    z: float
    yaw_radians: float
    half_x: float
    half_z: float
    name: str
    # A permeable structure has walkable openings between its solid members
    # (the arcade's arches). Route clearance is measured against the opening,
    # not against each pier, because walking between piers is the point.
    passage_width: float = 0.0

    def distance_to_point(self, point: tuple[float, float]) -> float:
        """Shortest distance from a world point to this box's surface (0 inside)."""
        dx, dz = point[0] - self.x, point[1] - self.z
        cos_a, sin_a = math.cos(-self.yaw_radians), math.sin(-self.yaw_radians)
        local_x = dx * cos_a + dz * sin_a
        local_z = -dx * sin_a + dz * cos_a
        outside_x = max(abs(local_x) - self.half_x, 0.0)
        outside_z = max(abs(local_z) - self.half_z, 0.0)
        return math.hypot(outside_x, outside_z)

    def distance_to_segment(self, a: tuple[float, float], b: tuple[float, float]) -> float:
        """Shortest distance from a walked route segment to this box's surface.

        Sampled rather than solved: a quarter-metre step is far finer than the
        0.9 m clearance being asserted, so a missed minimum cannot change a
        verdict.
        """
        length = math.dist(a, b)
        steps = max(2, int(length / 0.25) + 1)
        return min(
            self.distance_to_point((a[0] + (b[0] - a[0]) * i / steps,
                                    a[1] + (b[1] - a[1]) * i / steps))
            for i in range(steps + 1)
        )


def _yaw_basis(yaw_degrees: float, scale: float) -> tuple[float, ...]:
    angle = math.radians(yaw_degrees)
    cos_a, sin_a = math.cos(angle) * scale, math.sin(angle) * scale
    # Column-major Godot Transform3D basis for a Y rotation.
    return (cos_a, 0.0, -sin_a, 0.0, scale, 0.0, sin_a, 0.0, cos_a)


def _pitch_basis(pitch_degrees: float) -> tuple[float, ...]:
    angle = math.radians(pitch_degrees)
    cos_a, sin_a = math.cos(angle), math.sin(angle)
    return (1.0, 0.0, 0.0, 0.0, cos_a, sin_a, 0.0, -sin_a, cos_a)


# ---------------------------------------------------------------------------
# Route reading. The 2026-09-24 playtest found the street carried no route at
# eye level: the player navigated by HUD text. A warm pool of light over every
# objective lights each arrival, so the next place to go is the lit one.
# Visual only: no collision, no shadows.
#
# Worn-ground strips were tried and removed (2026-09-27): the ground near the
# routes is several overlapping visual layers (procedural terrain, Stage A
# fractured ground up to 0.64 m, ash patches), so no flat strip height sits on
# the visible surface, and a Decal -- the right tool -- cannot be rendered by
# the Compatibility renderer the review captures use. Ground wear belongs with
# the textured-materials pass (workstream D).
# ---------------------------------------------------------------------------
THRESHOLD_LIGHT_HEIGHT_M = 2.8
THRESHOLD_LIGHT_RANGE_M = 5.5
THRESHOLD_LIGHT_ENERGY = 1.4
THRESHOLD_LIGHT_COLOUR = (1.0, 0.74, 0.46)
# The component drops where the Scout dies; one light serves both.
THRESHOLD_TARGETS = [name for name in TARGETS
                     if name not in ("Player", "Placeholder_GalaxabrainScout/GalaxabrainComponentPickup")]


def _route_reading() -> list[str]:
    """Nodes for the threshold lights over each objective."""
    nodes = ['[node name="RouteReading" type="Node3D" parent="."]',
             'metadata/purpose = "Threshold lights: the route read from the street, not the HUD (workstream A)"']
    lr, lg, lb = THRESHOLD_LIGHT_COLOUR
    for name in THRESHOLD_TARGETS:
        x, z = TARGETS[name]
        label = name.replace("Placeholder_", "").replace("ResourceDrop_", "")
        nodes.append(f'[node name="ThresholdLight_{label}" type="OmniLight3D" parent="RouteReading"]')
        nodes.append(f"transform = {_transform(_yaw_basis(0.0, 1.0), (x, THRESHOLD_LIGHT_HEIGHT_M, z))}")
        nodes.append(f"light_color = Color({lr}, {lg}, {lb}, 1)")
        nodes.append(f"light_energy = {THRESHOLD_LIGHT_ENERGY}")
        nodes.append(f"omni_range = {THRESHOLD_LIGHT_RANGE_M}")
        nodes.append(f'metadata/target = "{name}"')
    return nodes


# ---------------------------------------------------------------------------
# Workstream D: the courts are paved, and the routes the player walks most
# carry worn ground. Paving is the post-MVP kit's 4 m slab tile, scaled to fit
# each court; wear is Decal projection (Forward+, the shipped renderer), which
# lands on whatever ground layer is on top -- the reason flat strips failed
# (see the route-reading note above).
# ---------------------------------------------------------------------------
PAVING_MODEL = "res://assets/models/post_mvp_kit_v1/TC_ENV_CourtPaving_V1.gltf"
# (centre x, centre z, scale x, scale z) per tile; the model tile is 4 m.
PAVING_TILES = (
    [(x, z, 0.75, 0.75) for x in (-20.0, -17.0, -14.0) for z in (-28.0, -25.0, -22.0)]
    + [(x, z, 0.75, 0.8) for x in (6.7, 9.7, 12.7, 15.7, 18.7) for z in (-28.0, -24.8)]
)
WEAR_WIDTH_M = 2.4
WEAR_PROJECTION_M = 1.0
WEAR_SEGMENTS = ROUTES + [
    # Act 2 and 3 routes a player walks again and again.
    ("Placeholder_Workbench", "Placeholder_SavePoint"),
]


def _paving_and_wear() -> tuple[list[str], list[str], list[str]]:
    """Ext-resources, sub-resources and nodes for the court paving and route wear."""
    resources = [f'[ext_resource type="PackedScene" path="{PAVING_MODEL}" id="800_court_paving"]']
    subs = [
        '[sub_resource type="FastNoiseLite" id="Noise_RouteWear"]',
        "noise_type = 3",
        "frequency = 0.045",
        "fractal_octaves = 4",
        "seed = 4207",
        '[sub_resource type="Gradient" id="Gradient_RouteWear"]',
        "offsets = PackedFloat32Array(0, 0.45, 1)",
        "colors = PackedColorArray(0.25, 0.21, 0.17, 0, 0.3, 0.25, 0.2, 0.35, 0.36, 0.3, 0.24, 0.7)",
        '[sub_resource type="NoiseTexture2D" id="Tex_RouteWear"]',
        "width = 256",
        "height = 512",
        "seamless = true",
        'color_ramp = SubResource("Gradient_RouteWear")',
        'noise = SubResource("Noise_RouteWear")',
    ]
    nodes = ['[node name="CourtPaving" type="Node3D" parent="."]',
             'metadata/purpose = "Workstream D: worn coral-stone paving in the yard and the tower court."']
    for index, (x, z, sx, sz) in enumerate(PAVING_TILES):
        nodes.append(f'[node name="Paving_{index:02d}" parent="CourtPaving" instance=ExtResource("800_court_paving")]')
        nodes.append(f"transform = Transform3D({_fmt(sx)}, 0, 0, 0, 1, 0, 0, 0, {_fmt(sz)}, {_fmt(x)}, 0.0, {_fmt(z)})")
    nodes.append('[node name="RouteWear" type="Node3D" parent="."]')
    nodes.append('metadata/purpose = "Workstream D: worn ground along the walked routes, projected as decals onto whatever ground layer is on top."')
    for index, (a, b) in enumerate(WEAR_SEGMENTS):
        ax, az = TARGETS[a]
        bx, bz = TARGETS[b]
        length = math.dist((ax, az), (bx, bz))
        if length < 1.0:
            continue
        yaw = math.degrees(math.atan2(bx - ax, bz - az))
        basis = _yaw_basis(yaw, 1.0)
        nodes.append(f'[node name="Wear_{index:02d}" type="Decal" parent="RouteWear"]')
        nodes.append(f"transform = {_transform(basis, ((ax + bx) / 2.0, 0.1, (az + bz) / 2.0))}")
        nodes.append(f"size = Vector3({_fmt(WEAR_WIDTH_M)}, {_fmt(WEAR_PROJECTION_M)}, {_fmt(length + 1.5)})")
        nodes.append('texture_albedo = SubResource("Tex_RouteWear")')
        nodes.append("albedo_mix = 0.85")
        nodes.append("upper_fade = 0.2")
        nodes.append("lower_fade = 0.2")
        nodes.append("distance_fade_enabled = true")
        nodes.append("distance_fade_begin = 45.0")
        nodes.append("distance_fade_length = 10.0")
        nodes.append(f'metadata/route = "{a} -> {b}"')
    return resources, subs, nodes


def _fmt(value: float) -> str:
    return f"{value:.4f}".rstrip("0").rstrip(".") or "0"


def _transform(basis: tuple[float, ...], origin: tuple[float, float, float]) -> str:
    return "Transform3D(" + ", ".join(_fmt(v) for v in (*basis, *origin)) + ")"


def build() -> str:
    resources: list[str] = []
    for index, key in enumerate(ASSET_FILES):
        resources.append(
            f'[ext_resource type="PackedScene" '
            f'path="{MODEL_DIR}/{ASSET_FILES[key]}.gltf" id="{index + 1}_{key.lower()}"]'
        )
    resource_ids = {key: f"{index + 1}_{key.lower()}" for index, key in enumerate(ASSET_FILES)}
    resources.append(
        f'[ext_resource type="Script" path="res://src/World/EnvironmentMotionPlayer.cs" '
        f'id="{MOTION_SCRIPT_ID}"]'
    )

    shapes: dict[tuple[float, float, float], str] = {}
    shape_blocks: list[str] = []
    nodes: list[str] = []
    collision_boxes: list[Footprint] = []
    counters: dict[str, int] = {}

    for asset, x, z, yaw, scale, collision in LAYOUT:
        counters[asset] = counters.get(asset, 0) + 1
        collision_only = asset in COLLISION_ONLY_ASSETS
        name = f"Quarter_{asset}_{counters[asset]}"
        ground = terrain_height(x, z)
        y = ground - BASE_SINK
        animated = asset in ANIMATED_ASSETS
        group = "Structures" if collision else ("Motion" if animated else "Dressing")
        node_type = "StaticBody3D" if collision else "Node3D"
        nodes.append(f'[node name="{name}" type="{node_type}" parent="{group}"]')
        nodes.append(f"transform = {_transform(_yaw_basis(yaw, scale), (x, y, z))}")
        nodes.append(
            f'metadata/placement = "terrain_height={ground:.3f} sink={BASE_SINK} '
            f'zone={_zone(x, z, distance_to_route(x, z), _bounds())} '
            f'route_distance={distance_to_route(x, z):.2f}"'
        )
        if animated:
            nodes.append(f'script = ExtResource("{MOTION_SCRIPT_ID}")')
        if not collision_only:
            nodes.append(
                f'[node name="Model" parent="{group}/{name}" '
                f'instance=ExtResource("{resource_ids[asset]}")]'
            )
        if not collision:
            continue
        for shape_index, entry in enumerate(collision):
            size, offset, pitch = entry[0], entry[1], entry[2]
            passage_width = entry[3] if len(entry) > 3 else 0.0
            if size not in shapes:
                shape_id = f"BoxShape3D_quarter_{len(shapes)}"
                shapes[size] = shape_id
                shape_blocks.append(f'[sub_resource type="BoxShape3D" id="{shape_id}"]')
                shape_blocks.append(
                    f"size = Vector3({_fmt(size[0])}, {_fmt(size[1])}, {_fmt(size[2])})"
                )
            nodes.append(
                f'[node name="Collision_QuarterBlock_{shape_index}" type="CollisionShape3D" '
                f'parent="{group}/{name}"]'
            )
            nodes.append(f"transform = {_transform(_pitch_basis(pitch), offset)}")
            nodes.append(f'shape = SubResource("{shapes[size]}")')
            # World footprint of this shape for the clearance asserts: centre,
            # yaw and half-extents, so the checks measure distance to the box
            # SURFACE the player and the Scout actually collide with. A
            # circumscribed radius would be simpler and wrong -- it forbids the
            # narrow alleys that give the quarter its grain.
            angle = math.radians(yaw)
            world_x = x + (offset[0] * math.cos(angle) + offset[2] * math.sin(angle)) * scale
            world_z = z + (-offset[0] * math.sin(angle) + offset[2] * math.cos(angle)) * scale
            collision_boxes.append(
                Footprint(world_x, world_z, angle,
                          size[0] / 2.0 * scale, size[2] / 2.0 * scale, name,
                          passage_width * scale)
            )

    boundary_nodes = []
    for label, bx, bz, size_x, size_z in BOUNDARY:
        size = (size_x, BOUNDARY_HEIGHT_M, size_z)
        if size not in shapes:
            shape_id = f"BoxShape3D_quarter_{len(shapes)}"
            shapes[size] = shape_id
            shape_blocks.append(f'[sub_resource type="BoxShape3D" id="{shape_id}"]')
            shape_blocks.append(f"size = Vector3({_fmt(size[0])}, {_fmt(size[1])}, {_fmt(size[2])})")
        boundary_nodes.append(f'[node name="{label}" type="StaticBody3D" parent="Boundary"]')
        boundary_nodes.append(f"transform = {_transform(_yaw_basis(0.0, 1.0), (bx, BOUNDARY_HEIGHT_M / 2.0, bz))}")
        boundary_nodes.append(f'[node name="Collision_Boundary" type="CollisionShape3D" parent="Boundary/{label}"]')
        boundary_nodes.append(f'shape = SubResource("{shapes[size]}")')
        collision_boxes.append(Footprint(bx, bz, 0.0, size_x / 2.0, size_z / 2.0, label))

    _assert_clearances(collision_boxes)
    reading_nodes = _route_reading()
    paving_resources, paving_subs, paving_nodes = _paving_and_wear()

    resources.extend(paving_resources)
    shape_blocks.extend(paving_subs)
    header = (
        f'[gd_scene load_steps={len(resources) + len(shapes) + 4} format=3 '
        f'uid="uid://mwezi_quarter_district"]'
    )
    root = [
        '[node name="MweziQuarterDistrict" type="Node3D"]',
        'metadata/authoring = "Generated by tools/level/build_mwezi_quarter_district.py -- '
        'edit the layout there, never this file."',
        'metadata/collision_policy = "Structures carry explicit BoxShape3D gameplay collision; '
        'Dressing is visual-only and collisionless."',
        'metadata/reference = "Approximate coastal coral-stone quarter morphology; not survey '
        'data, and no real place or building is named or reproduced."',
        '[node name="Structures" type="Node3D" parent="."]',
        '[node name="Dressing" type="Node3D" parent="."]',
        '[node name="Motion" type="Node3D" parent="."]',
        '[node name="Boundary" type="Node3D" parent="."]',
        'metadata/purpose = "Invisible quarter edge where the visual hills begin (the terrain has no collision)."',
    ]
    return "\n".join([header, "", *resources, "", *shape_blocks, "", *root, *nodes,
                      *boundary_nodes, *reading_nodes, *paving_nodes]) + "\n"


def _assert_clearances(collision_boxes: list["Footprint"]) -> None:
    """Reject any layout that buries a mission anchor or crowds the arena.

    Every check measures distance to the collision box's surface, not to its
    centre: a 7 m house whose centre is 4 m from a pickup still swallows it,
    while a wall 2 m from an alley's centre line is exactly what the quarter's
    street grain is supposed to look like.
    """
    failures: list[str] = []
    # Every anchor the player must stand at, reach, or fight in must stay
    # outside the district's collision volumes by a usable margin.
    anchors = {
        "player spawn": (TARGETS["Player"], SPAWN_CLEARANCE_M),
        "metal pickup": (TARGETS["ResourceDrop_MetalPickup"], PICKUP_CLEARANCE_M),
        "biomass pickup": (TARGETS["ResourceDrop_BiomassPickup"], PICKUP_CLEARANCE_M),
        "electronics pickup": (TARGETS["ResourceDrop_ElectronicsPickup"], PICKUP_CLEARANCE_M),
        "workbench": (TARGETS["Placeholder_Workbench"], PICKUP_CLEARANCE_M),
        "save point": (TARGETS["Placeholder_SavePoint"], PICKUP_CLEARANCE_M),
        "beacon": (TARGETS["Placeholder_Beacon"], PICKUP_CLEARANCE_M),
        "scout spawn": (TARGETS["Placeholder_GalaxabrainScout"], PICKUP_CLEARANCE_M),
    }
    for label, (target, clearance) in anchors.items():
        for box in collision_boxes:
            gap = box.distance_to_point(target)
            if gap <= clearance:
                failures.append(
                    f"{label}: {box.name} collision surface is {gap:.2f} m away "
                    f"(needs > {clearance})"
                )
    for box in collision_boxes:
        gap = box.distance_to_point(ARENA_CENTER)
        if gap < ARENA_CLEAR_RADIUS_M:
            failures.append(
                f"Scout arena: {box.name} collision surface is {gap:.2f} m from the centre "
                f"(needs >= {ARENA_CLEAR_RADIUS_M})"
            )
    # The mission route segments are the paths the player walks between
    # anchors; a building standing on one is a dead end, not a detour.
    for box in collision_boxes:
        if box.passage_width and box.passage_width < MIN_PASSAGE_WIDTH_M:
            failures.append(
                f"{box.name}: {box.passage_width:.2f} m opening is too narrow to walk "
                f"(needs >= {MIN_PASSAGE_WIDTH_M})"
            )
    for a, b in ROUTES:
        for box in collision_boxes:
            if box.passage_width:
                continue
            gap = box.distance_to_segment(TARGETS[a], TARGETS[b])
            if gap <= ROUTE_CLEARANCE_M:
                failures.append(
                    f"route {a} -> {b}: {box.name} collision surface is {gap:.2f} m "
                    f"from the walked line (needs > {ROUTE_CLEARANCE_M})"
                )
    for label, (point, clearance) in COURT_ANCHORS.items():
        for box in collision_boxes:
            gap = box.distance_to_point(point)
            if gap <= clearance:
                failures.append(f"{label}: {box.name} collision surface is {gap:.2f} m away (needs > {clearance})")
    for a, b in COURT_ROUTES:
        for box in collision_boxes:
            if box.passage_width:
                continue
            gap = box.distance_to_segment(a, b)
            if gap <= ROUTE_CLEARANCE_M:
                failures.append(f"court route {a} -> {b}: {box.name} collision surface is {gap:.2f} m "
                                f"from the walked line (needs > {ROUTE_CLEARANCE_M})")
    if failures:
        raise SystemExit("LAYOUT_CLEARANCE_FAILED\n  " + "\n  ".join(sorted(set(failures))))
    print(f"LAYOUT_CLEARANCE_OK collision_shapes={len(collision_boxes)}")


def main() -> None:
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    OUTPUT.write_text(build(), encoding="utf-8")
    print(f"MWEZI_QUARTER_DISTRICT_WRITTEN {OUTPUT} placements={len(LAYOUT)}")


if __name__ == "__main__":
    main()
