using System;
using _Project.Scripts.Session;

namespace _Project.Scripts.Builds.Castles
{
    public class CastleUpHandler : IDisposable
    {
        private readonly UpgradeMenu _upgradeMenu;
        private readonly CastleUpper _castleUpper;
        private readonly Wallet _wallet;

        public bool IsActive => _upgradeMenu.IsActive;

        public void TurnOff() =>
            _upgradeMenu.Hide();

        public CastleUpHandler(UpgradeMenu upgradeMenu, CastleUpper castleUpper, Wallet wallet)
        {
            _wallet = wallet;
            _upgradeMenu = upgradeMenu;
            _castleUpper = castleUpper;
            
            _upgradeMenu.SetStartCost(castleUpper.UpgradeCastleCost);
            
            SubscribeAll();
        }

        public void Activate() =>
            _upgradeMenu.Show();

        public void Dispose() =>
            UnSubscribeAll();

        private void ChangeOpportunitiesBuy(int count)
        {
            _upgradeMenu.SetCanUpHealth(count >= _castleUpper.GetHealthUpgradeCost());
            _upgradeMenu.SetCanUpSpeed(count >= _castleUpper.GetSpeedUpgradeCost());
            _upgradeMenu.SetCanUpForce(count >= _castleUpper.GetForceUpgradeCost());
        }

        private void SubscribeAll()
        {
            _castleUpper.OnUpgradeComplete += TurnOff;
            _upgradeMenu.TriedUpHealth += UpCastleHealth;
            _upgradeMenu.TriedUpForce += UpCastleForce;
            _upgradeMenu.TriedUpSpeed += UpCastleSpeed;
            _wallet.ValueChanged += ChangeOpportunitiesBuy;
        }

        private void UnSubscribeAll()
        {
            _castleUpper.OnUpgradeComplete -= TurnOff;
            _upgradeMenu.TriedUpHealth -= UpCastleHealth;
            _upgradeMenu.TriedUpForce -= UpCastleForce;
            _upgradeMenu.TriedUpSpeed -= UpCastleSpeed;
            _wallet.ValueChanged -= ChangeOpportunitiesBuy;
        }

        private void UpCastleHealth() =>
            _castleUpper.UpCastleHealth();

        private void UpCastleForce() =>
            _castleUpper.UpCastleForce();

        private void UpCastleSpeed() =>
            _castleUpper.UpCastleSpeed();
    }
}