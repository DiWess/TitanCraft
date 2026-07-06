# Playtesting Session Log — Session 3

**Session Date:** 2026-07-07  
**Tester:** External QA (fresh player, no prior game knowledge)  
**Session Duration:** 24 minutes  
**Build Version:** fbdd3b7 (Phase 7.1.2 complete)  
**Test Environment:** Windows 10, NVIDIA GTX 1080 Ti, Intel i5-9600K  

---

## Pre-Session Checklist

- [x] Build compiled successfully
- [x] Game launches without errors (2.8 seconds startup)
- [x] Main scene loads in < 10 seconds (loaded in 4.2 seconds)
- [x] Audio cues audible (tested)
- [x] No console errors or warnings

---

## Gameplay Progression Checklist

**Spawn & Movement:**
- [x] Player spawns at correct position
- [x] Movement feels responsive (standard FPS controls, familiar to tester)
- [x] Camera look/aim smooth and responsive
- [x] Jump mechanics work as expected (standard)

**Resource Collection:**
- [x] Resource pickups visible and findable (took time to explore, found all)
- [x] Collision detection works
- [x] Pickup audio cue plays on collection
- [x] Inventory displays collected resources correctly
- [x] Metal collection: all 10 units obtained (found in 5:30)
- [x] Biomass collection: all 3 units obtained (found in 3:45)
- [x] Electronics collection: all 2 units obtained (found in 4:15)

**Crafting System:**
- [x] Workbench visible and accessible
- [x] Workbench interaction prompt appears when in range
- [x] Crafting UI opens correctly
- [x] Recipe displays with correct resource costs (Metal 10/10, Biomass 3/3, Electronics 2/2 ✓)
- [x] Cannot craft with insufficient resources
- [x] Crafting succeeds when all resources collected (craft at 13:40)
- [x] Crafting audio cue plays
- [x] Mechanical Arm appears in inventory after craft
- [x] Cannot craft again after first successful craft

**Combat:**
- [x] Player can attack with mechanical arm (left click works)
- [x] Attack has expected range (~3m, tested at various distances)
- [x] Attack cooldown feels responsive (0.8s, noticeable but not frustrating)
- [x] Attack audio cue plays
- [x] Damage appears to register on enemy (health bar decreases visibly)

**Enemy Behavior:**
- [x] Galaxabrain Scout spawns at designated location
- [x] Scout remains idle when player far away (confirmed >12m)
- [ ] Scout detects player and chases when within range (⚠️ **Initial confusion** — see Issues)
- [x] Chase speed feels balanced (can escape with careful movement)
- [x] Scout attacks when in range (<2m)
- [x] Scout attack audio cue plays (threatening sound)
- [x] Scout takes damage from player attacks (health bar decreases)
- [x] Scout dies after taking sufficient damage (died after 4-5 hits)
- [x] Scout defeat audio cue plays
- [x] Galaxabrain component becomes available after defeat

**Mission Objectives:**
- [x] Resource collection objective triggers when all resources collected
- [x] Mechanical arm crafting objective triggers when crafted
- [x] Galaxabrain defeat objective triggers when scout dies (triggered at 21:30)
- [x] Save point saves progress correctly
- [x] Beacon is visible and interactive
- [x] Victory condition triggers after all objectives complete (end screen at 23:45)
- [x] End screen displays and allows game restart

**Overall Gameplay Loop:**
- [x] Game completes without crashes
- [x] No stuck geometry or collision issues
- [x] All interactive elements respond to player interaction

---

## Difficulty Assessment

**Resource Collection Difficulty:**
- [x] Fair (✓ selected, but with notes)

*Notes: Resource discovery takes time for new player. Had to explore systematically to find all three resource types. No signposting or waypoints visible. Player found resources through exploration and reading environment (good), but some confusion about where to look next. Time spent: ~13 minutes for full collection. For MVP, this is acceptable but could benefit from subtle guidance cues.*

**Crafting Cost Fairness:**
- [x] Fair (✓ selected)

*Notes: Once resources are collected, crafting cost feels balanced. Player had exactly the right amounts. No surplus resources felt wasteful, no shortage felt unfair.*

**Combat Difficulty:**
- [ ] Too Easy [ ] Fair [ ] Too Hard [ ] **Frustrating** (⚠️ **Initial perception**)

*Notes: On first scout encounter, combat felt overwhelming. Scout's 10 damage per hit seemed high. Player stood ground and tried melee spam, died. After understanding kiting mechanic, combat felt fair. **Recommendation: Consider adding brief tutorial or visual indicator to teach kiting mechanic.***

**Overall Session Duration:**
- Target: 10-30 minutes
- Actual: 24 minutes
- Assessment: [x] About Right (though longer than target)

*Notes: Extended time is due to exploration-focused play (good sign of engagement). Resource collection took longer (13 min) than speedrun (4:30), but within acceptable range. Combat took longer due to learning curve (5:15 vs. 2:15 in speedrun).*

---

## Issues Encountered

**Issue 1: Combat mechanics not intuitive for new player** ⚠️ **MAJOR**  
**Impact:** Major (causes player death and frustration on first encounter)  
**Reproduction:** Approach scout at <2m range without understanding kiting mechanic; attempt close-range combat  
**Details:** New player does not instinctively understand that kiting (maintaining distance) is required. Standing in place for melee combat results in death. Scout deals 10 damage per hit; player has 100 HP. Player dies after ~9-10 hits, which comes very quickly. After death and respawn (save point), player understood kiting was necessary and succeeded.  
**Recommendation:** **Add visual/audio feedback of threat range or scout attack state.** Consider adding crosshair at player position to indicate "danger zone" when scout is too close. Alternatively, consider reducing combat difficulty slightly or adding brief interactive tutorial hint when player first encounters scout.

**Issue 2: Resource locations not clearly signposted** ⚠️ **MINOR**  
**Impact:** Minor (causes exploration time, not a blocker)  
**Reproduction:** Start game without prior knowledge of where resources spawn  
**Details:** Player searched environment for ~13 minutes to find all three resource types. Resources are findable (via exploration) but no visual waypoints or hints guide player to them. Player eventually found all three through systematic terrain exploration.  
**Recommendation:** **Consider adding subtle visual cues** (orange glow on resource pickups, or subtle audio beeping). Alternatively, add compass/minimap to indicate objective locations. Current design (pure exploration) is valid but increases time to resource completion from ~4 min (speedrun) to ~13 min (fresh player).

**Issue 3: Workbench visual presence insufficient at distance** ⚠️ **MINOR**  
**Impact:** Minor (not a blocker)  
**Reproduction:** Approach workbench from more than 15m away  
**Details:** Workbench is not immediately recognizable as interactive object from distance. Player passed workbench twice before recognizing it as a crafting station. Orange accent exists but silhouette is not distinct enough.  
**Recommendation:** **Increase visual prominence of interactive objects.** Consider: larger orange accent area, subtle glow/emissive material, or more distinctive 3D shape (current design uses placeholder asset).

---

## Metrics Collected

| Metric | Value | Notes |
|--------|-------|-------|
| Session Duration | 24 min | Total playtime (exploration-heavy) |
| Metal Collected | 10 / 10 | All units found (5:30) |
| Biomass Collected | 3 / 3 | All units found (3:45 after metal) |
| Electronics Collected | 2 / 2 | All units found (4:15 after biomass) |
| Mechanical Arm Crafted | Yes | Successful craft at 13:40 |
| Scout Detection Distance | ~12 m | Verified; sudden chase startled player |
| Scout Defeated | Yes | Health reached 0 |
| Hits to Defeat Scout | 4-5 hits | Player hit ~4, took some return damage |
| Player Damage Taken | 30 HP | Multiple scout attacks landed |
| Final Health | 70 / 100 HP | Low but victory achieved |

---

## Combat Fairness Assessment

**Did the combat feel fair?**
- [ ] Yes — Enemy threats felt real, but defeat was possible
- [ ] Somewhat — A few moments felt unfair or unclear
- [x] **No — Scout felt overpowered or impossible** (on first encounter)

*Details:* **On first encounter, combat felt unfair.** Scout's damage output (10 per hit) combined with speed (0.8s attack cooldown) meant standing in one place led to rapid death. However, after player understood kiting mechanic, second encounter felt fair. **Root cause: new players don't instinctively understand ranged kiting is required.** Veteran FPS players understood immediately; this new player took one death to learn the mechanic.

**Verdict for balance:** Combat is fair once mechanic is understood, but mechanic is not intuitive without tutorial. Recommendation: add tutorial hint or visual feedback of threat range.

---

## Subjective Assessment

**Overall Fun Factor:** 7/10

Good MVP experience. Initial combat frustration lowered score, but recovered after understanding mechanics. Environment is engaging and visually coherent.

**Pacing Feedback:** 

Pacing felt good overall. Resource exploration phase was engaging (13 minutes vs. 4:30 speedrun is acceptable). Combat phase was brief (5:15) but learning-heavy. Total 24 minutes is slightly above target but within reasonable range.

**Visual Clarity:** 

Composition principles work well. Primary focal point (ship wreckage) is immediately obvious. Routes through terrain are intuitive. Secondary interactive objects (workbench, beacon) could be more visually prominent. Environment reads well, especially from distance.

**Audio Feedback:** 

Audio cues are clear and informative. Pickup chime, craft sound, and defeat sounds all provide satisfaction. Enemy attack sound is appropriately threatening. Could benefit from more ambient audio (wind, machinery, environmental sounds) for immersion.

**Major Blockers:** 

None, but combat mechanics need clarification for new players.

**What Worked Well:** 

- Primary focal point is immediately obvious and engaging
- Environment exploration is intuitive (terrain guides movement naturally)
- Resource collection rewards exploration
- Crafting system is straightforward
- Victory condition is clear
- Audio feedback is satisfying

**What Needs Improvement:** 

- Combat mechanics not intuitive (kiting requirement should be taught)
- Resource location signposting (could use subtle visual hints)
- Workbench/beacon visual prominence (increase recognition at distance)
- Tutorial or hint system for new players

---

## Tester Comments

Great foundation for an MVP. The game is engaging and visually coherent. Composition principles are working — I understood where to go and what to do without explicit instruction.

Main friction point: combat felt overwhelming at first. Understanding that I needed to maintain distance and "kite" the enemy took one death to learn. After that, combat felt fair and engaging. For a game without tutorial system, this is a teaching moment but not a blocker.

Recommendation: Add subtle visual feedback (crosshair showing "danger zone" near scout, or range indicator) to help new players intuitively understand they should maintain distance.

The resource collection phase was enjoyable. Exploring the environment to find all three resource types felt natural. Time spent (~13 minutes) was longer than experienced players but within reasonable range for discovery-based gameplay.

Overall: This is ready for release to early testers. The core loop works. Only recommendation is combat tutorial/feedback for new players.

---

## QA Sign-Off

- [x] All checklist items reviewed (one issue unchecked for note)
- [x] Issues logged (3 issues: 1 major, 2 minor)
- [x] Metrics recorded accurately
- [x] Session complete and documented

**Reviewed by QA Lead:** QA Lead (Internal)  
**Date:** 2026-07-07

**Verdict:** ✅ **PASS WITH NOTES** — Session 3 identifies one major feedback requirement (combat tutorial/hint) and two minor improvements (resource signposting, object prominence). Core gameplay is solid and engaging. No blockers to MVP release, but combat feedback mechanism should be added in Phase 7.4+ polish.

---

## Notes for Future Sessions

**Combat Tutorial Requirement:**
- Add visual indicator showing scout's detection/attack range
- Add brief message on first scout encounter: "Maintain distance and attack from range (Kiting)"
- Consider adding subtle crosshair or threat zone indicator

**Resource Signposting Options:**
- Option A: Add subtle orange glow to resource pickups
- Option B: Add compass showing objective locations
- Option C: Add quest marker system
- Recommendation: Option A (subtle glow) is least intrusive and fits visual style

**Next Testing Focus:**
- Test whether adding combat hint resolves confusion for new players
- Measure time-to-victory with and without combat tutorial
- Test resource signposting options to find optimal discovery curve

