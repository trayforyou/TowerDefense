using System;
using System.Collections.Generic;
using _Project.Scripts.Savers;

namespace _Project.Scripts
{
    public delegate void LevelChangedHandler(MetaUpgradeType upgradeType, int level);

    public class MetaLevelController
    {
        private readonly Saver _saver;

        private Dictionary<MetaUpgradeType, int> _currentLevels;

        public event LevelChangedHandler OnLevelChanged;

        public MetaLevelController(Saver saver)
        {
            _saver = saver;
            _currentLevels = new();

            foreach (MetaUpgradeType type in Enum.GetValues(typeof(MetaUpgradeType)))
                _currentLevels[type] = 0;

            LoadLevels();
            InvokeAllLevels();
        }

        public void InvokeAllLevels()
        {
            foreach (var pair in _currentLevels)
                OnLevelChanged?.Invoke(pair.Key, pair.Value);
        }

        public void UpLevel(MetaUpgradeType upgradeType)
        {
            if (_currentLevels.ContainsKey(upgradeType) == false)
                throw new ArgumentException($"There is no such upgrade type: {upgradeType}");

            _currentLevels[upgradeType]++;
            OnLevelChanged?.Invoke(upgradeType, _currentLevels[upgradeType]);
            RefreshData();
        }

        private void LoadLevels()
        {
            SaveDataLevels data = _saver.LoadDataLevels();

            foreach (MetaUpgradeType type in Enum.GetValues(typeof(MetaUpgradeType))) 
                _currentLevels[type] = data.GetLevel(type);
        }

        private void RefreshData()
        {
            SaveDataLevels data = new SaveDataLevels(_currentLevels);
            _saver.SaveLevels(data);
        }
    }
}