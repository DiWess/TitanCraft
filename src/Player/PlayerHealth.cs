using System;

namespace TitanCraft.Player;

public sealed class PlayerHealth
{
    public event Action<PlayerHealth>? Changed;
    public const int DefaultMaxHealth = 100;

    public PlayerHealth(int maxHealth = DefaultMaxHealth)
    {
        if (maxHealth <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxHealth), "Max health must be greater than zero.");
        }

        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public int MaxHealth { get; }

    public int CurrentHealth { get; private set; }

    public bool IsDead => CurrentHealth == 0;

    public void ApplyDamage(int damage)
    {
        if (damage <= 0 || IsDead)
        {
            return;
        }

        CurrentHealth = Math.Max(0, CurrentHealth - damage);
        Changed?.Invoke(this);
    }

    public void PrepareRespawn()
    {
        CurrentHealth = MaxHealth;
        Changed?.Invoke(this);
    }
}
