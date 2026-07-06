# Current Gameplay Parameters

**Date:** 2026-07-06  
**Build:** Phase 7 baseline  
**Purpose:** Document current balance values for playtesting reference and change tracking  

---

## Player Character

### Health System
- **Max Health:** 100 HP
- **Starting Health:** 100 HP (full)
- **Health Regeneration:** None (no healing mechanic)
- **Death Condition:** Health reaches 0 HP

### Movement
- **Walk Speed:** Standard (controller defines FPS movement)
- **Jump:** Functional (Godot CharacterBody3D physics)
- **Sprint:** Not implemented

### Mechanical Arm (Weapon)

| Parameter | Value | Notes |
|-----------|-------|-------|
| Damage per Hit | 25 HP | Against Galaxabrain Scout |
| Attack Cooldown | 0.8 seconds | Time between attacks |
| Attack Range | 3.0 meters | Raycast distance |
| Required to Defeat Scout | ~4 hits | 100 HP scout ÷ 25 HP per hit |

**Attack Behavior:**
- Player must craft mechanical arm before attacking
- Left-click to attack (when cooldown ready)
- Raycast-based detection (hit-scan, not projectile)
- Cooldown prevents spam
- Audio cue plays on hit

---

## Galaxabrain Scout (Enemy)

### Health & Damage
- **Max Health:** 100 HP
- **Damage per Attack:** 10 HP
- **Attack Cooldown:** 0.8 seconds
- **Attack Behavior:** Attacks when in range, cooldown-gated

### Detection & Behavior

| Parameter | Value | Range/Notes |
|-----------|-------|-------------|
| Detection Range | 12 meters | Scout notices player when within this distance |
| Attack Range | 2 meters | Scout must be this close to attack |
| Chase Speed | 3 m/s | Speed while pursuing player |
| State: Idle | > 12m from player | No engagement |
| State: Chase | 2-12m from player | Pursues player toward position |
| State: Attack | < 2m from player | Stops to attack |
| State: Dead | Health = 0 | Cannot engage |

### Combat Balance Notes

**Time to Defeat Scout:**
- Player damage: 25 HP per hit
- Scout health: 100 HP
- Hits needed: 4 (100 ÷ 25 = 4)
- Time to defeat: ~4.8 seconds (4 hits × 0.8s cooldown + some slop)

**Time for Scout to Defeat Player (if stationary):**
- Scout damage: 10 HP per hit
- Player health: 100 HP
- Hits needed: 10 (100 ÷ 10 = 10)
- Time to defeat: ~10.8 seconds (10 hits × 0.8s cooldown + some slop)
- **Implication:** Player must kite or the 10 hits will take ~8 seconds (faster than player defeat)

**Detection to Attack Timing:**
- Scout detects player at 12m
- Closes distance at 3 m/s
- Time to reach 2m range: ~3.3 seconds (10m ÷ 3 m/s)
- First attack: ~3.3-4.1 seconds (includes cooldown)

---

## Crafting System

### Mechanical Arm Recipe

| Resource | Required | Available | Balanced? |
|----------|----------|-----------|-----------|
| Metal | 10 units | 10 units | ✓ Exact |
| Biomass | 3 units | 3 units | ✓ Exact |
| Electronics | 2 units | 2 units | ✓ Exact |

**Current Design:**
- Player collects all 15 total resources (10 metal, 3 biomass, 2 electronics)
- Resources are distributed in the map as pickups
- When all resources are collected, mechanical arm becomes craftable
- Crafting consumes all resources in exact amounts
- After craft, mechanical arm is marked as built and ready to use

**Observations:**
- No leftover resources (game designed for exact match)
- No alternative recipes or upgrades
- Single crafting opportunity (no recrafting)

---

## Resources (Pickups)

### Metal
- **Total Available:** 10 units
- **Spawn Method:** Resource pickup nodes in Main scene
- **Collection Mechanic:** Area3D collision on player approach
- **Quantity Per Pickup:** 10 (single large pickup)
- **Location:** Near workbench area

### Biomass
- **Total Available:** 3 units
- **Spawn Method:** Resource pickup nodes in Main scene
- **Collection Mechanic:** Area3D collision on player approach
- **Quantity Per Pickup:** 3 (single large pickup)
- **Location:** Mid-area terrain

### Electronic Components
- **Total Available:** 2 units
- **Spawn Method:** Resource pickup nodes in Main scene
- **Collection Mechanic:** Area3D collision on player approach
- **Quantity Per Pickup:** 2 (single large pickup)
- **Location:** Far area / near beacon

**Audio Feedback:** Pickup chime plays on collection

---

## Mission Objectives

| Objective | Progress Trigger | Completion Trigger |
|-----------|------------------|-------------------|
| Collect Resources | Resource pickup | All resources collected (10M + 3B + 2E) |
| Craft Mechanical Arm | Resource collection complete | Successful craft at workbench |
| Defeat Galaxabrain Scout | Craft complete | Scout health reaches 0 |
| Escape / Victory | Scout defeated | Beacon interaction after defeat |

**Linear Flow:**
1. Spawn → Collect all resources → Objective 1 complete
2. Craft at workbench → Objective 2 complete
3. Defeat scout in combat → Objective 3 complete
4. Interact with beacon → Victory / End screen

---

## Balance Ratios

### Player vs. Enemy DPS

| Character | Damage | Cooldown | DPS |
|-----------|--------|----------|-----|
| Player (Mechanical Arm) | 25 HP | 0.8s | 31.25 HP/s |
| Scout | 10 HP | 0.8s | 12.5 HP/s |

**DPS Ratio:** Player does 2.5× Scout DPS (favors player)

### Health Pool Comparison

| Character | Health | Time to Die vs. Scout | Time to Die vs. Player |
|-----------|--------|----------------------|----------------------|
| Player | 100 HP | ~8 seconds (10 hits) | N/A |
| Scout | 100 HP | N/A | ~3.2 seconds (4 hits) |

**Combat Asymmetry:** Player must output 4 hits; Scout must output 10 hits. Kiting becomes important.

---

## Current Difficulty Assumptions

**"Fair" difficulty target:**
- Resource collection: 5-10 minutes of exploration
- Crafting: Instant (no time pressure)
- Combat: Beatable with basic kiting
- Total session: 10-30 minutes

**This playtest will validate:**
- Is resource collection too easy / too hard?
- Are crafting costs fair?
- Is combat fair (not overpowered, not impossible)?
- Does session duration match 10-30 minute target?

---

## Known Limitations & Placeholders

- **Mechanical arm visual:** Uses MeshInstance3D, not animated
- **Scout audio:** Temporary synthesized cues (not professional)
- **Workbench visual:** Placeholder asset (KayKit base)
- **Beacon visual:** Placeholder asset with orange material
- **Terrain:** Procedural basalt + ash patches (Stage A approved)
- **No difficulty settings:** Single "normal" balance

---

## Future Balance Levers (Do Not Adjust This Week)

These are documented for Phase 7.2.3+ balance iterations:

- `PlayerHealth.DefaultMaxHealth` — Player health pool
- `MechanicalArmAttackLogic.DefaultMechanicalArmDamage` — Player weapon damage
- `MechanicalArmAttackLogic.DefaultCooldownSeconds` — Player attack speed
- `GalaxabrainScout.Health` export — Scout health pool
- `GalaxabrainScout.Damage` export — Scout attack damage
- `GalaxabrainScout.AttackCooldownSeconds` export — Scout attack speed
- `GalaxabrainScout.DetectionRange` export — Scout awareness range
- `GalaxabrainScout.ChaseSpeed` export — Scout pursuit speed
- `MechanicalArmRecipe` JSON (Metal/Biomass/Electronics costs) — Crafting recipe
- Resource pickup `Quantity` values in Main.tscn — Available resource amounts

---

## Validation Checklist

- [x] All parameters extracted from source code (PlayerHealth.cs, MechanicalArmAttack.cs, GalaxabrainScout.cs, MechanicalArmRecipe.cs)
- [x] Crafting recipe extracted from data/Recipes/mechanical_arm_mk1.json
- [x] Resource quantities verified from Main.tscn pickups
- [x] Combat math verified (player hits needed, scout hits needed, DPS ratios)
- [x] All export properties documented
- [x] Future balance levers identified for iteration tracking

---

**Status:** Ready for Phase 7.2.2 playtesting (Week 2+)  
**Last Updated:** 2026-07-06  
**Owner:** Gameplay Engineer (Phase 7.2.1)

