using System;

namespace _Project.Scripts.Session
{
    public class Wallet
    {
        private int _count;
        
        public event Action<int> ValueChanged;

        public void AddMoneys(int count)
        {
            _count += count;

            ValueChanged?.Invoke(_count);
        }

        public bool TryTakeMoneys(int count)
        {
            if (_count < count)
                return false;

            _count -= count;
            ValueChanged?.Invoke(_count);

            return true;
        }
    }
}