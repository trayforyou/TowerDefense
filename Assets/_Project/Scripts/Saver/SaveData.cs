using System;

namespace TowerDefense.Saver
{
    [Serializable]
    public class SaveData
    {
        public int MetaCurrency;

        public SaveData(int metaCurrency) => 
            MetaCurrency = metaCurrency;
    }
}