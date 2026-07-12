using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Scriptable Objects/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Header("Коэффициенты наград")]
    [SerializeField] private int _moneyPerWave = 50;
    [SerializeField] private int _moneyPerKill = 10;

    [Header("Параметры строительства")]
    [SerializeField] private float _minDistanceForBuilding = 1f;
    [SerializeField] private int _fastTowerCost = 10;
    [SerializeField] private int _strongTowerCost = 10;

    [Header("Параметры башен")]
    [SerializeField] private float _radiusRangeFastTower = 1;
    [SerializeField] private float _radiusRangeStrongTower = 1;
    [SerializeField] private float _delayShootFastTower = 0.1f;
    [SerializeField] private float _delayShootStrongTower = 0.8f;
    [SerializeField] private int _damageStrongTower = 3;
    [SerializeField] private int _damageFastTower = 1;

    [Header("Параметры прокачки замка")]
    [SerializeField, Min(1)] private float _upgradeMultiplier = 1.2f;
    [SerializeField, Min(1)] private float _costMultiplier = 1.2f;
    [SerializeField] private float _radiusRangeCastle = 1;
    [SerializeField] private float _startDelayShootCastle = 1;
    [SerializeField] private int _startDamageCastle = 1;
    [SerializeField] private int _startHealthCastle = 25;
    [SerializeField] private int _maxCastleLevel = 10;
    [SerializeField] private int _upgradeCastleCost = 10;
    [SerializeField] private float _bulletSpeed = 3f;

    [Header("Параметры врагов")]
    [SerializeField] private float _enemySpeed = 0.5f;
    [SerializeField] private float _enemyAttackDelay = 1f;
    [SerializeField] private float _enemySpawnDelay = 1f;
    [SerializeField] private float _enemyStopDistance = 1f;
    [SerializeField] private int _enemyDamage = 1;
    [SerializeField] private int _enemyHealth = 1;
    [SerializeField] private int _costEnemyDeath = 10;
    [SerializeField] private int _enemiesPerWave = 5;
    [SerializeField] private float _multiplierEnemiesPerWave = 1.2f;
    [SerializeField] private int _wavesDelay = 15;

    public int WavesDelay => _wavesDelay;
    public float EnemySpawnDelay => _enemySpawnDelay;
    public float EnemySpeed => _enemySpeed;
    public float EnemyAttackDelay => _enemyAttackDelay;
    public int EnemyDamage => _enemyDamage;
    public int EnemyHealth => _enemyHealth;
    public int CostEnemyDeath => _costEnemyDeath;
    public int EnemiesPerWave => _enemiesPerWave;
    public float EnemyStopDistance => _enemyStopDistance;
    public float MultiplierEnemiesPerWave => _multiplierEnemiesPerWave;
    public int MaxCastleLevel => _maxCastleLevel;
    public int UpgradeCastleCost => _upgradeCastleCost;
    public float BulletSpeed => _bulletSpeed;
    public float UpgradeMultiplier => _upgradeMultiplier;
    public float CostMultiplier => _costMultiplier;
    public float RadiusRangeCastle => _radiusRangeCastle;
    public float StartDelayShootCastle => _startDelayShootCastle;
    public int StartDamageCastle => _startDamageCastle;
    public int StartHealthCastle => _startHealthCastle;
    public int MoneyPerWave => _moneyPerWave;
    public int MoneyPerKill => _moneyPerKill;
    public float MinDistanceForBuilding => _minDistanceForBuilding;
    public int FastTowerCost => _fastTowerCost;
    public int StrongTowerCost => _strongTowerCost;
    public float RadiusRangeFastTower => _radiusRangeFastTower;
    public float RadiusRangeStrongTower => _radiusRangeStrongTower;
    public int DamageStrongTower => _damageStrongTower;
    public int DamageFastTower => _damageFastTower;
    public float DelayShootFastTower => _delayShootFastTower;
    public float DelayShootStrongTower => _delayShootStrongTower;
}