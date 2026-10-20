namespace TitanCraft.Missions;

/// <summary>
/// Represents the current step/objective in the crash site mission.
/// Values are persisted in saves (save version 2); append-only from here on.
/// </summary>
public enum CrashSiteMissionStep
{
    CollectResources = 0,
    Start = CollectResources,
    BuildMechanicalArm = 1,
    DefeatGalaxabrain = 2,
    RecoverGalaxabrainComponent = 3,
    ComponentRecovery = RecoverGalaxabrainComponent,

    // Workstream B: the arm becomes the game.
    UpgradeArmMk2 = 4,
    OpenNetYard = 5,
    ClearDroneNest = 6,
    UpgradeArmMk3 = 7,
    BreachTowerGate = 8,

    // Workstream C: the Warden and the beacon holdout.
    DefeatWarden = 9,
    RecoverPowerCore = 10,
    ActivateBeacon = 11,
    DefendBeacon = 12,
    Victory = 13,
}
