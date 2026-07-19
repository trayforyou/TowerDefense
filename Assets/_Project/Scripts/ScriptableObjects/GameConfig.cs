using UnityEngine;

namespace TowerDefense.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [field: SerializeField] public int MoneyPerWave { get; private set; } = 50;
        [field: SerializeField] public int MoneyPerKill { get; private set; } = 10;

        [field: SerializeField] public float MinDistanceForBuilding { get; private set; } = 1f;
        [field: SerializeField] public int FastTowerCost { get; private set; } = 10;
        [field: SerializeField] public int StrongTowerCost { get; private set; } = 10;

        [field: SerializeField] public float RadiusRangeFastTower { get; private set; } = 1;
        [field: SerializeField] public float RadiusRangeStrongTower { get; private set; } = 1;
        [field: SerializeField] public float DelayShootFastTower { get; private set; } = 0.1f;
        [field: SerializeField] public float DelayShootStrongTower { get; private set; } = 0.8f;
        [field: SerializeField] public int DamageStrongTower { get; private set; } = 3;
        [field: SerializeField] public int DamageFastTower { get; private set; } = 1;

        [field: SerializeField] public int MaxCastleLevel { get; private set; } = 10;
        [field: SerializeField] public int UpgradeCastleCost { get; private set; } = 10;
        [field: SerializeField, Min(1.1f)] public float UpgradeMultiplier { get; private set; } = 1.2f;
        [field: SerializeField, Min(1.1f)] public float CostMultiplier { get; private set; } = 1.2f;
        [field: SerializeField] public float RadiusRangeCastle { get; private set; } = 1;
        [field: SerializeField] public float StartDelayShootCastle { get; private set; } = 1;
        [field: SerializeField] public int StartDamageCastle { get; private set; } = 1;
        [field: SerializeField] public int StartHealthCastle { get; private set; } = 25;

        [field: SerializeField] public int MinEnemyPullSize { get; private set; } = 5;
        [field: SerializeField] public int MaxEnemyPullSize { get; private set; } = 15;
        [field: SerializeField] public int WavesDelay { get; private set; } = 15;
        [field: SerializeField] public float EnemySpawnDelay { get; private set; } = 1f;
        [field: SerializeField] public float EnemySpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float EnemyAttackDelay { get; private set; } = 1f;
        [field: SerializeField] public int EnemyDamage { get; private set; } = 1;
        [field: SerializeField] public int EnemyHealth { get; private set; } = 1;
        [field: SerializeField] public int EnemiesPerWave { get; private set; } = 5;
        [field: SerializeField] public float EnemyStopDistance { get; private set; } = 1f;
        [field: SerializeField, Min(1.1f)] public float MultiplierEnemiesPerWave { get; private set; } = 1.2f;

        [field: SerializeField] public float BulletSpeed { get; private set; } = 3f;
        [field: SerializeField] public float FindDelay { get; private set; } = 1.5f;
        [field: SerializeField] public int MinBulletPullSize { get; private set; } = 5;
        [field: SerializeField] public int MaxBulletPullSize { get; private set; } = 15;
    }
}