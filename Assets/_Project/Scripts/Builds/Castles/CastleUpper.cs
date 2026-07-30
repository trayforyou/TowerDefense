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
        private GameConfig _config;
        private int _castleHealthLevel = 1;
        private int _castleSpeedLevel = 1;
        private int _castleForceLevel = 1;
        private int _currentCostUpgradeSpeed;
        private int _currentCostUpgradeForce;
        private int _currentCostUpgradeHealth;
        private float _currentDelay;
        private int _currentDamage;

        public bool IsActive => _upgradeMenu.IsActive;

        private void OnDestroy() =>
            UnSubscribeAll();

        public void TurnOff() =>
            _upgradeMenu.Hide();

        public void Initialize(GameConfig config, Wallet wallet, Castle castle)
        {
            _config = config;
            _wallet = wallet;
            _castle = castle;

            _upgradeMenu.SetStartCost(_config.UpgradeCastleCost);
            _currentCostUpgradeSpeed = _config.UpgradeCastleCost;
            _currentCostUpgradeForce = _config.UpgradeCastleCost;
            _currentCostUpgradeHealth = _config.UpgradeCastleCost;
            _currentDelay = config.StartDelayShootCastle;
            _currentDamage = config.StartDamageCastle;
            SubscribeAll();
        }

        public void Activate() =>
            _upgradeMenu.Show();

        private void ChangeOpportunitiesBuy(int count)
        {
            _upgradeMenu.SetCanUpHealth(count >= _currentCostUpgradeHealth);
            _upgradeMenu.SetCanUpSpeed(count >= _currentCostUpgradeSpeed);
            _upgradeMenu.SetCanUpForce(count >= _currentCostUpgradeForce);
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

        private void UpCastleHealth()
        {
            if (_castleHealthLevel > _config.MaxCastleLevel)
                return;

            if (_wallet.TryTakeMoney(_currentCostUpgradeHealth))
            {
                _castle.UpHealth();
                _currentCostUpgradeHealth = (int)(_currentCostUpgradeHealth * _config.CostMultiplier);
                _upgradeMenu.ChangeCostUpgradeHealth(_currentCostUpgradeHealth);
                _castleHealthLevel++;
            }

            _upgradeMenu.Hide();
        }

        private void UpCastleForce()
        {
            if (_castleForceLevel > _config.MaxCastleLevel)
                return;

            if (_wallet.TryTakeMoney(_currentCostUpgradeForce))
            {
                int tempDamage = _currentDamage;
                _currentDamage = (int)(_currentDamage * _config.UpgradeMultiplier);

                if (tempDamage == _currentDamage)
                    _currentDamage++;

                _castle.UpForce(_currentDamage);
                _currentCostUpgradeForce = (int)(_currentCostUpgradeForce * _config.CostMultiplier);
                _upgradeMenu.ChangeCostUpgradeForce(_currentCostUpgradeForce);
                _castleForceLevel++;
            }

            _upgradeMenu.Hide();
        }

        private void UpCastleSpeed()
        {
            if (_castleSpeedLevel > _config.MaxCastleLevel)
                return;

            if (_wallet.TryTakeMoney(_currentCostUpgradeSpeed))
            {
                _currentDelay /= _config.UpgradeMultiplier;
                _castle.UpSpeed(_currentDelay);
                _currentCostUpgradeSpeed = (int)(_currentCostUpgradeSpeed * _config.CostMultiplier);
                _upgradeMenu.ChangeCostUpgradeSpeed(_currentCostUpgradeSpeed);
                _castleSpeedLevel++;
            }

            _upgradeMenu.Hide();
        }
    }
}