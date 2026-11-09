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
            _levelController.OnLevelChanged += ChangeLevel;
            _menu.OnClickUpgrade += OpenUpgraderMenu;
            _upgraderMenu.OnClickCloseMenu += CloseMenu;
            _upgraderMenu.OnClickUpgrade += Upgrade;
            _bank.OnValueChanged += ChangeOpportunitiesBuy;
        }

        private void UnSubscribeAll()
        {
            _levelController.OnLevelChanged -= ChangeLevel;
            _menu.OnClickUpgrade -= OpenUpgraderMenu;
            _upgraderMenu.OnClickCloseMenu -= CloseMenu;
            _upgraderMenu.OnClickUpgrade -= Upgrade;
            _bank.OnValueChanged -= ChangeOpportunitiesBuy;
        }

        private void ChangeOpportunitiesBuy(int count)
        {
            _upgraderMenu.SetCanBuy(MetaUpgradeType.CastleHealth, count >= _config.CastleHealthUpCost);
            _upgraderMenu.SetCanBuy(MetaUpgradeType.CastleSpeed, count >= _config.CastleSpeedUpCost);
            _upgraderMenu.SetCanBuy(MetaUpgradeType.CastleDamage, count >= _config.CastleDamageUpCost);
            _upgraderMenu.SetCanBuy(MetaUpgradeType.TowerSpeed, count >= _config.TowerSpeedUpCost);
            _upgraderMenu.SetCanBuy(MetaUpgradeType.TowerDamage, count >= _config.TowerDamageUpCost);
        }

        private void ChangeLevel(MetaUpgradeType type, int level) =>
            _upgraderMenu.SetLevel(type, level, _calculator.GetMultiply(type, level));

        private void Upgrade(MetaUpgradeType type)
        {
            int cost = type switch
            {
                MetaUpgradeType.CastleHealth => _config.CastleHealthUpCost,
                MetaUpgradeType.CastleDamage => _config.CastleDamageUpCost,
                MetaUpgradeType.CastleSpeed => _config.CastleSpeedUpCost,
                MetaUpgradeType.TowerDamage => _config.TowerDamageUpCost,
                MetaUpgradeType.TowerSpeed => _config.TowerSpeedUpCost,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            if (_bank.TryGetMoney(cost))
                _levelController.UpLevel(type);
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