# TitanCraft MVP Project Management Hub

**Project Authority:** Producer + Claude Code  
**Last Updated:** 2026-07-05  
**Status:** 🟢 Phase 4–6 Complete | Phase 7 Planning

---

## 📋 Project Overview

TitanCraft is a solo, offline-first, Windows-first FPS built with Godot 4 .NET and C#. This document serves as the **master project manifest**, tracking all deliverables, phases, and task organization from A to Z.

**Current MVP Scope:** Crash Site (Phase 1–7)  
**Playable Slice:** Single-player arena with resource gathering → crafting → combat → beacon activation

---

## 🎯 Phase Roadmap

| Phase | Focus | Authority | Status | Deliverables |
|-------|-------|-----------|--------|--------------|
| **1–3** | Terrain, obstacles, visibility | Art Director | ✅ Complete | Terrain foundation, prop briefs |
| **4–6** | Interactables, combat, resources | Art Director → Gameplay | ✅ Complete | Workbench, Beacon, Save Point, Pickups, Arm, Scout |
| **7** | Composition & visual polish | Art Director | 🟡 Planning | Lighting guide, visual hierarchy |
| **8–9** | Advanced mechanics (post-MVP) | Art Director + Gameplay | ⏳ Pending | Enemy variants, progression systems |

---

## 📦 Deliverables Index (A→Z by Type)

### A. Art Assets (10 GLTF Models) ✅

**Status:** All delivered and integrated

| ID | Asset Name | Model File | Brief | Status |
|----|------------|-----------|-------|--------|
| **A1** | Beacon (Active) | TC_PROP_Beacon_Active_V1.gltf | brief-beacon-v1.md | ✅ Integrated |
| **A2** | Beacon (Dormant) | TC_PROP_Beacon_Dormant_V1.gltf | brief-beacon-v1.md | ✅ Integrated |
| **A3** | Component Pickup | TC_PICKUP_Component_V1.gltf | brief-pickups-v1.md | ✅ Integrated |
| **A4** | Galaxabrain Scout | TC_CHAR_GalaxabrainScout_V1.gltf | brief-scout-enemy-v1.md | ✅ Integrated |
| **A5** | Mechanical Arm | TC_PLAYER_MechanicalArm_V1.gltf | brief-mechanical-arm-v1.md | ✅ Integrated |
| **A6** | Metal Pickup | TC_PICKUP_Metal_V1.gltf | brief-pickups-v1.md | ✅ Integrated |
| **A7** | Biomass Pickup | TC_PICKUP_Biomass_V1.gltf | brief-pickups-v1.md | ✅ Integrated |
| **A8** | Electronics Pickup | TC_PICKUP_Electronics_V1.gltf | brief-pickups-v1.md | ✅ Integrated |
| **A9** | Save Point | TC_PROP_SavePoint_V1.gltf | brief-save-point-v1.md | ✅ Integrated |
| **A10** | Workbench | TC_PROP_Workbench_V1.gltf | brief-workbench-v1.md | ✅ Integrated |

---

### B. Code Systems (Core Gameplay) ✅

**Status:** All implemented and tested

| ID | System | File | Purpose | Status |
|----|--------|------|---------|--------|
| **B1** | Beacon Handler | src/World/Beacon.cs | Victory objective with dormant/active states | ✅ Complete |
| **B2** | Combat System | src/Player/MechanicalArmAttack.cs | Attack logic, cooldown, damage gating | ✅ Complete |
| **B3** | Enemy AI (Scout) | src/Enemies/GalaxabrainScout.cs | Finite state machine (Idle, Chase, Attack, Dead) | ✅ Complete |
| **B4** | First-Person Controller | src/Player/FirstPersonController.cs | Movement, look, interact, attack input | ✅ Complete |
| **B5** | Health System | src/Player/PlayerHealth.cs | Damage tracking, death detection, respawn | ✅ Complete |
| **B6** | Inventory System | src/Inventory/MvpInventory.cs | Resource tracking, affordability checks | ✅ Complete |
| **B7** | Mission State | src/Missions/CrashSiteMissionState.cs | Linear progression (collect → craft → defeat → beacon) | ✅ Complete |
| **B8** | Pickup Handler | src/World/ResourcePickup.cs | Collectable resource items | ✅ Complete |
| **B9** | Recipe System | src/Crafting/MechanicalArmRecipe.cs | Data-driven crafting (JSON config) | ✅ Complete |
| **B10** | Save System | src/SaveSystem/CrashSiteSaveService.cs | Offline save/load with validation | ✅ Complete |
| **B11** | Workbench Handler | src/World/Workbench.cs | Crafting interactable | ✅ Complete |

---

### C. Documentation (Briefs, Guides, Reports) ✅

**Status:** All complete

| ID | Document | File | Authority | Status |
|----|-----------|------|-----------|--------|
| **C1** | Art Brief: Beacon | docs/art/briefs/brief-beacon-v1.md | Art Director | ✅ Complete |
| **C2** | Art Brief: Mechanical Arm | docs/art/briefs/brief-mechanical-arm-v1.md | Art Director | ✅ Complete |
| **C3** | Art Brief: Pickups | docs/art/briefs/brief-pickups-v1.md | Art Director | ✅ Complete |
| **C4** | Art Brief: Save Point | docs/art/briefs/brief-save-point-v1.md | Art Director | ✅ Complete |
| **C5** | Art Brief: Scout Enemy | docs/art/briefs/brief-scout-enemy-v1.md | Art Director | ✅ Complete |
| **C6** | Art Brief: Workbench | docs/art/briefs/brief-workbench-v1.md | Art Director | ✅ Complete |
| **C7** | Art Direction: Visual Identity | studio/memory/titanCraft_visual_identity.md | Art Director | ✅ Complete |
| **C8** | Authority: Art Director Agent | studio/agents/art_director.md | Producer | ✅ Complete |
| **C9** | Authority: Gameplay Engineer Agent | studio/agents/gameplay_engineer.md | Producer | ✅ Complete |
| **C10** | Authority: AGENTS.md | AGENTS.md | Producer | ✅ Complete |
| **C11** | Authority: CLAUDE.md | CLAUDE.md | Producer | ✅ Complete |
| **C12** | Authority: README.md | README.md | Producer | ✅ Complete |
| **C13** | Execution Guide: Phase 4–6 | docs/art/execution-guides/phase-4-6-execution-guide.md | Claude Code | ✅ Complete |
| **C14** | Project Manifest | PROJECT.md | Producer + Claude Code | 🟢 In Progress |
| **C15** | Status Report: Phase 4–6 Assets | docs/art/status/phase-4-6-asset-status.md | Claude Code | ✅ Complete |
| **C16** | Test Documentation | docs/testing.md | Engine Architect | ✅ Complete |

---

### D. Data Files (Configuration, Recipes) ✅

**Status:** All configured

| ID | File | Purpose | Status |
|----|------|---------|--------|
| **D1** | data/Recipes/mechanical_arm_mk1.json | Crafting recipe (10 metal, 3 biomass, 2 electronics) | ✅ Complete |
| **D2** | project.godot | Engine config, input map (WASD/ZQSD), export settings | ✅ Complete |
| **D3** | TitanCraft.csproj | .NET project config | ✅ Complete |
| **D4** | THIRD_PARTY_DEPENDENCIES.md | Package metadata | ✅ Complete |

---

### E. Engine/Infrastructure (CI, Build, Tests) ✅

**Status:** All operational

| ID | Component | File | Purpose | Status |
|----|-----------|------|---------|--------|
| **E1** | CI Pipeline | .github/workflows/ci.yml | Linux/Windows build, test, export | ✅ Complete |
| **E2** | Build Script (Linux) | tools/test.sh | Local build/test orchestration | ✅ Complete |
| **E3** | Build Script (Windows) | tools/test.ps1 | Local build/test orchestration | ✅ Complete |
| **E4** | Integration Tests | tests/Integration/IntegrationTestRunner.cs | Godot headless tests | ✅ Complete |
| **E5** | Unit Tests | tests/Unit/*.cs | C# xUnit tests (41 total) | ✅ Complete (41/41 passing) |
| **E6** | PR Template | .github/pull_request_template.md | PR validation checklist | ✅ Complete |

---

### F. Scenes (Godot) ✅

**Status:** All integrated

| ID | Scene | Location | Purpose | Status |
|----|-------|----------|---------|--------|
| **F1** | Beacon Scene | scenes/World/Beacon.tscn | Victory objective with state transitions | ✅ Complete |
| **F2** | Defeat Screen | scenes/UI/DefeatScreen.tscn | UI: Loss condition | ✅ Complete |
| **F3** | Enemy: Scout | scenes/Enemies/GalaxabrainScout.tscn | Primary antagonist | ✅ Complete |
| **F4** | HUD | scenes/UI/HUD.tscn | In-game heads-up display | ✅ Complete |
| **F5** | Main Game Scene | scenes/Main/Main.tscn | Primary gameplay arena | ✅ Complete |
| **F6** | Main Menu | scenes/UI/MainMenu.tscn | Title/start screen | ✅ Complete |
| **F7** | Pause Menu | scenes/UI/PauseMenu.tscn | Gameplay pause interface | ✅ Complete |
| **F8** | Player Avatar | scenes/Player/Player.tscn | First-person controller with arm equipment | ✅ Complete |
| **F9** | Victory Screen | scenes/UI/VictoryScreen.tscn | UI: Win condition | ✅ Complete |
| **F10** | Workbench Scene | scenes/World/Workbench.tscn | Crafting interactable | ✅ Complete |

---

### G. Graphics & Materials ✅

**Status:** All configured

| ID | Material | File | Purpose | Status |
|----|----------|------|---------|--------|
| **G1** | Beacon Active | assets/Materials/Landmarks/BeaconActive.tres | Purple crystal emissive | ✅ Complete |
| **G2** | Beacon Dormant | assets/Materials/Landmarks/BeaconDormant.tres | Red LED standby | ✅ Complete |
| **G3** | Workbench Chassis | assets/Materials/Landmarks/WorkbenchChassis.tres | Beige/off-white base | ✅ Complete |
| **G4** | Workbench Highlight | assets/Materials/Landmarks/WorkbenchHighlight.tres | Orange glow on approach | ✅ Complete |
| **G5** | Resource Highlight | assets/Materials/ResourceDrop/ResourceItemHighlight.tres | Pickup glow | ✅ Complete |

---

## 📊 Task Organization (A→Z by Category)

### Phase 4–6 Tasks (COMPLETED)

#### A. Art Assets Delivery
- [ ] A1 Deliver Beacon (Active) GLTF model → ✅ Delivered
- [ ] A2 Deliver Beacon (Dormant) GLTF model → ✅ Delivered
- [ ] A3 Deliver Component Pickup GLTF model → ✅ Delivered
- [ ] A4 Deliver Galaxabrain Scout GLTF model → ✅ Delivered
- [ ] A5 Deliver Mechanical Arm GLTF model → ✅ Delivered
- [ ] A6 Deliver Metal Pickup GLTF model → ✅ Delivered
- [ ] A7 Deliver Biomass Pickup GLTF model → ✅ Delivered
- [ ] A8 Deliver Electronics Pickup GLTF model → ✅ Delivered
- [ ] A9 Deliver Save Point GLTF model → ✅ Delivered
- [ ] A10 Deliver Workbench GLTF model → ✅ Delivered

#### B. Beacon Implementation
- [ ] B1 Create Beacon.tscn scene → ✅ Complete
- [ ] B2 Implement dormant/active state transition → ✅ Complete
- [ ] B3 Wire to mission victory trigger → ✅ Complete
- [ ] B4 Add activation particle effects → ✅ Complete

#### C. Combat System
- [ ] C1 Implement mechanical arm attack logic → ✅ Complete
- [ ] C2 Add raycast damage system → ✅ Complete
- [ ] C3 Implement attack cooldown → ✅ Complete
- [ ] C4 Gate attacks by arm-built flag → ✅ Complete

#### D. Enemy Implementation
- [ ] D1 Create GalaxabrainScout FSM → ✅ Complete
- [ ] D2 Implement Idle → Chase → Attack states → ✅ Complete
- [ ] D3 Add component drop on defeat → ✅ Complete
- [ ] D4 Configure damage, health, cooldown → ✅ Complete

#### E. Interactables
- [ ] E1 Implement Workbench interactable → ✅ Complete
- [ ] E2 Implement Save Point interactable → ✅ Complete
- [ ] E3 Implement Beacon victory trigger → ✅ Complete
- [ ] E4 Add interaction zone collision → ✅ Complete

#### F. Resources & Pickups
- [ ] F1 Implement Metal pickup → ✅ Complete
- [ ] F2 Implement Biomass pickup → ✅ Complete
- [ ] F3 Implement Electronics pickup → ✅ Complete
- [ ] F4 Implement Component pickup → ✅ Complete

#### G. Inventory & Crafting
- [ ] G1 Implement MvpInventory resource tracking → ✅ Complete
- [ ] G2 Create MechanicalArmRecipe data-driven → ✅ Complete
- [ ] G3 Wire recipe to workbench → ✅ Complete
- [ ] G4 Implement affordability checks → ✅ Complete

#### H. Mission Progression
- [ ] H1 Implement CrashSiteMissionState FSM → ✅ Complete
- [ ] H2 Add collect resources step → ✅ Complete
- [ ] H3 Add build arm step → ✅ Complete
- [ ] H4 Add defeat enemy step → ✅ Complete
- [ ] H5 Add activate beacon step → ✅ Complete

#### I. Save System
- [ ] I1 Implement CrashSiteSaveService → ✅ Complete
- [ ] I2 Add save file validation → ✅ Complete
- [ ] I3 Add version checking → ✅ Complete
- [ ] I4 Implement respawn from save point → ✅ Complete

#### J. UI Implementation
- [ ] J1 Create HUD scene with resource display → ✅ Complete
- [ ] J2 Create Main Menu scene → ✅ Complete
- [ ] J3 Create Pause Menu scene → ✅ Complete
- [ ] J4 Create Victory/Defeat screens → ✅ Complete
- [ ] J5 Add mission objective text display → ✅ Complete

#### K. Input & Controls
- [ ] K1 Add WASD movement mappings → ✅ Complete
- [ ] K2 Add ZQSD (AZERTY) support → ✅ Complete
- [ ] K3 Add interact (E) action → ✅ Complete
- [ ] K4 Add attack (Left Click) action → ✅ Complete
- [ ] K5 Add pause (Esc) action → ✅ Complete

#### L. Testing & Validation
- [ ] L1 Create unit tests for inventory → ✅ Complete
- [ ] L2 Create unit tests for recipes → ✅ Complete
- [ ] L3 Create unit tests for mission state → ✅ Complete
- [ ] L4 Create unit tests for health → ✅ Complete
- [ ] L5 Create unit tests for save system → ✅ Complete
- [ ] L6 Create integration tests → ✅ Complete
- [ ] L7 Verify all 41 unit tests pass → ✅ Complete
- [ ] L8 Verify CI pipeline passes → ✅ Complete

#### M. Documentation
- [ ] M1 Create execution guide for Phase 4–6 → ✅ Complete
- [ ] M2 Create asset status report → ✅ Complete
- [ ] M3 Document all brief fulfillment → ✅ Complete
- [ ] M4 Create project manifest → 🟢 In Progress

---

### Phase 7 Tasks (PLANNING)

#### A. Composition & Visual Polish
- [ ] A1 Create composition guide (Art Director)
- [ ] A2 Implement lighting setup recommendations
- [ ] A3 Add visual effects (particles, bloom)
- [ ] A4 Optimize shader performance
- [ ] A5 Test visual hierarchy at distance

#### B. Polish & Refinement
- [ ] B1 Balance difficulty (enemy damage, health, cooldown)
- [ ] B2 Tune resource distribution (spawn locations, quantities)
- [ ] B3 Test progression pacing (time to craft, time to defeat)
- [ ] B4 Verify UI readability and usability
- [ ] B5 Test save/load reliability

#### C. Gameplay Polish
- [ ] C1 Add sound effects (pickup, craft, attack, enemy)
- [ ] C2 Add music/ambient audio
- [ ] C3 Implement screen shake on hit
- [ ] C4 Add damage popup text
- [ ] C5 Test feel/feedback loop

#### D. Platform Testing
- [ ] D1 Windows native build & test
- [ ] D2 Linux headless validation
- [ ] D3 Export artifact verification
- [ ] D4 Performance profiling
- [ ] D5 Accessibility check

---

## 📈 Progress Metrics

| Metric | Target | Current | Status |
|--------|--------|---------|--------|
| **Art Assets Delivered** | 10 | 10 | ✅ 100% |
| **Code Systems Implemented** | 11 | 11 | ✅ 100% |
| **Unit Tests Passing** | 40+ | 41 | ✅ 102% |
| **Scenes Complete** | 10 | 10 | ✅ 100% |
| **Materials Configured** | 5 | 5 | ✅ 100% |
| **Documentation Pages** | 15+ | 16 | ✅ 107% |
| **Gameplay Systems Wired** | 6 | 6 | ✅ 100% |

---

## 🔗 Critical Links

### Authority Documents
- **README.md** — Product scope, MVP definition, gameplay loop
- **AGENTS.md** — Agent roles, governance, validation workflow
- **CLAUDE.md** — Claude Code operating mandate

### Art Direction
- **studio/agents/art_director.md** — Art Director authority & deliverables
- **studio/memory/titanCraft_visual_identity.md** — Visual identity guide

### Execution Guides
- **docs/art/execution-guides/phase-4-6-execution-guide.md** — Implementation patterns
- **docs/art/status/phase-4-6-asset-status.md** — Asset delivery & integration status

### Test & Build
- **.github/workflows/ci.yml** — CI pipeline definition
- **docs/testing.md** — Test strategy and local execution

---

## 🎮 Gameplay Flow (Implemented MVP Loop)

```
START
  ↓
1. COLLECT RESOURCES
   - Metal pickups (10 required)
   - Biomass pickups (3 required)
   - Electronics pickups (2 required)
   - HUD shows progress
  ↓
2. BUILD MECHANICAL ARM
   - Approach Workbench
   - Press E to craft
   - Arm becomes visible, attack enabled
  ↓
3. DEFEAT GALAXABRAIN SCOUT
   - Enemy spawns at distance
   - Pursues on detection
   - Attack with Left Click (25 damage, 0.8s cooldown)
   - Enemy health: 30 HP
   - Component pickup revealed on death
  ↓
4. COLLECT COMPONENT
   - Approach hidden component
   - Pickup triggers mission progression
  ↓
5. ACTIVATE BEACON
   - Approach beacon
   - Press E to activate
   - Beacon transitions dormant → active
   - Purple crystal emissive glow
   - Victory screen displayed
  ↓
VICTORY
```

---

## 📝 Version History

| Date | Event | Author |
|------|-------|--------|
| 2026-06-27 | Phase 1–3 docs (README, AGENTS, CLAUDE) | Codex |
| 2026-06-28 | Godot project & CI initialization | Codex |
| 2026-06-29 | Gameplay systems (inventory, mission, combat) | Codex |
| 2026-06-29 | UI implementation (HUD, menus, screens) | Codex |
| 2026-06-29 | Input polish (ZQSD support, quit fix) | Codex |
| 2026-06-29 | Tutorial prompt added | Codex |
| 2026-07-05 | Phase 4–6 execution guide & status report | Claude Code |
| 2026-07-05 | Project manifest & GitHub project setup | Claude Code |

---

## 🚀 Next Steps

### Immediate (Next 1–2 Days)
1. **Visual Review** — Art Director signs off on rendered scenes
2. **Gameplay Balance** — Playtest difficulty, resource distribution
3. **Build Verification** — Confirm Windows/Linux exports succeed

### Short-term (Next 1 Week)
1. **Phase 7 Planning** — Composition guide, lighting setup
2. **Polish Pass** — Sound, particle effects, screen shake
3. **Accessibility** — Colorblind mode, font sizes, control remapping

### Medium-term (Weeks 2–4)
1. **Performance Optimization** — Profile and optimize rendering
2. **Stability Testing** — Long play sessions, edge case testing
3. **Release Preparation** — Export, documentation, version control

---

## 📞 Project Contacts

| Role | Authority | MCP Reference |
|------|-----------|---|
| **Producer** | Scope, schedule, decisions | README.md, AGENTS.md |
| **Art Director** | Visual briefs, asset delivery, quality | studio/agents/art_director.md |
| **Gameplay Engineer** | Game logic, balance, progression | studio/agents/gameplay_engineer.md |
| **Engine Architect** | Build, CI, performance | studio/agents/engine_architect.md |
| **Claude Code** | Code review, architecture validation | CLAUDE.md |

---

**Project Status:** 🟢 Phase 4–6 Complete | Ready for Phase 7 Planning  
**Last Commit:** Phase 4–6 execution guide & status report  
**Next Review:** Phase 7 composition guide (Art Director)

