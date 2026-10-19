using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts
{
    public class MainGrowthCalculator
    {
        private readonly MainUpgradeConfig _config;

        public MainGrowthCalculator(MainUpgradeConfig config) => 
            _config = config;

        public float GetCastleHealthMultiply(int level) =>
            Mathf.Pow(_config.CastleHealthMultiplier, level);

        public float GetCastleSpeedMultiply(int level) =>
            Mathf.Pow(_config.CastleSpeedMultiplier, level);

        public float GetCastleDamageMultiply(int level) =>
            Mathf.Pow(_config.CastleDamageMultiplier, level);

        public float GetTowerSpeedMultiply(int level) =>
            Mathf.Pow(_config.TowerSpeedMultiplier, level);

        public float GetTowerDamageMultiply(int level) => 
            Mathf.Pow(_config.TowerDamageMultiplier, level);
    }
}