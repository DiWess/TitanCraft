using System;
using TitanCraft.Enemies;
using TitanCraft.Resources;

namespace TitanCraft.Player;

public sealed class MechanicalArmAttackLogic
{
    public const int DefaultMechanicalArmDamage = 25;
    public const float DefaultCooldownSeconds = 0.8f;
    public const float DefaultRange = 3.0f;

    private float _cooldownRemainingSeconds;

    public MechanicalArmAttackLogic(int damage = DefaultMechanicalArmDamage, float cooldownSeconds = DefaultCooldownSeconds)
    {
        if (damage <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(damage), "Mechanical arm damage must be greater than zero.");
        }

        if (cooldownSeconds < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(cooldownSeconds), "Mechanical arm cooldown cannot be negative.");
        }

        Damage = damage;
        CooldownSeconds = cooldownSeconds;
    }

    public int Damage { get; }

    public float CooldownSeconds { get; }

    public bool IsOnCooldown => _cooldownRemainingSeconds > 0f;

    public void Tick(float deltaSeconds)
    {
        _cooldownRemainingSeconds = Math.Max(0f, _cooldownRemainingSeconds - Math.Max(0f, deltaSeconds));
    }

    public bool TryAttack(MvpInventory inventory, GalaxabrainScoutBrain target)
    {
        ArgumentNullException.ThrowIfNull(inventory);
        ArgumentNullException.ThrowIfNull(target);

        if (!inventory.IsMechanicalArmBuilt || IsOnCooldown || target.IsDead)
        {
            return false;
        }

        target.ApplyDamage(Damage);
        _cooldownRemainingSeconds = CooldownSeconds;
        return true;
    }
}
