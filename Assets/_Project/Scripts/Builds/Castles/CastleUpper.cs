using System;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;

namespace _Project.Scripts.Builds.Castles
{
    public class CastleUpper : IDisposable
    {
        private readonly UpgradeMenu _upgradeMenu;
        private readonly Upgrade _healthUpgrade;
        private readonly Upgrade _forceUpgrade;
        private readonly Upgrade _speedUpgrade;
        private readonly Wallet _wallet;

        public bool IsActive => _upgradeMenu.IsActive;

        public void TurnOff() =>
            _upgradeMenu.Hide();

        public CastleUpper(CastleConfig castleConfig, Wallet wallet, Castle castle, UpgradeMenu upgradeMenu)
        {
            _upgradeMenu = upgradeMenu;
            _wallet = wallet;
            var currentDelay = castleConfig.StartDelayShootCastle;
            var currentDamage = castleConfig.StartDamageCastle;

            _healthUpgrade = new Upgrade(castleConfig.UpgradeCastleCost, castleConfig.MaxCastleLevel,
                castleConfig.CostMultiplier,
                () => castle.UpHealth(),
                cost => _upgradeMenu.ChangeCostUpgradeHealth(cost));

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
                cost => _upgradeMenu.ChangeCostUpgradeForce(cost));

            _speedUpgrade = new Upgrade(castleConfig.UpgradeCastleCost, castleConfig.MaxCastleLevel,
                castleConfig.CostMultiplier,
                () =>
                {
                    currentDelay /= castleConfig.UpgradeMultiplier;
                    castle.UpSpeed(currentDelay);
                },
                cost => _upgradeMenu.ChangeCostUpgradeSpeed(cost));

            _upgradeMenu.SetStartCost(castleConfig.UpgradeCastleCost);

            SubscribeAll();
        }

        public void Activate() =>
            _upgradeMenu.Show();
        
        public void Dispose() => 
            UnSubscribeAll();

        private void ChangeOpportunitiesBuy(int count)
        {
            _upgradeMenu.SetCanUpHealth(count >= _healthUpgrade.CurrentCost);
            _upgradeMenu.SetCanUpSpeed(count >= _speedUpgrade.CurrentCost);
            _upgradeMenu.SetCanUpForce(count >= _forceUpgrade.CurrentCost);
        }

        private void SubscribeAll()
        {
            _upgradeMenu.TriedUpHealth += UpCastleHealth;
            _upgradeMenu.TriedUpForce += UpCastleForce;
            _upgradeMenu.TriedUpSpeed += UpCastleSpeed;
            _wallet.ValueChanged += ChangeOpportunitiesBuy;
        }

        private void UnSubscribeAll()
        {
            _upgradeMenu.TriedUpHealth -= UpCastleHealth;
            _upgradeMenu.TriedUpForce -= UpCastleForce;
            _upgradeMenu.TriedUpSpeed -= UpCastleSpeed;
            _wallet.ValueChanged -= ChangeOpportunitiesBuy;
        }

        private void ProcessUpgrade(Upgrade upgrade)
        {
            upgrade.TryUpgrade(_wallet);
            _upgradeMenu.Hide();
        }

        private void UpCastleHealth() =>
            ProcessUpgrade(_healthUpgrade);

        private void UpCastleForce() =>
            ProcessUpgrade(_forceUpgrade);

        private void UpCastleSpeed() =>
            ProcessUpgrade(_speedUpgrade);
    }
}