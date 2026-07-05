#!/usr/bin/env python3
"""Offline authoring script for TitanCraft Stage A custom static OBJ meshes.

The script writes hand-authored low-poly meshes only. It does not run in Godot,
does not generate runtime geometry, and does not add gameplay collision.
"""
from __future__ import annotations

from math import cos, sin, pi
from pathlib import Path

OUT = Path("assets/Production/Custom/StageA")


def write_obj(name: str, vertices: list[tuple[float, float, float]], faces: list[tuple[int, ...]]) -> None:
    path = OUT / f"{name}.obj"
    with path.open("w", encoding="utf-8") as f:
        f.write(f"# TitanCraft custom Stage A static mesh: {name}\n")
        f.write("# Offline-authored by tools/art_generation/build_stage_a_assets.py\n")
        f.write("o " + name + "\n")
        for x, y, z in vertices:
            f.write(f"v {x:.4f} {y:.4f} {z:.4f}\n")
        for face in faces:
            f.write("f " + " ".join(str(i) for i in face) + "\n")


def box(name: str, sx: float, sy: float, sz: float, skew: float = 0.0, bite: bool = False) -> None:
    """Write a crushed debris block with sloped sides so route props do not read as default cubes."""
    x, y, z = sx / 2, sy / 2, sz / 2
    v = [
        (-x - skew, -y, -z*0.82), (x*0.82 - skew, -y, -z), (x + skew, -y, z*0.48), (-x*0.75 + skew, -y, z),
        (-x*0.62 + skew, y*0.42, -z*0.58), (x*0.58 + skew, y, -z*0.35), (x*0.7 - skew, y*0.55, z*0.38), (-x*0.48 - skew, y*0.72, z*0.55),
        (-x*0.18, y*1.18, -z*0.12), (x*0.22, -y*1.05, z*0.1),
    ]
    faces = [(1,2,6,5), (2,3,7,6), (3,4,8,7), (4,1,5,8), (5,6,9,8), (1,4,10,2), (2,10,3), (4,3,10)]
    if bite:
        v += [(0.12*sx, y*1.05, -z*1.05), (0.46*sx, y*0.92, -0.25*z), (0.16*sx, y*1.02, 0.28*z)]
        faces += [(6,11,12), (6,12,7), (7,12,13), (7,13,8)]
    write_obj(name, v, faces)

def wedge(name: str, sx: float, sy: float, sz: float, nose: float = 0.35) -> None:
    x, y, z = sx / 2, sy / 2, sz / 2
    v = [(-x,-y,-z), (x,-y,-z*nose), (x,-y,z*nose), (-x,-y,z), (-x,y,-z), (x,y*0.55,-z*nose), (x,y*0.55,z*nose), (-x,y,z)]
    faces = [(1,2,3,4), (5,8,7,6), (1,5,6,2), (2,6,7,3), (3,7,8,4), (4,8,5,1)]
    write_obj(name, v, faces)


def cylinder(name: str, radius_a: float, radius_b: float, depth: float, segments: int = 10, dent: float = 0.0) -> None:
    v: list[tuple[float, float, float]] = []
    for side, x in [(-1, -depth/2), (1, depth/2)]:
        r = radius_a if side < 0 else radius_b
        for i in range(segments):
            a = 2*pi*i/segments
            d = 1.0 - dent if i in (1, 2) and side > 0 else 1.0
            v.append((x, cos(a)*r*d, sin(a)*r*d))
    faces = []
    faces.append(tuple(range(segments, 0, -1)))
    faces.append(tuple(range(segments+1, segments*2+1)))
    for i in range(segments):
        a = i + 1
        b = (i + 1) % segments + 1
        c = segments + (i + 1) % segments + 1
        d = segments + i + 1
        faces.append((a, b, c, d))
    write_obj(name, v, faces)


def rock(name: str, sx: float, sy: float, sz: float, ridge: bool = False) -> None:
    """Write layered basalt with broken shelves rather than cone/cube primitive silhouettes."""
    x, z = sx/2, sz/2
    top = sy
    v = [
        (-x,0,-z*0.82), (-x*0.34,0,-z*1.08), (x*0.86,0,-z*0.72), (x,0,z*0.28),
        (x*0.36,0,z), (-x*0.9,0,z*0.64),
        (-x*0.82,top*0.24,-z*0.58), (-x*0.28,top*0.5,-z*0.78), (x*0.52,top*0.34,-z*0.46),
        (x*0.7,top*0.26,z*0.2), (x*0.14,top*0.42,z*0.68), (-x*0.62,top*0.36,z*0.44),
        (-x*0.48,top*0.74,-z*0.32), (x*0.08,top*0.98,-z*0.18), (x*0.46,top*0.68,z*0.08), (-x*0.18,top*0.82,z*0.34),
    ]
    faces = [
        (1,2,8,7), (2,3,9,8), (3,4,10,9), (4,5,11,10), (5,6,12,11), (6,1,7,12),
        (7,8,13), (8,9,14,13), (9,10,15,14), (10,11,16,15), (11,12,16), (12,7,13,16),
        (13,14,15,16), (1,6,5,4,3,2),
    ]
    if ridge:
        v += [(-x*0.14, top*1.28, -z*0.24), (x*0.34, top*1.08, z*0.18), (-x*0.58, top*0.96, z*0.06)]
        faces += [(13,17,14), (14,17,15), (15,18,16), (16,19,13)]
    write_obj(name, v, faces)


def ground(name: str, sx: float, sz: float, broken: bool = False) -> None:
    """Write a thickened crash basin with raised lips, interior lows, and ash mounds."""
    x, z = sx/2, sz/2
    top = [
        (-x,0.32,-z*0.48), (-x*0.66,0.46,-z), (-x*0.12,0.22,-z*1.08), (x*0.54,0.38,-z*0.82), (x,0.34,-z*0.32),
        (x*0.9,0.3,z*0.48), (x*0.32,0.2,z), (-x*0.42,0.34,z*0.92), (-x*0.92,0.38,z*0.44),
        (-x*0.42,-0.24,-z*0.38), (x*0.14,-0.32,-z*0.3), (x*0.42,-0.22,z*0.18), (-x*0.22,-0.2,z*0.34),
        (-x*0.6,0.58,-z*0.7), (x*0.68,0.54,-z*0.42), (x*0.58,0.46,z*0.52), (-x*0.74,0.5,z*0.42),
        (-x*0.2,0.22,-z*0.78), (x*0.18,0.12,z*0.72),
    ]
    v = top[:]
    # Add a low skirt around the perimeter so terrain edges read as thickness/berm, not floating cards.
    skirt_start = len(v) + 1
    for px, _py, pz in top[:9]:
        v.append((px, -0.62, pz))
    faces = [
        (1,2,10), (2,3,18,10), (3,4,11,18), (4,5,15,11), (5,6,12,15), (6,7,19,12),
        (7,8,13,19), (8,9,17,13), (9,1,10,17), (10,11,12,13), (10,13,17), (11,15,12),
        (2,14,3), (4,15,5), (6,16,7), (8,17,9),
    ]
    # Side wall faces for visible non-card terrain edge.
    for i in range(9):
        a = i + 1
        b = (i + 1) % 9 + 1
        c = skirt_start + (i + 1) % 9
        d = skirt_start + i
        faces.append((a, b, c, d))
    if broken:
        v += [(-x*0.78,0.78,-z*0.76), (x*0.76,0.72,-z*0.48), (x*0.64,0.64,z*0.62), (-x*0.84,0.68,z*0.52)]
        faces += [(2,29,3), (4,30,5), (6,31,7), (8,32,9)]
    write_obj(name, v, faces)

def ribs() -> None:
    v = []
    faces = []
    for idx, x in enumerate([-1.2, 0.0, 1.2]):
        base = len(v) + 1
        w, h, d = 0.18, 2.8 - abs(x)*0.35, 0.16
        v += [(x-w,0,-d), (x+w,0,-d), (x+w,h,-d), (x-w,h,-d), (x-w,0,d), (x+w,0,d), (x+w,h,d), (x-w,h,d)]
        faces += [(base,base+1,base+2,base+3), (base+4,base+7,base+6,base+5), (base,base+4,base+5,base+1), (base+1,base+5,base+6,base+2), (base+2,base+6,base+7,base+3), (base+3,base+7,base+4,base)]
    write_obj("TC_InternalRibs", v, faces)



def fuselage(name: str, sections: list[tuple[float, float, float, float]]) -> None:
    """Write an faceted octagonal spacecraft hull from x/y/z/rib-scale sections."""
    ring = [(-0.55,-1), (0.55,-1), (1,-0.52), (1,0.52), (0.55,1), (-0.55,1), (-1,0.45), (-1,-0.55)]
    v: list[tuple[float, float, float]] = []
    for x, sy, sz, lift in sections:
        for y, z in ring:
            v.append((x, y * sy + lift, z * sz))
    faces: list[tuple[int, ...]] = []
    n = len(ring)
    faces.append(tuple(range(n, 0, -1)))
    faces.append(tuple(range((len(sections)-1)*n+1, len(sections)*n+1)))
    for si in range(len(sections)-1):
        base = si * n
        nxt = (si + 1) * n
        for i in range(n):
            faces.append((base+i+1, base+(i+1)%n+1, nxt+(i+1)%n+1, nxt+i+1))
    # Add raised torn spine and side cheek plates so Blender/Godot review keeps a non-box silhouette.
    if len(sections) >= 3:
        top_a = len(v) + 1
        mid = len(sections) // 2
        v += [(sections[0][0] + 0.5, sections[0][1]*1.08, -sections[0][2]*0.2), (sections[mid][0], sections[mid][1]*1.25, -sections[mid][2]*0.05), (sections[-1][0] - 0.5, sections[-1][1]*1.05, 0.0)]
        faces += [(top_a, top_a+1, top_a+2)]
        cheek = len(v) + 1
        v += [
            (sections[0][0] + 0.4, -sections[0][1]*1.12, -sections[0][2]*0.55),
            (sections[mid][0], -sections[mid][1]*1.38, -sections[mid][2]*0.18),
            (sections[-1][0] - 0.4, -sections[-1][1]*1.08, -sections[-1][2]*0.45),
            (sections[0][0] + 0.8, sections[0][1]*1.05, sections[0][2]*0.58),
            (sections[mid][0] - 0.3, sections[mid][1]*1.32, sections[mid][2]*0.22),
            (sections[-1][0] - 0.8, sections[-1][1]*1.02, sections[-1][2]*0.5),
        ]
        faces += [(cheek, cheek+1, cheek+2), (cheek+3, cheek+5, cheek+4)]
    write_obj(name, v, faces)


def torn_wing(name: str, sx: float, sy: float, sz: float) -> None:
    x, y, z = sx/2, sy/2, sz/2
    v = [(-x,-y,-z*0.35), (-x*0.1,-y,-z), (x,-y,-z*0.45), (x*0.65,-y,z*0.75), (-x*0.8,-y,z),
         (-x,y,-z*0.22), (-x*0.05,y*0.7,-z*0.82), (x,y*0.55,-z*0.25), (x*0.45,y,z*0.52), (-x*0.88,y,z*0.78),
         (x*0.18, y*1.8, z*0.2)]
    faces = [(1,2,3,4,5), (6,10,9,8,7), (1,6,7,2), (2,7,8,3), (3,8,11,4), (4,11,9,5), (5,10,6,1)]
    write_obj(name, v, faces)

def main() -> None:
    OUT.mkdir(parents=True, exist_ok=True)
    fuselage("TC_HullMain", [(-4.8, 1.35, 2.0, 0.0), (-2.3, 1.7, 2.35, 0.15), (0.8, 1.45, 2.05, -0.1), (4.6, 1.05, 1.45, -0.25)])
    fuselage("TC_NoseCrushed", [(-2.0, 1.0, 1.45, -0.15), (-0.3, 0.85, 1.1, -0.25), (1.6, 0.38, 0.55, -0.35)])
    torn_wing("TC_WingSheared", 7.2, 0.72, 2.6)
    cylinder("TC_EngineExposed", 1.35, 1.0, 2.8, segments=10, dent=0.32)
    ribs()
    box("TC_BreachDebris", 1.6, 0.55, 1.1, skew=0.25, bite=True)
    rock("TC_BasaltForeground", 5.2, 1.7, 3.5, ridge=True)
    rock("TC_BasaltMidgroundA", 4.0, 2.8, 3.2, ridge=True)
    rock("TC_BasaltMidgroundB", 3.6, 2.2, 4.1, ridge=True)
    rock("TC_RidgeA", 9.5, 4.2, 4.0, ridge=True)
    rock("TC_RidgeB", 8.0, 4.8, 4.4, ridge=True)
    rock("TC_RidgeC", 5.0, 6.5, 3.2, ridge=True)
    ground("TC_FracturedGround", 14.0, 8.5, broken=True)
    ground("TC_AshPatch", 5.5, 3.8, broken=False)


if __name__ == "__main__":
    main()
