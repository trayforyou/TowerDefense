using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;
using UnityEngine;

namespace _Project.Scripts.Builds.Castles
{
    public class CastleUpper : MonoBehaviour
    {
        [SerializeField] private UpgradeMenu _upgradeMenu;

        private Wallet _wallet;
        private Castle _castle;
        private CastleConfig _config;
        private Upgrade _healthUpgrade;
        private Upgrade _forceUpgrade;
        private Upgrade _speedUpgrade;
        private float _currentDelay;
        private int _currentDamage;

        public bool IsActive => _upgradeMenu.IsActive;

        private void OnDestroy() =>
            UnSubscribeAll();

        public void TurnOff() =>
            _upgradeMenu.Hide();

        public void Initialize(CastleConfig config, Wallet wallet, Castle castle)
        {
            _config = config;
            _wallet = wallet;
            _castle = castle;
            _currentDelay = config.StartDelayShootCastle;
            _currentDamage = config.StartDamageCastle;

            _healthUpgrade = new Upgrade(config.UpgradeCastleCost, config.MaxCastleLevel, config.CostMultiplier,
                () => _castle.UpHealth(),
                cost => _upgradeMenu.ChangeCostUpgradeHealth(cost));

            _forceUpgrade = new Upgrade(config.UpgradeCastleCost, config.MaxCastleLevel, config.CostMultiplier,
                () =>
                {
                    int tempDamage = _currentDamage;
                    _currentDamage = (int)(_currentDamage * _config.UpgradeMultiplier);
                    if (tempDamage == _currentDamage)
                        _currentDamage++;
                    _castle.UpForce(_currentDamage);
                },
                cost => _upgradeMenu.ChangeCostUpgradeForce(cost));

            _speedUpgrade = new Upgrade(config.UpgradeCastleCost, config.MaxCastleLevel, config.CostMultiplier,
                () =>
                {
                    _currentDelay /= _config.UpgradeMultiplier;
                    _castle.UpSpeed(_currentDelay);
                },
                cost => _upgradeMenu.ChangeCostUpgradeSpeed(cost));

            _upgradeMenu.SetStartCost(config.UpgradeCastleCost);

            SubscribeAll();
        }

        public void Activate() =>
            _upgradeMenu.Show();

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