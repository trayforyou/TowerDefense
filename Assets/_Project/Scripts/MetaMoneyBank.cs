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

        private void AddMetaMoney(int count)
        {
            SaveData data = _saver.Load();
            int metaMoney = data.MetaCurrency + count;
            _saver.Save(new SaveData(metaMoney));
        }
    }
}