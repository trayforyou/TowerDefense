using _Project.Scripts.Savers;

namespace _Project.Scripts
{
    public class UpgradeHandler
    {
        private readonly MainUpgraderMenu _upgraderMenu;
        private readonly MetaLevelController _levelController;
        private readonly MainMenu _menu;

        private int _healthCastleLevel;
        private int _damageCastleLevel;
        private int _speedCastleLevel;
        private int _damageTowersLevel;
        private int _speedTowersLevel;

        public UpgradeHandler(MainMenu menu, MainUpgraderMenu upgraderMenu, MetaLevelController levelController)
        {
            _levelController = levelController;
            _upgraderMenu = upgraderMenu;
            _menu = menu;
            
            SubscribeAll();
        }


        private void SubscribeAll()
        {
            // _healthCastleLevel = dataLevels.CastleHealthLevel;  // TO DO нужно подписаться на все кнопки в левл контроллере
            // _damageCastleLevel = dataLevels.CastleDamageLevel;
            // _speedCastleLevel = dataLevels.CastleSpeedLevel;
            // _damageTowersLevel = dataLevels.TowersDamageLevel;
            // _speedTowersLevel = dataLevels.TowersSpeedLevel;
            //

            _levelController.OnLevelCastleSpeedChanged += ChangeLevelCastleHealth;

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
            _menu.OnClickUpgrade -= OpenUpgraderMenu;

            _upgraderMenu.OnCloseMenu -= CloseMenu;
            _upgraderMenu.OnUpCastleHealth -= UpCastleHealth;
            _upgraderMenu.OnUpCastleSpeed -= UpCastleSpeed;
            _upgraderMenu.OnUpCastleDamage -= UpCastleDamage;
            _upgraderMenu.OnUpTowersSpeed -= UpTowersSpeed;
            _upgraderMenu.OnUpTowersDamage -= UpTowersDamage;
        }

        private void ChangeLevelCastleHealth(int level)
        {
            // _upgraderMenu.SetLevel(level);
            // _upgraderMenu.SetGrowth();
        }

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