using System;
using System.Collections.Generic;

namespace TitanCraft.World;

public enum HoldoutEnemyKind
{
    Drone,
    Scout,
}

/// <summary>Enemies that arrive once the beacon has charged for <see cref="AtChargeSeconds"/>.</summary>
public readonly record struct HoldoutWave(float AtChargeSeconds, int Drones, int Scouts);

/// <summary>
/// The beacon holdout's timing, kept engine-free (README section 6 amendment,
/// D4: the climax is an encounter, not a button press). The beacon charges only
/// while the player holds the plaza; waves arrive by charge reached, not by
/// clock, so walking away pauses the pressure along with the progress. At most
/// <see cref="MaxActiveEnemies"/> enemies are alive at once (README section 28
/// allows two or three); later arrivals queue until there is room.
/// </summary>
public sealed class BeaconHoldoutLogic
{
    public const float DefaultChargeSeconds = 75f;
    public const int DefaultMaxActiveEnemies = 3;

    public static readonly IReadOnlyList<HoldoutWave> DefaultWaves =
    [
        new(0f, Drones: 2, Scouts: 0),
        new(14f, Drones: 0, Scouts: 1),
        new(28f, Drones: 1, Scouts: 1),
        new(44f, Drones: 2, Scouts: 0),
        new(58f, Drones: 1, Scouts: 1),
    ];

    private readonly IReadOnlyList<HoldoutWave> _waves;
    private readonly Queue<HoldoutEnemyKind> _pending = new();
    private int _nextWave;

    public BeaconHoldoutLogic(float chargeSeconds = DefaultChargeSeconds, IReadOnlyList<HoldoutWave>? waves = null,
        int maxActiveEnemies = DefaultMaxActiveEnemies)
    {
        if (chargeSeconds <= 0f)
            throw new ArgumentOutOfRangeException(nameof(chargeSeconds), "Charge time must be greater than zero.");
        if (maxActiveEnemies <= 0)
            throw new ArgumentOutOfRangeException(nameof(maxActiveEnemies), "At least one enemy must be allowed.");

        ChargeSeconds = chargeSeconds;
        MaxActiveEnemies = maxActiveEnemies;
        _waves = waves ?? DefaultWaves;
    }

    public float ChargeSeconds { get; }

    public int MaxActiveEnemies { get; }

    public float ChargedSeconds { get; private set; }

    public float ChargeFraction => Math.Clamp(ChargedSeconds / ChargeSeconds, 0f, 1f);

    public bool IsComplete => ChargedSeconds >= ChargeSeconds;

    public int PendingSpawns => _pending.Count;

    public int TotalEnemies
    {
        get
        {
            var total = 0;
            foreach (var wave in _waves)
                total += wave.Drones + wave.Scouts;
            return total;
        }
    }

    /// <summary>
    /// Advances the charge (only while <paramref name="playerHoldsPlaza"/>) and
    /// returns the enemies to spawn this tick given <paramref name="activeEnemies"/> alive.
    /// </summary>
    public IReadOnlyList<HoldoutEnemyKind> Tick(float deltaSeconds, bool playerHoldsPlaza, int activeEnemies)
    {
        if (IsComplete)
            return Array.Empty<HoldoutEnemyKind>();

        if (playerHoldsPlaza)
            ChargedSeconds = Math.Min(ChargeSeconds, ChargedSeconds + Math.Max(0f, deltaSeconds));

        while (_nextWave < _waves.Count && _waves[_nextWave].AtChargeSeconds <= ChargedSeconds)
        {
            var wave = _waves[_nextWave++];
            // Interleave so a mixed wave does not arrive as all of one kind first.
            for (var i = 0; i < Math.Max(wave.Drones, wave.Scouts); i++)
            {
                if (i < wave.Scouts)
                    _pending.Enqueue(HoldoutEnemyKind.Scout);
                if (i < wave.Drones)
                    _pending.Enqueue(HoldoutEnemyKind.Drone);
            }
        }

        if (IsComplete || _pending.Count == 0)
            return Array.Empty<HoldoutEnemyKind>();

        var room = MaxActiveEnemies - Math.Max(0, activeEnemies);
        var spawns = new List<HoldoutEnemyKind>();
        while (room-- > 0 && _pending.Count > 0)
            spawns.Add(_pending.Dequeue());
        return spawns;
    }
}
