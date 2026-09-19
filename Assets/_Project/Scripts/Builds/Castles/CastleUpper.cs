using System;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;

namespace _Project.Scripts.Builds.Castles
{
    public class CastleUpper
    {
        private readonly Upgrade _healthUpgrade;
        private readonly Upgrade _forceUpgrade;
        private readonly Upgrade _speedUpgrade;
        private readonly Wallet _wallet;
        private readonly CastleConfig _castleConfig;

        public event Action OnUpgradeComplete;

        public int UpgradeCastleCost => _castleConfig.UpgradeCastleCost;
        
        public CastleUpper(CastleConfig castleConfig, Wallet wallet, Castle castle, Action<int> changeCostUpgradeHealth,
            Action<int> changeCostUpgradeForce, Action<int> changeCostUpgradeSpeed)
        {
            _wallet = wallet;
            _castleConfig = castleConfig;
            var currentDelay = castleConfig.StartDelayShootCastle;
            var currentDamage = castleConfig.StartDamageCastle;

            _healthUpgrade = new Upgrade(castleConfig.UpgradeCastleCost, castleConfig.MaxCastleLevel,
                castleConfig.CostMultiplier,
                castle.UpHealth,
                cost => changeCostUpgradeHealth?.Invoke(cost));

            _forceUpgrade = new Upgrade(castleConfig.UpgradeCastleCost, castleConfig.MaxCastleLevel,
                castleConfig.CostMultiplier,
                () =>
                {
                    int tempDamage = currentDamage;
                    currentDamage = (int)(currentDamage * castleConfig.UpgradeMultiplier);
                    if (tempDamage == currentDamage)
                        currentDamage++;
                    castle.UpForce(currentDamage);
                },
                cost => changeCostUpgradeForce?.Invoke(cost));

            _speedUpgrade = new Upgrade(castleConfig.UpgradeCastleCost, castleConfig.MaxCastleLevel,
                castleConfig.CostMultiplier,
                () =>
                {
                    currentDelay /= castleConfig.UpgradeMultiplier;
                    castle.UpSpeed(currentDelay);
                },
                cost => changeCostUpgradeSpeed?.Invoke(cost));
        }

        public int GetHealthUpgradeCost() => 
            _healthUpgrade.CurrentCost;
        
        public int GetForceUpgradeCost() => 
            _forceUpgrade.CurrentCost;

        public int GetSpeedUpgradeCost() => 
            _speedUpgrade.CurrentCost;

        public void UpCastleHealth() =>
            ProcessUpgrade(_healthUpgrade);

        public void UpCastleForce() =>
            ProcessUpgrade(_forceUpgrade);

        public void UpCastleSpeed() =>
            ProcessUpgrade(_speedUpgrade);
        
        private void ProcessUpgrade(Upgrade upgrade)
        {
            upgrade.TryUpgrade(_wallet);
            OnUpgradeComplete?.Invoke();
        }
    }
}