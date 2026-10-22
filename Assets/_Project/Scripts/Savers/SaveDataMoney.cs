using System;

namespace _Project.Scripts.Savers
{
    [Serializable]
    public class SaveDataMoney : ISaveData
    {
        public int MetaCurrency;

        public SaveDataMoney() =>
            MetaCurrency = 0;

        public SaveDataMoney(int metaCurrency) =>
            MetaCurrency = metaCurrency;
    }
}