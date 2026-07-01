# Phase 3A mesh geometry audit

Calculated directly from OBJ vertices/faces.

| Candidate | Vertices | Faces | AABB min | AABB max | AABB size | Centre | Origin distance | Longest | Materials | Expected Godot scale | Orientation | Complete/component | Classification |
|---|---:|---:|---|---|---|---|---:|---:|---:|---|---|---|---|
| ship_hull_body.obj | 3153 | 3222 | (-3.668,-2.182,-3.094) | (3.668,3.522,1.864) | (7.337,5.703,4.958) | (0.000,0.670,-0.615) | 0.910 | 7.337 | 1 | world scale only, after collision review | upright spacecraft body | complete object | COMPLETE_MODEL; SAFE_FOR_WORLD_VISUAL |
| damaged_structural_base.obj | 1457 | 1313 | (-4.265,0.004,-4.265) | (4.265,4.975,4.265) | (8.530,4.971,8.530) | (0.000,2.490,0.000) | 2.490 | 8.530 | 1 | world scale only, likely 0.2-1.0 after derivative review | Y-up base | complete object | COMPLETE_MODEL; SAFE_FOR_WORLD_VISUAL |
| robot_body_component.obj | 1822 | 2010 | (-1.521,-0.020,-1.282) | (1.579,4.477,1.346) | (3.099,4.497,2.628) | (0.029,2.228,0.032) | 2.229 | 4.497 | 3 | enemy scale only after visual/collision contract | upright robot | complete object | COMPLETE_MODEL; REQUIRES_DOCUMENTED_DERIVATIVE for 1.4m capsule |
| george_mech_source_arm.obj | 4124 | 3816 | (-2.079,-0.033,-1.153) | (2.080,6.057,1.678) | (4.159,6.090,2.831) | (0.001,3.012,0.262) | 3.023 | 6.090 | 1 | not acceptable as-is under camera | upright full mech | complete object, not isolated arm | COMPLETE_MODEL; UNSUITABLE_FOR_ROLE; REQUIRES_DOCUMENTED_DERIVATIVE; not SAFE_FOR_FIRST_PERSON_VIEWMODEL |
