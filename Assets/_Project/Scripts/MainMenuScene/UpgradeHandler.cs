using System;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.MainMenuScene
{
    public class UpgradeHandler : IDisposable
    {
        private readonly MainUpgraderMenu _upgraderMenu;
        private readonly MetaLevelController _levelController;
        private readonly MainGrowthCalculator _calculator;
        private readonly LevelConfig _config;
        private readonly MetaMoneyBank _bank;
        private readonly MainMenu _menu;

        private int _healthCastleLevel;
        private int _damageCastleLevel;
        private int _speedCastleLevel;
        private int _damageTowersLevel;
        private int _speedTowersLevel;

        public UpgradeHandler(MainMenu menu, MainUpgraderMenu upgraderMenu, MetaLevelController levelController,
            MainGrowthCalculator calculator, MetaMoneyBank bank, LevelConfig config)
        {
            _config = config;
            _bank = bank;
            _calculator = calculator;
            _levelController = levelController;
            _upgraderMenu = upgraderMenu;
            _menu = menu;

            SubscribeAll();
            ChangeOpportunitiesBuy(_bank.GetBalance()); 
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
            _bank.OnValueChanged += ChangeOpportunitiesBuy;
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
            _bank.OnValueChanged -= ChangeOpportunitiesBuy;
        }

        private void ChangeOpportunitiesBuy(int count)
        {
            _upgraderMenu.SetCanBuyCastleHealth(count >= _config.CastleHealthUpCost);
            _upgraderMenu.SetCanBuyCastleSpeed(count >= _config.CastleSpeedUpCost);
            _upgraderMenu.SetCanBuyCastleDamage(count >= _config.CastleDamageUpCost);
            _upgraderMenu.SetCanBuyTowerSpeed(count >= _config.TowerSpeedUpCost);
            _upgraderMenu.SetCanBuyTowerDamage(count >= _config.TowerDamageUpCost);
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

        private void UpTowersDamage()
        {
            if (_bank.TryGetMoney(_config.TowerDamageUpCost))
                _levelController.UpTowerDamage();
        }

        private void UpTowersSpeed()
        {
            if (_bank.TryGetMoney(_config.TowerSpeedUpCost))
                _levelController.UpTowerSpeed();
        }

        private void UpCastleDamage()
        {
            if (_bank.TryGetMoney(_config.CastleDamageUpCost))
                _levelController.UpCastleDamage();
        }

        private void UpCastleSpeed()
        {
            if (_bank.TryGetMoney(_config.CastleSpeedUpCost))
                _levelController.UpCastleSpeed();
        }

        private void UpCastleHealth()
        {
            if (_bank.TryGetMoney(_config.CastleHealthUpCost))
                _levelController.UpCastleHealth();
        }

        private void OpenUpgraderMenu()
        {
            _levelController.InvokeAllLevels();
            _upgraderMenu.Show();
        }

        private void CloseMenu() =>
            _upgraderMenu.Hide();
    }
}