using System;

namespace _Project.Scripts.Savers
{
    [Serializable]
    public class SaveData
    {
        public int MetaCurrency;

        public SaveData(int metaCurrency) => 
            MetaCurrency = metaCurrency;
    }
}