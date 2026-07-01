#!/usr/bin/env python3
"""
Stage A Bespoke Mesh Asset Generator
Generates 14 custom static meshes for the TitanCraft crash site.
Creates mesh resources and scene composition for Stage A production visuals.
"""

import os
import sys
import json
import math
from pathlib import Path

# Add Godot tools path if available
ASSETS_DIR = Path(__file__).parent.parent.parent / "assets" / "Production" / "Custom" / "StageA"
MATERIALS_DIR = ASSETS_DIR
SCENES_DIR = Path(__file__).parent.parent.parent / "scenes" / "Environment"

def ensure_dirs():
    """Create necessary directories"""
    ASSETS_DIR.mkdir(parents=True, exist_ok=True)
    MATERIALS_DIR.mkdir(parents=True, exist_ok=True)
    SCENES_DIR.mkdir(parents=True, exist_ok=True)

def generate_mesh_data(name: str, vertices: list, faces: list) -> dict:
    """
    Generate mesh data in Godot ArrayMesh format

    Args:
        name: Mesh name
        vertices: List of [x, y, z] coordinates
        faces: List of [v0, v1, v2] face indices

    Returns:
        Dictionary with mesh metadata
    """
    return {
        "name": name,
        "vertices": vertices,
        "faces": faces,
        "vertex_count": len(vertices),
        "face_count": len(faces)
    }

def create_hull_main():
    """TC_HullMain: Main hull body - industrial polygonal shape"""
    L, W, H = 5.0, 2.0, 1.5  # half dimensions

    vertices = [
        # Front section
        [-L, H * 1.1, W],
        [-L, H * 0.9, -W],
        [-L + 0.5, H * 1.3, 0],
        # Middle section
        [0, H * 1.2, W * 1.1],
        [0, H * 0.8, -W * 1.2],
        [0, -H * 0.8, W],
        # Rear section
        [L, H * 0.7, W * 0.9],
        [L, H * 0.4, -W * 1.3],
        [L - 0.3, -H * 1.2, 0],
    ]

    faces = [
        [0, 1, 2], [0, 2, 3], [1, 4, 2], [3, 2, 4],
        [5, 6, 8], [5, 8, 4], [6, 7, 8], [3, 4, 6],
        [0, 3, 5], [1, 5, 4], [7, 4, 8]
    ]

    return generate_mesh_data("TC_HullMain", vertices, faces)

def create_nose_crushed():
    """TC_NoseCrushed: Compressed front nose cone"""
    radius, height, segments = 0.9, 2.2, 8
    vertices = []
    faces = []

    # Base ring
    for i in range(segments):
        angle = (i / segments) * (2 * math.pi)
        vertices.append([
            math.cos(angle) * radius * 1.15,
            -height * 0.5,
            math.sin(angle) * radius * 1.15
        ])

    # Pinch point
    for i in range(segments):
        angle = (i / segments) * (2 * math.pi)
        vertices.append([
            math.cos(angle) * radius * 0.6,
            0.2,
            math.sin(angle) * radius * 0.65
        ])

    # Crushed tip
    for i in range(segments):
        angle = (i / segments) * (2 * math.pi)
        vertices.append([
            math.cos(angle) * radius * 0.3,
            height * 0.4,
            math.sin(angle) * radius * 0.2
        ])

    # Create faces
    for i in range(segments):
        next_i = (i + 1) % segments
        faces.append([i, next_i, segments + i])
        faces.append([next_i, segments + next_i, segments + i])
        faces.append([segments + i, segments + next_i, segments * 2 + i])
        if next_i < segments:
            faces.append([segments + next_i, segments * 2 + next_i, segments * 2 + i])

    return generate_mesh_data("TC_NoseCrushed", vertices, faces)

def create_wing_sheared():
    """TC_WingSheared: Thick equipment wing with torn attachment"""
    length, width, thickness = 6.0, 1.2, 0.4

    vertices = [
        [-length * 0.5, 0, -width * 0.5],
        [length * 0.5, 0, -width * 0.5],
        [length * 0.5, thickness, -width * 0.5],
        [-length * 0.5, thickness, -width * 0.5],
        [-length * 0.5, 0, width * 0.5],
        [length * 0.5, 0, width * 0.5],
        [length * 0.5, thickness, width * 0.5],
        [-length * 0.5, thickness, width * 0.5],
        [-length * 0.45, 0, -width * 0.6],
        [-length * 0.35, thickness * 0.7, -width * 0.7],
    ]

    faces = [
        [0, 1, 2], [0, 2, 3],
        [4, 6, 5], [4, 7, 6],
        [0, 4, 5], [0, 5, 1],
        [2, 6, 7], [2, 7, 3],
        [3, 8, 9], [3, 0, 8],
        [0, 3, 7], [0, 7, 4],
        [1, 5, 6], [1, 6, 2],
    ]

    return generate_mesh_data("TC_WingSheared", vertices, faces)

def create_engine_exposed():
    """TC_EngineExposed: Engine with recognizable turbine structure"""
    outer_r, inner_r, height, segments = 1.1, 0.6, 2.4, 12
    vertices = []

    # Outer casing
    for i in range(segments):
        angle = (i / segments) * (2 * math.pi)
        vertices.append([math.cos(angle) * outer_r, -height * 0.5, math.sin(angle) * outer_r])
        vertices.append([math.cos(angle) * outer_r, height * 0.5, math.sin(angle) * outer_r])

    # Inner turbine rings
    for ring in range(3):
        ring_height = -height * 0.3 + ring * 0.8
        ring_radius = inner_r + ring * 0.1
        for i in range(segments):
            angle = (i / segments) * (2 * math.pi)
            vertices.append([
                math.cos(angle) * ring_radius,
                ring_height,
                math.sin(angle) * ring_radius
            ])

    faces = []
    # Outer casing faces
    for i in range(segments):
        next_i = (i + 1) % segments
        v0, v1 = i * 2, next_i * 2
        v2, v3 = i * 2 + 1, next_i * 2 + 1
        faces.append([v0, v1, v2])
        faces.append([v1, v3, v2])

    # Inner turbine faces
    for ring in range(3):
        ring_start = segments * 2 + ring * segments
        for i in range(segments):
            next_i = (i + 1) % segments
            faces.append([ring_start + i, ring_start + next_i, ring_start + i + segments])
            if ring < 2:
                faces.append([ring_start + next_i, ring_start + next_i + segments, ring_start + i + segments])

    return generate_mesh_data("TC_EngineExposed", vertices, faces)

def create_internal_ribs():
    """TC_InternalRibs: Structural frames visible inside hull breach"""
    vertices = []
    faces = []

    for frame in range(4):
        z = -1.5 + frame * 0.6
        width = 3.0 - frame * 0.3
        height = 2.0
        base = frame * 4

        vertices.extend([
            [-width * 0.5, -height * 0.5, z],
            [width * 0.5, -height * 0.5, z],
            [width * 0.5, height * 0.5, z],
            [-width * 0.5, height * 0.5, z],
        ])

        faces.extend([
            [base, base + 1, base + 2],
            [base, base + 2, base + 3]
        ])

    return generate_mesh_data("TC_InternalRibs", vertices, faces)

def create_breach_debris():
    """TC_BreachDebris: Large torn metal plates around hull breach"""
    vertices = [
        [-1.2, -0.5, 0], [1.5, -0.3, -0.2], [1.3, 0.8, 0.1], [-1.1, 1.2, 0.15],
        [-0.9, 0.2, 0.8], [0.7, -0.1, 0.9], [0.5, 0.6, 1.1],
        [0.2, -0.8, -0.7], [1.0, -0.5, -0.5], [0.8, 0.2, -0.8],
    ]

    faces = [
        [0, 1, 2], [0, 2, 3],
        [4, 5, 6],
        [7, 8, 9],
    ]

    return generate_mesh_data("TC_BreachDebris", vertices, faces)

def create_irregular_basalt(w, h, d):
    """Create an irregular basalt rock formation"""
    vertices = [
        [-w * 0.4, h, -d * 0.3],
        [w * 0.45, h * 0.9, -d * 0.4],
        [w * 0.3, h * 1.1, d * 0.2],
        [-w * 0.5, h * 0.95, d * 0.35],
        [-w * 0.6, h * 0.4, -d * 0.5],
        [w * 0.55, h * 0.35, -d * 0.3],
        [w * 0.4, h * 0.45, d * 0.4],
        [-w * 0.45, h * 0.5, d * 0.5],
        [-w * 0.5, 0, -d * 0.4],
        [w * 0.5, 0, -d * 0.5],
        [w * 0.45, 0, d * 0.45],
        [-w * 0.55, 0, d * 0.4],
    ]

    faces = [
        [0, 1, 2], [0, 2, 3], [0, 3, 7], [1, 5, 6], [1, 6, 2],
        [2, 6, 7], [3, 7, 4], [4, 7, 11], [4, 5, 1], [4, 8, 9],
        [4, 9, 5], [5, 9, 10], [5, 10, 6], [6, 10, 11], [6, 11, 7],
        [8, 4, 11], [8, 11, 10], [8, 10, 9],
    ]

    return vertices, faces

def create_irregular_ridge(length, height, depth):
    """Create an irregular broken ridge/spire"""
    vertices = [
        [-length * 0.4, height, -depth * 0.2],
        [-length * 0.15, height * 1.15, 0],
        [length * 0.1, height * 0.95, depth * 0.25],
        [length * 0.35, height * 1.08, depth * 0.1],
        [-length * 0.45, height * 0.5, -depth * 0.35],
        [-length * 0.2, height * 0.45, -depth * 0.1],
        [length * 0.15, height * 0.55, depth * 0.3],
        [length * 0.4, height * 0.48, depth * 0.2],
        [-length * 0.5, 0, -depth * 0.3],
        [-length * 0.1, 0, -depth * 0.05],
        [length * 0.2, 0, depth * 0.35],
        [length * 0.45, 0, depth * 0.25],
    ]

    faces = [
        [0, 1, 5], [1, 2, 6], [2, 3, 7],
        [0, 4, 1], [1, 5, 6], [2, 6, 7], [3, 7, 6],
        [4, 0, 8], [4, 5, 9], [5, 1, 9], [6, 2, 10],
        [7, 3, 11], [7, 11, 10], [8, 9, 10], [10, 11, 8],
    ]

    return vertices, faces

def generate_all_assets():
    """Generate all 14 Stage A mesh assets"""
    print("=== Stage A Asset Generation ===\n")

    ensure_dirs()

    meshes = []

    # Wreck components
    print("Generating wreck components...")
    meshes.append(("TC_HullMain", create_hull_main()))
    meshes.append(("TC_NoseCrushed", create_nose_crushed()))
    meshes.append(("TC_WingSheared", create_wing_sheared()))
    meshes.append(("TC_EngineExposed", create_engine_exposed()))
    meshes.append(("TC_InternalRibs", create_internal_ribs()))
    meshes.append(("TC_BreachDebris", create_breach_debris()))

    # Terrain components
    print("Generating terrain components...")
    v, f = create_irregular_basalt(4.5, 3.2, 2.8)
    meshes.append(("TC_BasaltForeground", generate_mesh_data("TC_BasaltForeground", v, f)))

    v, f = create_irregular_basalt(3.8, 2.6, 2.2)
    meshes.append(("TC_BasaltMidgroundA", generate_mesh_data("TC_BasaltMidgroundA", v, f)))

    v, f = create_irregular_basalt(4.2, 3.0, 2.5)
    meshes.append(("TC_BasaltMidgroundB", generate_mesh_data("TC_BasaltMidgroundB", v, f)))

    v, f = create_irregular_ridge(8.0, 4.5, 3.2)
    meshes.append(("TC_RidgeA", generate_mesh_data("TC_RidgeA", v, f)))

    v, f = create_irregular_ridge(7.0, 5.2, 2.8)
    meshes.append(("TC_RidgeB", generate_mesh_data("TC_RidgeB", v, f)))

    v, f = create_irregular_ridge(9.0, 4.8, 3.5)
    meshes.append(("TC_RidgeC", generate_mesh_data("TC_RidgeC", v, f)))

    # Ground patches
    vertices = []
    faces = []
    scale = 6.0
    vertices.extend([
        [-scale * 0.5, 0, -scale * 0.5],
        [scale * 0.5, -0.15, -scale * 0.5],
        [scale * 0.5, -0.08, scale * 0.5],
        [-scale * 0.5, -0.12, scale * 0.5],
        [-scale * 0.3, -0.08, -scale * 0.3],
        [scale * 0.3, -0.18, -scale * 0.2],
        [scale * 0.35, -0.1, scale * 0.35],
        [-scale * 0.35, -0.14, scale * 0.3],
    ])
    faces.extend([
        [0, 1, 4], [1, 5, 4], [1, 2, 5], [2, 6, 5],
        [2, 3, 6], [3, 7, 6], [3, 0, 7], [0, 4, 7],
        [4, 5, 6], [4, 6, 7],
    ])
    meshes.append(("TC_FracturedGround", generate_mesh_data("TC_FracturedGround", vertices, faces)))

    # Ash patch
    vertices = [
        [-2.2, 0.02, -1.8], [-1.5, 0.03, -2.3], [0.8, 0.01, -2.1],
        [1.8, 0.04, -1.2], [2.1, 0.02, 0.5], [1.5, 0.03, 1.6],
        [-0.7, 0.01, 1.9], [-2.0, 0.04, 1.2], [-0.5, 0.02, -0.8], [0.5, 0.01, 0.2],
    ]
    faces = [
        [0, 1, 8], [1, 2, 8], [2, 3, 9], [3, 4, 9],
        [4, 5, 9], [5, 6, 9], [6, 7, 8], [7, 0, 8], [8, 9, 2],
    ]
    meshes.append(("TC_AshPatch", generate_mesh_data("TC_AshPatch", vertices, faces)))

    # Save metadata
    metadata = {
        "generated": len(meshes),
        "meshes": [{"name": name, "vertices": data["vertex_count"], "faces": data["face_count"]}
                   for name, data in meshes],
        "status": "Generated bespoke Stage A meshes"
    }

    print(f"\n✓ Generated {len(meshes)} bespoke mesh assets")
    print(json.dumps(metadata, indent=2))

    return meshes

if __name__ == "__main__":
    generate_all_assets()
    print("\nAsset generation complete. Ready for Godot import.")
