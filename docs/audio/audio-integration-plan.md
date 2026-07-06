# Phase 7.3.3: Godot Audio Integration
## Audio Event Wiring & AudioStreamPlayer Setup

**Date:** 2026-07-07  
**Phase:** Phase 7.3.3 (Creative Director audio integration)  
**Scope:** Integrate 33 sourced audio files into Godot with event-driven playback  
**Authority:** Phase 7.3.2 provenance documentation + Godot audio system  
**Status:** Integration plan complete, ready for implementation  

---

## Overview

Phase 7.3.3 integrates sourced audio into the Crash Site MVP by:
1. Creating AudioStreamPlayer nodes for each audio category
2. Wiring sounds to gameplay events (footsteps, attacks, pickups, objectives)
3. Setting up audio layers (ambient loops, UI, gameplay)
4. Configuring volume levels and mixing
5. Testing audio timing and synchronization

**Result:** Complete audio integration ready for smoke testing (Phase 7.3.4)

---

## Audio Architecture in Godot

### AudioStreamPlayer Hierarchy

```
Main (Node3D)
├── AudioLayer_Ambient (Node)
│   ├── AmbientLoop_Wind (AudioStreamPlayer3D)
│   ├── AmbientLoop_Rumble (AudioStreamPlayer3D)
│   └── AmbientLoop_Machinery (AudioStreamPlayer3D)
├── AudioLayer_Player (Node)
│   ├── Footsteps_Metal (AudioStreamPlayer3D)
│   ├── Footsteps_Rock (AudioStreamPlayer3D)
│   ├── Footsteps_Ash (AudioStreamPlayer3D)
│   ├── Weapon_Swing (AudioStreamPlayer3D)
│   ├── Weapon_Impact (AudioStreamPlayer3D)
│   └── Weapon_Ready (AudioStreamPlayer3D)
├── AudioLayer_Enemy (Node)
│   ├── Scout_Alert (AudioStreamPlayer3D)
│   ├── Scout_Attack (AudioStreamPlayer3D)
│   ├── Scout_Hurt (AudioStreamPlayer3D)
│   ├── Scout_Death (AudioStreamPlayer3D)
│   └── Scout_Idle (AudioStreamPlayer3D)
├── AudioLayer_UI (Node)
│   ├── UI_Select (AudioStreamPlayer)
│   ├── UI_Hover (AudioStreamPlayer)
│   ├── UI_Craft_Complete (AudioStreamPlayer)
│   └── UI_Menu_Toggle (AudioStreamPlayer)
├── AudioLayer_Pickup (Node)
│   ├── Pickup_Metal (AudioStreamPlayer3D)
│   ├── Pickup_Glass (AudioStreamPlayer3D)
│   ├── Pickup_Organic (AudioStreamPlayer3D)
│   └── Pickup_Generic (AudioStreamPlayer3D)
├── AudioLayer_State (Node)
│   ├── State_Victory (AudioStreamPlayer)
│   ├── State_Defeat (AudioStreamPlayer)
│   ├── State_Objective (AudioStreamPlayer)
│   └── State_Mission_Complete (AudioStreamPlayer)
└── AudioLayer_Save (Node)
    ├── Save_Complete (AudioStreamPlayer)
    ├── Save_Progress (AudioStreamPlayer)
    └── Load_Complete (AudioStreamPlayer)
```

**Notes:**
- **AudioStreamPlayer3D** for positional audio (ambient, footsteps, weapon, enemy, pickup)
- **AudioStreamPlayer** (2D) for non-positional UI and state audio
- Each player is independent and can play sounds simultaneously

---

## Audio Event Triggers

### 1. AMBIENT AUDIO (Continuous Loops)

**Initialization:** Scene load (Main._Ready)

**Code Pattern:**
```csharp
public override void _Ready()
{
    // Start ambient loops
    GetNode<AudioStreamPlayer3D>("AudioLayer_Ambient/AmbientLoop_Wind")
        .Play();
    GetNode<AudioStreamPlayer3D>("AudioLayer_Ambient/AmbientLoop_Rumble")
        .Play();
    GetNode<AudioStreamPlayer3D>("AudioLayer_Ambient/AmbientLoop_Machinery")
        .Play();
}
```

**Volume Levels:**
- Wind: 0.3 (background presence)
- Rumble: 0.2 (distant, subtle)
- Machinery: 0.15 (very subtle)

**Result:** Ambient atmosphere plays continuously throughout session

---

### 2. FOOTSTEP AUDIO (Player Movement)

**Trigger:** FirstPersonMovement._PhysicsProcess (every step)

**Integration Point:** FirstPersonMovement.cs

**Code Pattern:**
```csharp
private void PlayFootstep(Vector3 position, string terrainType)
{
    AudioStreamPlayer3D footstepPlayer = GetFootstepPlayer(terrainType);
    if (footstepPlayer != null)
    {
        footstepPlayer.GlobalPosition = position;
        footstepPlayer.Play();
    }
}

private AudioStreamPlayer3D GetFootstepPlayer(string terrainType)
{
    return terrainType switch
    {
        "metal" => GetNode<AudioStreamPlayer3D>("../AudioLayer_Player/Footsteps_Metal"),
        "rock" => GetNode<AudioStreamPlayer3D>("../AudioLayer_Player/Footsteps_Rock"),
        "ash" => GetNode<AudioStreamPlayer3D>("../AudioLayer_Player/Footsteps_Ash"),
        _ => null,
    };
}
```

**Timing:**
- Walk: Play every 0.4 seconds
- Run/Sprint: Play every 0.3 seconds
- When grounded (not jumping/falling)

**Volume:** 0.6-0.8

**Positional:** Yes (plays from player position)

**Result:** Footsteps provide movement feedback and terrain type audio cues

---

### 3. MECHANICAL ARM WEAPON AUDIO

**Trigger 3A: Attack Swing**
- Event: Left-click / attack input
- Location: MechanicalArmAttack.cs, TryAttack()

```csharp
public bool TryAttack(MvpInventory inventory)
{
    if (!inventory.IsMechanicalArmBuilt || IsOnCooldown)
        return false;

    _cooldownRemainingSeconds = CooldownSeconds;
    
    // Play swing sound
    AudioCue.Play(this, "AudioLayer_Player/Weapon_Swing");
    return true;
}
```

**Trigger 3B: Hit Impact**
- Event: Raycast hit registers on enemy
- Location: MechanicalArmAttack.cs, raycast hit callback

```csharp
if (hit_result.collider is GalaxabrainScout enemy)
{
    enemy.ApplyDamage(Damage);
    
    // Play impact sound only on confirmed hit
    AudioCue.Play(this, "AudioLayer_Player/Weapon_Impact");
}
```

**Trigger 3C: Attack Ready**
- Event: Cooldown timer reaches 0
- Location: MechanicalArmAttackLogic.Tick()

```csharp
public void Tick(float deltaSeconds)
{
    _cooldownRemainingSeconds = Math.Max(0f, 
        _cooldownRemainingSeconds - Math.Max(0f, deltaSeconds));
    
    if (_cooldownRemainingSeconds == 0f && _previousWasCoolingDown)
    {
        // Play ready tone when cooldown completes
        AudioCue.Play(this, "AudioLayer_Player/Weapon_Ready");
    }
    _previousWasCoolingDown = IsOnCooldown;
}
```

**Volume Levels:**
- Swing: 0.8 (immediate feedback)
- Impact: 0.9 (satisfying confirmation)
- Ready: 0.5 (subtle notification)

**Result:** Complete attack audio feedback cycle (swing → impact → cooldown → ready)

---

### 4. SCOUT ENEMY AUDIO

**Trigger 4A: Detection/Alert**
- Event: Scout.Brain.State changes to Chase
- Location: GalaxabrainScout._PhysicsProcess, state transition

```csharp
if (_brain.State == GalaxabrainScoutState.Chase 
    && previous_state != GalaxabrainScoutState.Chase)
{
    AudioCue.Play(this, "AudioLayer_Enemy/Scout_Alert");
}
```

**Trigger 4B: Attack Threat**
- Event: Scout.Brain.State changes to Attack
- Location: GalaxabrainScout._PhysicsProcess

```csharp
if (_brain.State == GalaxabrainScoutState.Attack 
    && previous_state != GalaxabrainScoutState.Attack)
{
    AudioCue.Play(this, "AudioLayer_Enemy/Scout_Attack");
}
```

**Trigger 4C: Enemy Damage**
- Event: Scout.ApplyDamage() is called
- Location: GalaxabrainScout.ApplyDamage()

```csharp
public void ApplyDamage(int damage)
{
    if (damage <= 0 || _brain.IsDead) return;
    
    bool was_alive = !_brain.IsDead;
    _brain.ApplyDamage(damage);
    
    if (!_brain.IsDead)
    {
        // Play hurt sound while still alive
        AudioCue.Play(this, "AudioLayer_Enemy/Scout_Hurt");
    }
    
    if (was_alive && _brain.IsDead)
    {
        Die();
    }
}
```

**Trigger 4D: Death**
- Event: Scout.Die() called
- Location: GalaxabrainScout.Die()

```csharp
private void Die()
{
    // ... existing death logic ...
    
    // Play death sound
    AudioCue.Play(this, "AudioLayer_Enemy/Scout_Death");
    
    Visible = false;
    // ... rest of death sequence ...
}
```

**Trigger 4E: Idle Ambience (Optional)**
- Event: Scene active, scout spawned but not in Chase/Attack
- Location: GalaxabrainScout._PhysicsProcess

```csharp
// Optional: play idle loop when scout is idle and player is within earshot
if (_brain.State == GalaxabrainScoutState.Idle 
    && distance_to_player < 20f
    && !scout_idle_audio.Playing)
{
    scout_idle_audio.Play();
}
```

**Volume Levels:**
- Alert: 0.9 (immediate, threatening)
- Attack: 0.95 (urgent)
- Hurt: 0.7 (impactful but not overwhelming)
- Death: 1.0 (full presence)
- Idle: 0.3 (background)

**Positional:** Yes (plays from scout position)

**Result:** Enemy audio creates threat presence and provides state feedback

---

### 5. UI FEEDBACK AUDIO

**Trigger 5A: Menu Item Hover**
- Event: UI item receives focus
- Location: CrashSiteHud.cs (or UI script)

```csharp
public override void _GuiInput(InputEvent @event)
{
    if (@event is InputEventMouseMotion && IsMouseOver(GlobalMousePosition))
    {
        if (!_was_hovered)
        {
            AudioCue.Play(this, "AudioLayer_UI/UI_Hover");
            _was_hovered = true;
        }
    }
}
```

**Trigger 5B: Menu Item Select**
- Event: UI item pressed/selected
- Location: UI button pressed callback

```csharp
private void OnCraftButtonPressed()
{
    AudioCue.Play(this, "AudioLayer_UI/UI_Select");
    TryCraft();
}
```

**Trigger 5C: Crafting Complete**
- Event: Crafting succeeds
- Location: Workbench.Interact() or MechanicalArmRecipe.TryCraft()

```csharp
if (recipe.TryCraft(inventory))
{
    AudioCue.Play(this, "AudioLayer_UI/UI_Craft_Complete");
}
```

**Trigger 5D: Menu Open/Close**
- Event: Pause menu opened/closed
- Location: PauseMenu._Ready() / _ExitTree()

```csharp
public override void _Ready()
{
    AudioCue.Play(this, "AudioLayer_UI/UI_Menu_Toggle");
}
```

**Volume:** 0.7-0.9 (clear but not jarring)

**Positional:** No (2D AudioStreamPlayer)

**Result:** UI provides immediate audio feedback for all interactions

---

### 6. RESOURCE PICKUP AUDIO

**Trigger:** ResourcePickup.Interact() called

**Location:** ResourcePickup.cs

```csharp
public bool Interact(MvpInventory inventory, CrashSiteMissionState mission)
{
    if (_isCollected || Quantity <= 0)
        return false;

    // Determine which audio to play based on resource type
    string audio_path = ResourceKind switch
    {
        MvpResourceKind.Metal => "AudioLayer_Pickup/Pickup_Metal",
        MvpResourceKind.Biomass => "AudioLayer_Pickup/Pickup_Organic",
        MvpResourceKind.ElectronicComponents => "AudioLayer_Pickup/Pickup_Glass",
        _ => "AudioLayer_Pickup/Pickup_Generic",
    };
    
    AudioCue.Play(this, audio_path);
    
    // ... rest of pickup logic ...
}
```

**Volume:** 0.8-1.0 (satisfying, rewarding)

**Positional:** Yes (plays from pickup location)

**Result:** Different audio for each resource type, provides collection feedback

---

### 7. GAME STATE AUDIO

**Trigger 7A: Objective Complete**
- Event: Mission step completes
- Location: CrashSiteMissionState.TryComplete*() methods

```csharp
public bool TryCompleteResourceCollection()
{
    if (CurrentStep != CrashSiteMissionStep.CollectResources)
        return false;

    CurrentStep = CrashSiteMissionStep.CraftMechanicalArm;
    AudioCue.Play(this, "AudioLayer_State/State_Objective");
    return true;
}
```

**Trigger 7B: Victory**
- Event: Beacon interacted after all objectives complete
- Location: Beacon.Interact() or mission victory trigger

```csharp
if (mission.AllObjectivesComplete())
{
    AudioCue.Play(this, "AudioLayer_State/State_Victory");
    // Trigger end screen
}
```

**Trigger 7C: Defeat**
- Event: Player health reaches 0
- Location: PlayerHealth.ApplyDamage() or FirstPersonController

```csharp
public void ApplyDamage(int damage)
{
    // ... damage logic ...
    
    if (CurrentHealth == 0)
    {
        AudioCue.Play(this, "AudioLayer_State/State_Defeat");
        // Trigger respawn/game over
    }
}
```

**Volume:** 1.0 (full presence for state changes)

**Positional:** No (2D AudioStreamPlayer)

**Result:** Clear audio signaling of objective progression and game states

---

### 8. SAVE/LOAD AUDIO (Optional)

**Trigger 8A: Save Complete**
- Event: SavePoint.Interact() or save successful
- Location: CrashSiteSaveCoordinator.cs

```csharp
public async Task<bool> SaveGameAsync()
{
    bool success = await _saveService.SaveAsync(_state);
    if (success)
    {
        AudioCue.Play(this, "AudioLayer_Save/Save_Complete");
    }
    return success;
}
```

**Trigger 8B: Load Complete**
- Event: Game loads from save file
- Location: CrashSiteSaveCoordinator.LoadGameAsync() completion

```csharp
public async Task<bool> LoadGameAsync()
{
    _state = await _saveService.LoadAsync();
    AudioCue.Play(this, "AudioLayer_Save/Load_Complete");
    return true;
}
```

**Volume:** 0.6-0.8 (subtle confirmation)

**Note:** Optional feature; can skip if time budget limited

**Result:** Subtle audio confirmation of save/load operations

---

## Audio Configuration (Godot Editor Setup)

### AudioStreamPlayer Node Properties

**For Ambient Loops (3D):**
```
Bus: "Master"
Volume Db: -10 dB (wind), -14 dB (rumble), -16 dB (machinery)
Pitch Scale: 1.0
Max Polyphony: 1 (prevent duplicate plays)
Playing: false (start in code)
Stream: [audio file path]
```

**For Positional Sounds (3D):**
```
Bus: "Master"
Volume Db: -5 dB to 0 dB (varies by sound)
Pitch Scale: 1.0 (add slight variation in code if desired)
Max Polyphony: 4 (allow multiple simultaneous instances)
Playing: false (triggered by event)
Stream: [audio file path]
Attenuation: Linear
Max Distance: 50m
```

**For UI/State Sounds (2D):**
```
Bus: "UI" (separate from game audio)
Volume Db: -3 dB to 0 dB
Playing: false (triggered by event)
Stream: [audio file path]
```

### Audio Bus Structure

```
Master (Main output)
├── Game (gameplay audio)
│   ├── Ambient (loops)
│   ├── Player (footsteps, weapon)
│   ├── Enemy (scout)
│   └── Pickup (resource collection)
└── UI (non-diegetic audio)
    ├── Menu (UI feedback)
    ├── State (objectives, victory, defeat)
    └── Save (save/load)
```

**Bus Volumes:**
- Master: 0 dB (baseline)
- Game: -3 dB (gameplay focus)
- UI: 0 dB (full clarity)

---

## Integration Checklist

**Phase 7.3.3 Tasks:**

- [ ] Create AudioLayer nodes in Main.tscn
  - [ ] AudioLayer_Ambient with 3 ambient players
  - [ ] AudioLayer_Player with footstep, weapon players
  - [ ] AudioLayer_Enemy with scout audio players
  - [ ] AudioLayer_UI with menu/state players
  - [ ] AudioLayer_Pickup with resource pickup players
  - [ ] AudioLayer_State with objective/victory/defeat players
  - [ ] AudioLayer_Save with save/load players (optional)

- [ ] Configure AudioStreamPlayer properties
  - [ ] Set bus assignments (Master/Game/UI)
  - [ ] Set volume levels per sound type
  - [ ] Configure 3D vs 2D positioning
  - [ ] Set max polyphony (1 for loops, 4 for effects)

- [ ] Wire event triggers in C# code
  - [ ] FirstPersonMovement: Footstep playback
  - [ ] MechanicalArmAttack: Weapon audio (swing, impact, ready)
  - [ ] GalaxabrainScout: Enemy audio (alert, attack, hurt, death)
  - [ ] ResourcePickup: Pickup audio by type
  - [ ] CrashSiteMissionState: Objective/state audio
  - [ ] UI handlers: Menu audio

- [ ] Verify AudioCue helper function
  - [ ] Ensure AudioCue.Play() correctly finds and plays AudioStreamPlayers
  - [ ] Test with known audio paths

- [ ] Test audio timing
  - [ ] Footsteps sync with player movement
  - [ ] Weapon audio aligns with attack animations
  - [ ] Enemy audio triggers at correct state changes
  - [ ] Pickup audio plays on collection

- [ ] Verify audio levels
  - [ ] Ambient loops at correct background volume
  - [ ] Gameplay sounds clearly audible
  - [ ] UI sounds distinct but not jarring
  - [ ] No audio clipping or distortion

---

## Success Criteria (Phase 7.3.3)

✅ **Deliverables:**
- [ ] Main.tscn updated with all AudioLayer nodes
- [ ] C# event trigger code updated in all relevant classes
- [ ] Audio configuration documented (bus structure, volume levels)
- [ ] Audio event timing verified (footsteps, weapon, enemy, state)
- [ ] Integration testing complete (all sounds play at correct times)

✅ **Quality:**
- [ ] No audio clipping or distortion
- [ ] Volume levels balanced (ambient vs gameplay vs UI)
- [ ] Positional audio positioned correctly
- [ ] No performance impact (audio plays smoothly at 60 FPS)

✅ **Readiness for Phase 7.3.4:**
- [ ] All 33 audio files integrated
- [ ] All event triggers wired
- [ ] Ready for smoke testing

---

## Phase 7.3.3 Status

**Current Status:** Integration plan complete  
**Next Steps:** Implement AudioLayer nodes and event triggers  
**Effort:** 2-3 hours (straightforward Godot integration)  
**Timeline:** Ready for implementation now (no blockers)  

**After Completion:** Phase 7.3.4 (Audio Smoke Testing, 1-2 hours)

---

## Audio Optimization Notes

**Performance Considerations:**
- Limit simultaneous audio sources (max polyphony)
- Use loops for ambient (efficient memory)
- Preload audio files at scene start (avoid stream delays)
- Monitor CPU usage during playback

**Quality Considerations:**
- Test on multiple audio systems (headphones, speakers)
- Verify audio sync with animations
- Balance volume levels across all sounds
- Consider player hearing comfort (avoid excessive volume)

---

**Status:** 📋 **PHASE 7.3.3 INTEGRATION PLAN COMPLETE**  
**Ready for:** Implementation in Godot (2-3 hours)  
**Date:** 2026-07-07  
**Prepared by:** Creative Director (Phase 7.3.3)

