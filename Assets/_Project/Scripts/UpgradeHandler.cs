using _Project.Scripts.Savers;

namespace _Project.Scripts
{
    public class UpgradeHandler
    {
        private readonly MainUpgraderMenu _upgraderMenu;

        private MainMenu _menu;
        private int _healthCastleLevel;
        private int _damageCastleLevel;
        private int _speedCastleLevel;
        private int _damageTowersLevel;
        private int _speedTowersLevel;

        public UpgradeHandler(MainMenu menu, MainUpgraderMenu upgraderMenu, SaveDataLevels dataLevels)
        {
            _menu = menu;
            _upgraderMenu = upgraderMenu;

            _healthCastleLevel = dataLevels.CastleHealthLevel;
            _damageCastleLevel = dataLevels.CastleDamageLevel;
            _speedCastleLevel = dataLevels.CastleSpeedLevel;
            _damageTowersLevel = dataLevels.TowersDamageLevel;
            _speedTowersLevel = dataLevels.TowersSpeedLevel;

            SetValue();
            
            SubscribeAll();
        }

        private void SetValue()
        {
            _upgraderMenu.SetValues(_healthCastleLevel, _damageCastleLevel, _speedCastleLevel, _damageTowersLevel,
                _speedTowersLevel);
        }
        
        private void SubscribeAll()
        {
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

        private void UpTowersDamage()
        {
            throw new System.NotImplementedException();
        }

        private void UpTowersSpeed()
        {
            throw new System.NotImplementedException();
        }

        private void UpCastleDamage()
        {
            throw new System.NotImplementedException();
        }

        private void UpCastleSpeed()
        {
            throw new System.NotImplementedException();
        }

        private void UpCastleHealth()
        {
            throw new System.NotImplementedException();
        }

        private void OpenUpgraderMenu()
        {
            _upgraderMenu.Show();
        }

        private void CloseMenu()
        {
            _upgraderMenu.Hide();
        }
    }
}