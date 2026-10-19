using System;

namespace _Project.Scripts
{
    public class UpgradeHandler : IDisposable
    {
        private readonly MainUpgraderMenu _upgraderMenu;
        private readonly MetaLevelController _levelController;
        private readonly MainGrowthCalculator _calculator;
        private readonly MainMenu _menu;

        private int _healthCastleLevel;
        private int _damageCastleLevel;
        private int _speedCastleLevel;
        private int _damageTowersLevel;
        private int _speedTowersLevel;

        public UpgradeHandler(MainMenu menu, MainUpgraderMenu upgraderMenu, MetaLevelController levelController,
            MainGrowthCalculator calculator)
        {
            _calculator = calculator;
            _levelController = levelController;
            _upgraderMenu = upgraderMenu;
            _menu = menu;

            SubscribeAll();
        }

        public void Dispose() =>
            UnSubscribeAll();

        private void SubscribeAll()
        {
            _levelController.OnLevelCastleHealthChanged += ChangeLevelCastleHealth;
            _levelController.OnLevelCastleSpeedChanged += ChangeLevelCastleSpeed;
            _levelController.OnLevelCastleDamageChanged += ChangeLevelCastleDamage;
            _levelController.OnLevelTowerSpeedChanged += ChangeLevelTowerSpeed;
            _levelController.OnLevelTowerDamageChanged += ChangeLevelTowerDamage;

            _menu.OnClickUpgrade += OpenUpgraderMenu;

            _upgraderMenu.OnCloseMenu += CloseMenu;
            _upgraderMenu.OnUpCastleHealth += UpCastleHealth;
            _upgraderMenu.OnUpCastleSpeed += UpCastleSpeed;
            _upgraderMenu.OnUpCastleDamage += UpCastleDamage;
            _upgraderMenu.OnUpTowersSpeed += UpTowersSpeed;
            _upgraderMenu.OnUpTowersDamage += UpTowersDamage;
        }

        private void UnSubscribeAll()
        {
            _levelController.OnLevelCastleHealthChanged -= ChangeLevelCastleHealth;
            _levelController.OnLevelCastleSpeedChanged -= ChangeLevelCastleSpeed;
            _levelController.OnLevelCastleDamageChanged -= ChangeLevelCastleDamage;
            _levelController.OnLevelTowerSpeedChanged -= ChangeLevelTowerSpeed;
            _levelController.OnLevelTowerDamageChanged -= ChangeLevelTowerDamage;

            _menu.OnClickUpgrade -= OpenUpgraderMenu;

            _upgraderMenu.OnCloseMenu -= CloseMenu;
            _upgraderMenu.OnUpCastleHealth -= UpCastleHealth;
            _upgraderMenu.OnUpCastleSpeed -= UpCastleSpeed;
            _upgraderMenu.OnUpCastleDamage -= UpCastleDamage;
            _upgraderMenu.OnUpTowersSpeed -= UpTowersSpeed;
            _upgraderMenu.OnUpTowersDamage -= UpTowersDamage;
        }

        private void ChangeLevelCastleHealth(int level) =>
            _upgraderMenu.SetCastleHealthLevel(level, _calculator.GetCastleHealthMultiply(level));

        private void ChangeLevelCastleSpeed(int level) => 
            _upgraderMenu.SetCastleSpeedLevel(level, _calculator.GetCastleSpeedMultiply(level));

        private void ChangeLevelCastleDamage(int level) => 
            _upgraderMenu.SetCastleDamageLevel(level, _calculator.GetCastleDamageMultiply(level));

        private void ChangeLevelTowerSpeed(int level) => 
            _upgraderMenu.SetTowerSpeedLevel(level, _calculator.GetTowerSpeedMultiply(level));

        private void ChangeLevelTowerDamage(int level) => 
            _upgraderMenu.SetTowerDamageLevel(level, _calculator.GetTowerDamageMultiply(level));

        private void UpTowersDamage() =>
            _levelController.UpTowerDamage();

        private void UpTowersSpeed() =>
            _levelController.UpTowerSpeed();

        private void UpCastleDamage() =>
            _levelController.UpCastleDamage();

        private void UpCastleSpeed() =>
            _levelController.UpCastleSpeed();

        private void UpCastleHealth() =>
            _levelController.UpCastleHealth();

        private void OpenUpgraderMenu() =>
            _upgraderMenu.Show();

        private void CloseMenu() =>
            _upgraderMenu.Hide();
    }
}