using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts
{
    public class MainGrowthCalculator
    {
        private readonly LevelConfig _config;

        public MainGrowthCalculator(LevelConfig config) =>
            _config = config;

        public float GetCastleHealthMultiply(int level) =>
            Mathf.Pow(_config.CastleHealthMultiplier, level);

        public float GetCastleSpeedMultiply(int level) =>
            Mathf.Max(Mathf.Pow(_config.CastleSpeedMultiplier, level), _config.MaxCastleSpeed);

        public float GetCastleDamageMultiply(int level) =>
            Mathf.Pow(_config.CastleDamageMultiplier, level);

        public float GetTowerSpeedMultiply(int level) =>
            Mathf.Max(Mathf.Pow(_config.TowerSpeedMultiplier, level), _config.MaxTowerSpeed);

        public float GetTowerDamageMultiply(int level) =>
            Mathf.Pow(_config.TowerDamageMultiplier, level);
    }
}