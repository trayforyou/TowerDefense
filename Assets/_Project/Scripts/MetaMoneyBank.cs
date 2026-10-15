using _Project.Scripts.Savers;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts
{
    public class MetaMoneyBank
    {
        private readonly Saver _saver;
        private readonly MoneyConfig _moneyConfig;

        public MetaMoneyBank(Saver saver, MoneyConfig moneyConfig)
        {
            _moneyConfig = moneyConfig;
            _saver = saver;
        }

        public int CalculateMoney(int waveCount, int enemiesDeath)
        {
            int reward = waveCount * _moneyConfig.MoneyPerWave + enemiesDeath * _moneyConfig.MoneyPerKill;

            AddMetaMoney(reward);

            return reward;
        }

        public int GetBalance()
        {
            SaveDataMoney dataMoney = _saver.LoadDataMoney();
            return dataMoney.MetaCurrency;
        }

        private void AddMetaMoney(int count)
        {
            SaveDataMoney dataMoney = _saver.LoadDataMoney();
            int metaMoney = dataMoney.MetaCurrency + count;
            _saver.SaveMoney(new SaveDataMoney(metaMoney));
        }
    }
}