using System;
using System.Collections.Generic;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts
{
    public class MainGrowthCalculator
    {
        private readonly LevelConfig _config;
        private Dictionary<MetaUpgradeType, UpgradeCalculateParameters> _multipliers;

        public MainGrowthCalculator(LevelConfig config)
        {
            _config = config;
            InitializeMultipliers();
        }

        public float GetMultiply(MetaUpgradeType type, int level)
        {
            if (!_multipliers.TryGetValue(type, out UpgradeCalculateParameters parameters))
                throw new ArgumentException($"В калькуляторе нет множителя для типа {type.ToString()}");

            if (Mathf.Approximately(parameters.MaxLimit, float.MaxValue))
                return Mathf.Pow(parameters.Multiplier, level);

            return Mathf.Max(Mathf.Pow(parameters.Multiplier, level), parameters.MaxLimit);
        }

        private void InitializeMultipliers()
        {
            _multipliers = new();

            _multipliers[MetaUpgradeType.CastleHealth] = new UpgradeCalculateParameters(_config.CastleHealthMultiplier);
            _multipliers[MetaUpgradeType.CastleDamage] = new UpgradeCalculateParameters(_config.CastleDamageMultiplier);
            _multipliers[MetaUpgradeType.TowerDamage] = new UpgradeCalculateParameters(_config.TowerDamageMultiplier);
            _multipliers[MetaUpgradeType.CastleSpeed] =
                new UpgradeCalculateParameters(_config.CastleSpeedMultiplier, _config.MaxCastleSpeed);
            _multipliers[MetaUpgradeType.TowerSpeed] =
                new UpgradeCalculateParameters(_config.TowerSpeedMultiplier, _config.MaxTowerSpeed);
        }
    }
}