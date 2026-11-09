using System;
using System.Collections.Generic;

namespace _Project.Scripts.Savers
{
    [Serializable]
    public class SaveDataLevels : ISaveData
    {
        public List<LevelDataPair> Levels;

        public SaveDataLevels()
        {
            Levels = new();

            foreach (MetaUpgradeType type in Enum.GetValues(typeof(MetaUpgradeType)))
                Levels.Add(new LevelDataPair(type, 0));
        }

        public int GetLevel(MetaUpgradeType upgradeType)
        {
            foreach (LevelDataPair pair in Levels)
            {
                if (pair.UpgradeType == upgradeType)
                    return pair.UpgradeLevel;
            }
            
            throw new Exception($"Level {upgradeType} not found");
        }

        public SaveDataLevels(Dictionary<MetaUpgradeType, int> upgrades)
        {
            Levels = new();

            foreach (var pair in upgrades)
                Levels.Add(new LevelDataPair(pair.Key, pair.Value));
        }
    }
}