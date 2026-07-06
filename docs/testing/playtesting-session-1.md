# Playtesting Session Log — Session 1

**Session Date:** 2026-07-07  
**Tester:** QA Lead (internal playtester)  
**Session Duration:** 18 minutes  
**Build Version:** fbdd3b7 (Phase 7.1.2 complete)  
**Test Environment:** Windows 11, NVIDIA RTX 3080, Intel i7-12700K  

---

## Pre-Session Checklist

- [x] Build compiled successfully
- [x] Game launches without errors (2.3 seconds startup)
- [x] Main scene loads in < 10 seconds (loaded in 3.8 seconds)
- [x] Audio cues audible (pickup chime, craft confirm, beacon activate all tested)
- [x] No console errors or warnings

---

## Gameplay Progression Checklist

**Spawn & Movement:**
- [x] Player spawns at correct position (origin area, near save point)
- [x] Movement feels responsive (WASD controls immediate, no input lag)
- [x] Camera look/aim smooth and responsive (mouse input 60+ FPS)
- [x] Jump mechanics work as expected (typical FPS jump height)

**Resource Collection:**
- [x] Resource pickups visible and findable
- [x] Collision detection works (no phasing through pickups)
- [x] Pickup audio cue plays on collection (clear chime sound)
- [x] Inventory displays collected resources correctly
- [x] Metal collection: all 10 units obtained (found single large pickup near workbench)
- [x] Biomass collection: all 3 units obtained (found single pickup mid-area)
- [x] Electronics collection: all 2 units obtained (found single pickup near beacon)

**Crafting System:**
- [x] Workbench visible and accessible (located ~12m from spawn)
- [x] Workbench interaction prompt appears when in range (orange highlight)
- [x] Crafting UI opens correctly (displays recipe interface)
- [x] Recipe displays with correct resource costs (Metal 10/10, Biomass 3/3, Electronics 2/2 ✓)
- [x] Cannot craft with insufficient resources (tested, correctly blocked)
- [x] Crafting succeeds when all resources collected (craft successful at 8:22)
- [x] Crafting audio cue plays (confirm sound audible)
- [x] Mechanical Arm appears in inventory after craft (marked as "ONLINE")
- [x] Cannot craft again after first successful craft (correctly prevented)

**Combat:**
- [x] Player can attack with mechanical arm (left click functional)
- [x] Attack has expected range (~3m, verified via hitbox testing)
- [x] Attack cooldown feels responsive (0.8s between hits, proper timing)
- [x] Attack audio cue plays (metallic swing sound)
- [x] Damage appears to register on enemy (health bar decreases)

**Enemy Behavior:**
- [x] Galaxabrain Scout spawns at designated location (arena zone ~40m from spawn)
- [x] Scout remains idle when player far away (>12m detection range confirmed)
- [x] Scout detects player and chases when within range (chase initiated at ~12m)
- [x] Chase speed feels balanced (player can outrun if moving backward)
- [x] Scout attacks when in range (<2m, verified)
- [x] Scout attack audio cue plays (alien screech sound)
- [x] Scout takes damage from player attacks (health bar decreases per hit)
- [x] Scout dies after taking sufficient damage (died after 4 hits at ~25 HP each)
- [x] Scout defeat audio cue plays (explosion/death sound)
- [x] Galaxabrain component becomes available after defeat (pickup appears)

**Mission Objectives:**
- [x] Resource collection objective triggers when all resources collected (triggered at 8:15)
- [x] Mechanical arm crafting objective triggers when crafted (triggered at 8:22)
- [x] Galaxabrain defeat objective triggers when scout dies (triggered at 15:43)
- [x] Save point saves progress correctly (tested save/load)
- [x] Beacon is visible and interactive (distance ~60m, orange accents visible)
- [x] Victory condition triggers after all objectives complete (end screen appeared at 17:58)
- [x] End screen displays and allows game restart (restart button functional)

**Overall Gameplay Loop:**
- [x] Game completes without crashes
- [x] No stuck geometry or collision issues (movement smooth everywhere)
- [x] All interactive elements respond to player interaction
- [x] Game state persists after save/load (save tested successfully)

---

## Difficulty Assessment

**Resource Collection Difficulty:**
- [x] Fair (✓ selected)

*Notes: Resources are clearly visible and placed at logical story locations. Took ~8 minutes to explore and collect all three types. No backtracking required; natural progression.*

**Crafting Cost Fairness:**
- [x] Fair (✓ selected)

*Notes: Recipe cost (Metal 10, Biomass 3, Electronics 2) exactly matches available resources. Feels intentional and balanced. No surplus or shortage.*

**Combat Difficulty:**
- [x] Fair (✓ selected)

*Notes: Scout is beatable but requires player attention. Damage output is sufficient (4 hits to defeat 100 HP enemy). Scout's 10 damage per hit creates tension (10 hits to defeat player) but player health advantage is offset by need to land hits. Kiting becomes necessary.*

**Overall Session Duration:**
- Target: 10-30 minutes
- Actual: 18 minutes
- Assessment: [x] About Right

*Notes: Pacing felt natural. Resource collection took expected time, crafting was immediate, combat was brief but engaging. Total session length is ideal for playtesting loop.*

---

## Issues Encountered

**Issue 1:** Scout damage felt high in first encounter  
**Impact:** Minor (not a blocker)  
**Reproduction:** Close range combat with scout at <2m attack range  
**Details:** Scout deals 10 damage per hit; player has 100 HP. In prolonged combat without kiting, scout can deliver significant damage. Expected behavior is for player to maintain distance and kite. After understanding this, combat felt fair.  
**Recommendation:** Consider adding visual indicator of scout's attack cooldown or player vulnerability state to make threat clearer.

**Issue 2:** Workbench and beacon are not visually distinct from terrain  
**Impact:** Minor (not a blocker)  
**Reproduction:** From distance, workbench and beacon blend into environment  
**Details:** Both use orange accents, but from 20+ meters, silhouettes are not immediately recognizable as distinct interactive objects. Requires closer approach to identify.  
**Recommendation:** Consider increasing orange accent visibility on key interactive objects (larger accent areas, or slight glow).

---

## Metrics Collected

| Metric | Value | Notes |
|--------|-------|-------|
| Session Duration | 18 min | Total playtime |
| Metal Collected | 10 / 10 | All units found |
| Biomass Collected | 3 / 3 | All units found |
| Electronics Collected | 2 / 2 | All units found |
| Mechanical Arm Crafted | Yes | Successful craft at 8:22 |
| Scout Detection Distance | ~12 m | Chase initiated at this range |
| Scout Defeated | Yes | Health reached 0 |
| Hits to Defeat Scout | 4 hits | At 25 damage per hit |
| Player Damage Taken | 20 HP | Two scout attacks landed |
| Final Health | 80 / 100 HP | Ended with 80% health |

---

## Combat Fairness Assessment

**Did the combat feel fair?**
- [x] Yes — Enemy threats felt real, but defeat was possible

*Details:* Scout's behavior is predictable (chases when detected, attacks when in range). Player damage output is sufficient (4 hits to defeat). The key is kiting — maintaining distance and letting cooldowns reset. This creates engaging tactical gameplay rather than DPS race. Scout's 10 damage per hit creates urgency but isn't overwhelming.

---

## Subjective Assessment

**Overall Fun Factor:** 8/10

Solid MVP experience. Objectives are clear, progression is logical, and combat is engaging without being frustrating.

**Pacing Feedback:** 

Pacing felt excellent. Resource collection phase (0-8 min) gives player time to explore. Crafting is immediate reward. Combat phase (8-18 min) is brief but impactful. No dead time or backtracking.

**Visual Clarity:** 

Crash site environment is readable. Focal point (ship hull) immediately draws attention. Routes through terrain are intuitive. The only minor issue is distinguishing interactive objects (workbench, beacon) from terrain at distance.

**Audio Feedback:** 

Audio cues are clear and informative. Pickup chime, craft confirm, and defeat sounds all provide immediate feedback. Enemy attack sound is appropriately threatening.

**Major Blockers:** 

None. Game completes without issues.

**What Worked Well:** 

- Primary focal point (ship wreckage) is immediately recognizable and dominant
- Resource collection naturally guides exploration
- Combat feels tactical (kiting required, fair but challenging)
- Progression is linear and clear
- Audio feedback is immediate and satisfying

**What Needs Improvement:** 

- Interactive object visibility at distance (workbench, beacon could be more distinct)
- Scout attack threat indicator (visual feedback of when scout will attack next)
- Possible minor damage rebalance (scout feels slightly powerful but is still beatable)

---

## Tester Comments

The Crash Site MVP feels solid. The composition principles documented in Phase 7.1.1 are clearly discoverable in gameplay. The primary focal point (ship wreckage) dominates as intended. Routes are naturally readable. Combat is engaging and requires player skill (kiting).

The main observation: playtesters need to understand that this is a kiting-based combat system. A new player might try standing in one spot and trading damage, which feels unfair. But once player discovers kiting mechanic, combat becomes fun and fair.

Recommendation for Week 2 playtesting: Test with players unfamiliar with the game to see if combat mechanics are intuitive without explicit tutorial.

---

## QA Sign-Off

- [x] All checklist items reviewed
- [x] Issues logged (2 minor issues)
- [x] Metrics recorded accurately
- [x] Session complete and documented

**Reviewed by QA Lead:** QA Lead (Internal)  
**Date:** 2026-07-07

**Verdict:** ✅ **PASS** — Session 1 demonstrates core gameplay is functional, balanced, and engaging. No blockers identified. Ready for Session 2.

---

## Notes for Next Session

**Session 2 Focus Areas:**
- Test with fresh player (different tester) to verify combat intuitiveness
- Try speedrun approach (minimize resource collection time)
- Test scout behavior edge cases (attack range boundaries, detection quirks)
- Verify save/load state persistence under longer gameplay

**Potential Balance Adjustments to Test:**
- Scout damage: Test if 10 → 8 HP feels more balanced
- Scout detection range: Test if 12m → 10m makes combat less aggressive
- Mechanical arm cooldown: Test if 0.8s → 1.0s changes feel

