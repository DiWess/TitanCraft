# Playtesting Session Log — Template

**Session Date:** [date]  
**Tester:** [name]  
**Session Duration:** [minutes]  
**Build Version:** [git hash or build #]  
**Test Environment:** [Windows version, GPU, CPU if relevant]  

---

## Pre-Session Checklist

- [ ] Build compiled successfully
- [ ] Game launches without errors
- [ ] Main scene loads in < 10 seconds
- [ ] Audio cues audible
- [ ] No console errors or warnings

---

## Gameplay Progression Checklist

**Spawn & Movement:**
- [ ] Player spawns at correct position
- [ ] Movement feels responsive (WASD controls responsive)
- [ ] Camera look/aim smooth and responsive
- [ ] Jump mechanics work as expected

**Resource Collection:**
- [ ] Resource pickups visible and findable
- [ ] Collision detection works (player can approach pickups)
- [ ] Pickup audio cue plays on collection
- [ ] Inventory displays collected resources correctly
- [ ] Metal collection: all 10 units obtainable
- [ ] Biomass collection: all 3 units obtainable
- [ ] Electronics collection: all 2 units obtainable

**Crafting System:**
- [ ] Workbench visible and accessible
- [ ] Workbench interaction prompt appears when in range
- [ ] Crafting UI opens correctly
- [ ] Mechanical Arm recipe displays with correct resource costs (Metal 10, Biomass 3, Electronics 2)
- [ ] Cannot craft with insufficient resources
- [ ] Crafting succeeds when all resources collected
- [ ] Crafting audio cue plays
- [ ] Mechanical Arm appears in inventory after craft
- [ ] Cannot craft again after first successful craft

**Combat:**
- [ ] Player can attack with mechanical arm (left click)
- [ ] Attack has expected range (~3m)
- [ ] Attack cooldown feels responsive (0.8s)
- [ ] Attack audio cue plays
- [ ] Damage appears to register on enemy

**Enemy Behavior:**
- [ ] Galaxabrain Scout spawns at designated location
- [ ] Scout remains idle when player far away (>12m detection range)
- [ ] Scout detects player and chases when within range
- [ ] Chase speed feels balanced (not impossible to escape)
- [ ] Scout attacks when in range (<2m)
- [ ] Scout attack audio cue plays
- [ ] Scout takes damage from player attacks
- [ ] Scout dies after taking sufficient damage (~4 hits at 25 damage each)
- [ ] Scout defeat audio cue plays
- [ ] Galaxabrain component becomes available after defeat

**Mission Objectives:**
- [ ] Resource collection objective triggers when all resources collected
- [ ] Mechanical arm crafting objective triggers when crafted
- [ ] Galaxabrain defeat objective triggers when scout dies
- [ ] Save point saves progress correctly
- [ ] Beacon is visible and interactive
- [ ] Victory condition triggers after all objectives complete
- [ ] End screen displays and allows game restart

**Overall Gameplay Loop:**
- [ ] Game completes without crashes
- [ ] No stuck geometry or collision issues
- [ ] All interactive elements respond to player interaction
- [ ] Game state persists after save/load (if tested)

---

## Difficulty Assessment

**Resource Collection Difficulty:**
- [ ] Too Easy  [ ] Fair  [ ] Too Hard

*Notes: [How long did it take to find all resources? Were pickups clearly visible?]*

**Crafting Cost Fairness:**
- [ ] Too Cheap  [ ] Fair  [ ] Too Expensive

*Notes: [Did resource quantities feel balanced for the recipe cost?]*

**Combat Difficulty:**
- [ ] Too Easy  [ ] Fair  [ ] Too Hard  [ ] Frustrating

*Notes: [Could player consistently hit the enemy? Did enemy feel threatening but defeatable?]*

**Overall Session Duration:**
- Target: 10-30 minutes
- Actual: ____ minutes
- Assessment: [ ] Too Quick  [ ] About Right  [ ] Too Long

---

## Issues Encountered

[List any bugs, glitches, crashes, balance problems, or unclear mechanics]

**Issue 1:** [Description]  
**Impact:** [Blocker / Major / Minor]  
**Reproduction:** [Steps to reproduce]  

[Add more as needed]

---

## Metrics Collected

| Metric | Value | Notes |
|--------|-------|-------|
| Session Duration | ____ min | Total playtime |
| Metal Collected | ____ / 10 | How many metal resources found |
| Biomass Collected | ____ / 3 | How many biomass resources found |
| Electronics Collected | ____ / 2 | How many electronics found |
| Mechanical Arm Crafted | Yes / No | Was craft successful |
| Scout Detection Distance | ~____ m | When did scout start chasing? |
| Scout Defeated | Yes / No | Did player defeat scout |
| Hits to Defeat Scout | ____ hits | How many attacks needed (~4 expected) |
| Player Damage Taken | ____ HP | Total damage received from scout |
| Final Health | ____ / 100 HP | Health at end of session |

---

## Combat Fairness Assessment

**Did the combat feel fair?**
- [ ] Yes — Enemy threats felt real, but defeat was possible
- [ ] Somewhat — A few moments felt unfair or unclear
- [ ] No — Scout felt overpowered or impossible

*Details: [What felt fair or unfair about enemy behavior?]*

---

## Subjective Assessment

**Overall Fun Factor:** 1-10: ____

**Pacing Feedback:** [Did progression feel natural or rushed?]

**Visual Clarity:** [Could you clearly see where to go and what to do?]

**Audio Feedback:** [Did audio cues help you understand gameplay events?]

**Major Blockers:** [Did anything prevent you from completing the game?]

**What Worked Well:** [What felt polished or enjoyable?]

**What Needs Improvement:** [What felt incomplete or confusing?]

---

## Tester Comments

[Free-form notes from the playtester about the experience]

---

## QA Sign-Off

- [ ] All checklist items reviewed
- [ ] Issues logged
- [ ] Metrics recorded accurately
- [ ] Session complete and documented

**Reviewed by QA Lead:** ________________  
**Date:** __________________

---

## Notes for Next Session

[Any follow-ups, parameter changes, or test focus areas for next playtest]

