using System;

namespace TitanCraft.Resources;

public sealed class MvpInventory
{
    /// <summary>Highest Mechanical Arm module: Mk I strike, Mk II magnet clamp, Mk III breaker coil.</summary>
    public const int MaxArmLevel = 3;

    public event Action<MvpInventory>? Changed;
    public int Metal { get; private set; }

    public int Biomass { get; private set; }

    public int ElectronicComponents { get; private set; }

    /// <summary>
    /// 0 before crafting, then 1-3 as the one arm gains modules (README section 6
    /// amendment, D2: a single tool that evolves, not several weapons).
    /// </summary>
    public int ArmLevel { get; private set; }

    public bool IsMechanicalArmBuilt => ArmLevel >= 1;

    public bool HasMagnetClamp => ArmLevel >= 2;

    public bool HasBreakerCoil => ArmLevel >= 3;

    /// <summary>The Scout's component, carried until it is fitted into the arm as Mk II.</summary>
    public bool HasGalaxabrainComponent { get; private set; }

    /// <summary>The beacon's power core, taken back from the Warden.</summary>
    public bool HasPowerCore { get; private set; }

    public void AddResources(int metal = 0, int biomass = 0, int electronicComponents = 0)
    {
        ThrowIfNegative(metal, nameof(metal));
        ThrowIfNegative(biomass, nameof(biomass));
        ThrowIfNegative(electronicComponents, nameof(electronicComponents));

        Metal += metal;
        Biomass += biomass;
        ElectronicComponents += electronicComponents;
        Changed?.Invoke(this);
    }

    public bool CanSpendResources(int metal = 0, int biomass = 0, int electronicComponents = 0)
    {
        ThrowIfNegative(metal, nameof(metal));
        ThrowIfNegative(biomass, nameof(biomass));
        ThrowIfNegative(electronicComponents, nameof(electronicComponents));

        return Metal >= metal
            && Biomass >= biomass
            && ElectronicComponents >= electronicComponents;
    }

    public bool TrySpendResources(int metal = 0, int biomass = 0, int electronicComponents = 0)
    {
        if (!CanSpendResources(metal, biomass, electronicComponents))
        {
            return false;
        }

        Metal -= metal;
        Biomass -= biomass;
        ElectronicComponents -= electronicComponents;
        Changed?.Invoke(this);
        return true;
    }

    public void MarkMechanicalArmBuilt() => UpgradeArm(1);

    /// <summary>Raises the arm to <paramref name="level"/>; never lowers it.</summary>
    public void UpgradeArm(int level)
    {
        if (level < 1 || level > MaxArmLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(level), $"Arm level must be 1-{MaxArmLevel}.");
        }

        if (level <= ArmLevel)
        {
            return;
        }

        ArmLevel = level;
        Changed?.Invoke(this);
    }

    public void MarkGalaxabrainComponentCollected()
    {
        HasGalaxabrainComponent = true;
        Changed?.Invoke(this);
    }

    /// <summary>The component is fitted into the arm when Mk II is crafted.</summary>
    public bool TryConsumeGalaxabrainComponent()
    {
        if (!HasGalaxabrainComponent)
        {
            return false;
        }

        HasGalaxabrainComponent = false;
        Changed?.Invoke(this);
        return true;
    }

    public void MarkPowerCoreCollected()
    {
        HasPowerCore = true;
        Changed?.Invoke(this);
    }

    public void Restore(int metal, int biomass, int electronicComponents, bool isMechanicalArmBuilt, bool hasGalaxabrainComponent)
        => Restore(metal, biomass, electronicComponents, isMechanicalArmBuilt ? 1 : 0, hasGalaxabrainComponent, hasPowerCore: false);

    public void Restore(int metal, int biomass, int electronicComponents, int armLevel, bool hasGalaxabrainComponent, bool hasPowerCore)
    {
        ThrowIfNegative(metal, nameof(metal));
        ThrowIfNegative(biomass, nameof(biomass));
        ThrowIfNegative(electronicComponents, nameof(electronicComponents));
        if (armLevel < 0 || armLevel > MaxArmLevel)
        {
            throw new ArgumentOutOfRangeException(nameof(armLevel), $"Arm level must be 0-{MaxArmLevel}.");
        }

        Metal = metal;
        Biomass = biomass;
        ElectronicComponents = electronicComponents;
        ArmLevel = armLevel;
        HasGalaxabrainComponent = hasGalaxabrainComponent;
        HasPowerCore = hasPowerCore;
        Changed?.Invoke(this);
    }

    private static void ThrowIfNegative(int quantity, string parameterName)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Resource quantities cannot be negative.");
        }
    }
}
