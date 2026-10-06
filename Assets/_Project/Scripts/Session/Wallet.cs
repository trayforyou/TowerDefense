using System;

namespace _Project.Scripts.Session
{
    public class Wallet
    {
        public event Action<int> ValueChanged;
        
        public int Count { get; private set; }

        public void RefreshInfo() =>
            ValueChanged?.Invoke(Count);

        public void AddMoney(int count)
        {
            Count += count;

            ValueChanged?.Invoke(Count);
        }

        public bool TryTakeMoney(int count)
        {
            if (Count < count)
                return false;

            Count -= count;
            ValueChanged?.Invoke(Count);

            return true;
        }
    }
}