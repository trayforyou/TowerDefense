using TowerDefense.ScriptableObjects;
using TowerDefense.Session;
using UnityEngine;

namespace TowerDefense.Builds.Castle
{
    public class CastleUpper : MonoBehaviour
    {
        [SerializeField] private UpgradeMenu _upgradeMenu;

        private Wallet _wallet;
        private Castle _castle;
        private GameConfig _config;
        private int _castleHealthLevel = 1;
        private int _castleSpeedLevel = 1;
        private int _castleStrongLevel = 1;
        private int _currentCostUpgradeSpeed;
        private int _currentCostUpgradeForce;
        private int _currentCostUpgradeHealth;

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
            SubscribeAll();
        }

        public void Activate() =>
            _upgradeMenu.Show();

        private void ChangeOpportunitiesBuy(int count)
        {
            _upgradeMenu.SetCanUpHealth(count >= _currentCostUpgradeHealth);
            _upgradeMenu.SetCanUpSpeed(count >= _currentCostUpgradeSpeed);
            _upgradeMenu.SetCanUpStrong(count >= _currentCostUpgradeForce);
        }

        private void SubscribeAll()
        {
            _upgradeMenu.TriedUpHealth += UpCastleHealth;
            _upgradeMenu.TriedUpStrong += UpCastleStrong;
            _upgradeMenu.TriedUpSpeed += UpCastleSpeed;
            _wallet.ValueChanged += ChangeOpportunitiesBuy;
        }

        private void UnSubscribeAll()
        {
            _upgradeMenu.TriedUpHealth -= UpCastleHealth;
            _upgradeMenu.TriedUpStrong -= UpCastleStrong;
            _upgradeMenu.TriedUpSpeed -= UpCastleSpeed;
            _wallet.ValueChanged -= ChangeOpportunitiesBuy;
        }

        private void UpCastleHealth()
        {
            if (_castleHealthLevel > _config.MaxCastleLevel)
                return;

            if (_wallet.TryTakeMoneys(_currentCostUpgradeHealth))
            {
                _castle.UpHealth();
                _currentCostUpgradeHealth = (int)(_currentCostUpgradeHealth * _config.CostMultiplier);
                _upgradeMenu.ChangeCostUpgradeHealth(_currentCostUpgradeHealth);
                _castleHealthLevel++;
            }

            _upgradeMenu.Hide();
        }

        private void UpCastleStrong()
        {
            if (_castleStrongLevel > _config.MaxCastleLevel)
                return;

            if (_wallet.TryTakeMoneys(_currentCostUpgradeForce))
            {
                _castle.UpStrong();
                _currentCostUpgradeForce = (int)(_currentCostUpgradeForce * _config.CostMultiplier);
                _upgradeMenu.ChangeCostUpgradeStrong(_currentCostUpgradeForce);
                _castleStrongLevel++;
            }

            _upgradeMenu.Hide();
        }

        private void UpCastleSpeed()
        {
            if (_castleSpeedLevel > _config.MaxCastleLevel)
                return;

            if (_wallet.TryTakeMoneys(_currentCostUpgradeSpeed))
            {
                _castle.UpSpeed();
                _currentCostUpgradeSpeed = (int)(_currentCostUpgradeSpeed * _config.CostMultiplier);
                _upgradeMenu.ChangeCostUpgradeSpeed(_currentCostUpgradeSpeed);
                _castleSpeedLevel++;
            }

            _upgradeMenu.Hide();
        }
    }
}