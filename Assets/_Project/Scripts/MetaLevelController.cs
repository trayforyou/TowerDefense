using System;
using _Project.Scripts.Savers;

namespace _Project.Scripts
{
    public class MetaLevelController
    {
        private readonly Saver _saver;

        private int _currentLevelCastleHealth;
        private int _currentLevelCastleSpeed;
        private int _currentLevelCastleDamage;
        private int _currentLevelTowerSpeed;
        private int _currentLevelTowerDamage;

        public event Action<int> OnLevelCastleHealthChanged;
        public event Action<int> OnLevelCastleSpeedChanged;
        public event Action<int> OnLevelCastleDamageChanged;
        public event Action<int> OnLevelTowerSpeedChanged;
        public event Action<int> OnLevelTowerDamageChanged;

        public MetaLevelController(Saver saver)
        {
            _saver = saver;

            LoadLevels();
            InvokeAllLevels();
        }

        public void InvokeAllLevels()
        {
            OnLevelCastleHealthChanged?.Invoke(_currentLevelCastleHealth);
            OnLevelCastleSpeedChanged?.Invoke(_currentLevelCastleSpeed);
            OnLevelCastleDamageChanged?.Invoke(_currentLevelCastleDamage);
            OnLevelTowerSpeedChanged?.Invoke(_currentLevelTowerSpeed);
            OnLevelTowerDamageChanged?.Invoke(_currentLevelTowerDamage);
        }

        public void UpCastleHealth()
        {
            _currentLevelCastleHealth++;
            OnLevelCastleHealthChanged?.Invoke(_currentLevelCastleHealth);
            RefreshData();
        }

        public void UpCastleSpeed()
        {
            _currentLevelCastleSpeed++;
            OnLevelCastleSpeedChanged?.Invoke(_currentLevelCastleSpeed);
            RefreshData();
        }

        public void UpCastleDamage()
        {
            _currentLevelCastleDamage++;
            OnLevelCastleDamageChanged?.Invoke(_currentLevelCastleDamage);
            RefreshData();
        }

        public void UpTowerSpeed()
        {
            _currentLevelTowerSpeed++;
            OnLevelTowerSpeedChanged?.Invoke(_currentLevelTowerSpeed);
            RefreshData();
        }

        public void UpTowerDamage()
        {
            _currentLevelTowerDamage++;
            OnLevelTowerDamageChanged?.Invoke(_currentLevelTowerDamage);
            RefreshData();
        }

        private void LoadLevels()
        {
            SaveDataLevels data = _saver.LoadDataLevels();

            _currentLevelCastleHealth = data.CastleHealthLevel;
            _currentLevelCastleSpeed = data.CastleSpeedLevel;
            _currentLevelCastleDamage = data.CastleDamageLevel;
            _currentLevelTowerSpeed = data.TowersSpeedLevel;
            _currentLevelTowerDamage = data.TowersDamageLevel;
        }

        private void RefreshData()
        {
            SaveDataLevels data = new SaveDataLevels(_currentLevelCastleHealth, _currentLevelCastleDamage,
                _currentLevelCastleSpeed, _currentLevelTowerDamage, _currentLevelTowerSpeed);
            _saver.SaveLevels(data);
        }
    }
}