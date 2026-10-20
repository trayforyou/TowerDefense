using System;
using _Project.Scripts.Savers;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts
{
    public class MetaMoneyBank
    {
        private readonly Saver _saver;
        private readonly MoneyConfig _moneyConfig;

        private int _currentValue;
        private bool _isInitializedConfig = false;

        public event Action<int> OnValueChanged;

        public MetaMoneyBank(Saver saver, MoneyConfig moneyConfig) : this(saver)
        {
            _isInitializedConfig = true;
            _moneyConfig = moneyConfig;
        }

        public MetaMoneyBank(Saver saver) =>
            _saver = saver;

        public int CalculateMoney(int waveCount, int enemiesDeath)
        {
            if (!_isInitializedConfig)
                return 0;

            if (waveCount < 0 || enemiesDeath < 0)
                throw new ArgumentOutOfRangeException("waveCount", "waveCount and enemiesDeath must be nonnegative");

            int reward = waveCount * _moneyConfig.MoneyPerWave + enemiesDeath * _moneyConfig.MoneyPerKill;
            ChangeMoney(reward);

            return reward;
        }

        public int GetBalance()
        {
            SaveDataMoney dataMoney = _saver.LoadDataMoney();
            _currentValue = dataMoney.MetaCurrency;

            return dataMoney.MetaCurrency;
        }

        public bool TryGetMoney(int count)
        {
            if (_currentValue < count || count <= 0)
                return false;

            ChangeMoney(-count);
            return true;
        }

        private void ChangeMoney(int count)
        {
            SaveDataMoney dataMoney = _saver.LoadDataMoney();
            int metaMoney = dataMoney.MetaCurrency + count;
            _currentValue = metaMoney;
            _saver.SaveMoney(new SaveDataMoney(metaMoney));
            OnValueChanged?.Invoke(metaMoney);
        }
    }
}