using System;

namespace _Project.Scripts.Savers
{
    [Serializable]
    public class SaveDataLevels : ISaveData
    {
        public int CastleHealthLevel;
        public int CastleDamageLevel;
        public int CastleSpeedLevel;
        public int TowersDamageLevel;
        public int TowersSpeedLevel;

        public SaveDataLevels()
        {
            CastleHealthLevel = 0;
            CastleDamageLevel = 0;
            CastleSpeedLevel = 0;
            TowersDamageLevel = 0;
            TowersSpeedLevel = 0;
        }

        public SaveDataLevels(int castleHealthLevel, int castleDamageLevel, int castleSpeedLevel,
            int towersDamageLevel, int towersSpeedLevel)
        {
            CastleHealthLevel = castleHealthLevel;
            CastleDamageLevel = castleDamageLevel;
            CastleSpeedLevel = castleSpeedLevel;
            TowersDamageLevel = towersDamageLevel;
            TowersSpeedLevel = towersSpeedLevel;
        }
    }
}