using System;

namespace _Project.Scripts
{
    [Serializable]
    public struct LevelDataPair
    {
        public MetaUpgradeType UpgradeType;
        public int UpgradeLevel;

        public LevelDataPair(MetaUpgradeType  upgradeType, int upgradeLevel)
        {
            UpgradeLevel = upgradeLevel;
            UpgradeType = upgradeType;
        }
    }
}