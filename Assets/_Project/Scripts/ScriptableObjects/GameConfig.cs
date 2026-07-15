using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Money")]
    [field: SerializeField] public int MoneyPerWave { get; } = 50;
    [field: SerializeField] public int MoneyPerKill { get; } = 10;

    [Header("Building Settings")]
    [field: SerializeField] public float MinDistanceForBuilding { get; } = 1f;
    [field: SerializeField] public int FastTowerCost { get; } = 10;
    [field: SerializeField] public int StrongTowerCost { get; } = 10;

    [Header("Towers Settings")]
    [field: SerializeField] public float RadiusRangeFastTower { get; } = 1;
    [field: SerializeField] public float RadiusRangeStrongTower { get; } = 1;
    [field: SerializeField] public float DelayShootFastTower { get; } = 0.1f;
    [field: SerializeField] public float DelayShootStrongTower { get; } = 0.8f;
    [field: SerializeField] public int DamageStrongTower { get; } = 3;
    [field: SerializeField] public int DamageFastTower { get; } = 1;

    [Header("Castle Upgrade Settings")]
    [field: SerializeField] public int MaxCastleLevel { get; } = 10;
    [field: SerializeField] public int UpgradeCastleCost { get; } = 10;
    [field: SerializeField] public float BulletSpeed { get; } = 3f;
    [field: SerializeField, Min(1.1f)] public float UpgradeMultiplier { get; } = 1.2f;
    [field: SerializeField, Min(1.1f)] public float CostMultiplier { get; } = 1.2f;
    [field: SerializeField] public float RadiusRangeCastle { get; } = 1;
    [field: SerializeField] public float StartDelayShootCastle { get; } = 1;
    [field: SerializeField] public int StartDamageCastle { get; } = 1;
    [field: SerializeField] public int StartHealthCastle { get; } = 25;


    [Header("Enemy Settings")]
    [field: SerializeField] public int WavesDelay { get; } = 15;
    [field: SerializeField] public float EnemySpawnDelay { get; } = 1f;
    [field: SerializeField] public float EnemySpeed { get; } = 0.5f;
    [field: SerializeField] public float EnemyAttackDelay { get; } = 1f;
    [field: SerializeField] public int EnemyDamage { get; } = 1;
    [field: SerializeField] public int EnemyHealth { get; } = 1;
    [field: SerializeField] public int CostEnemyDeath { get; } = 10;
    [field: SerializeField] public int EnemiesPerWave { get; } = 5;
    [field: SerializeField] public float EnemyStopDistance { get; } = 1f;
    [field: SerializeField, Min(1.1f)] public float MultiplierEnemiesPerWave { get; } = 1.2f;
}