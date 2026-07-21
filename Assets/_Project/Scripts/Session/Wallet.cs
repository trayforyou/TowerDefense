using System;
using UnityEngine;

namespace TowerDefense.Session
{
    public class Wallet : MonoBehaviour
    {
        public event Action<int> ValueChanged;

        public int Count { get; private set; }

        private void Start() =>
            Count = 0;

        public void AddMoneys(int count)
        {
            Count += count;

            ValueChanged?.Invoke(Count);
        }

        public bool TryTakeMoneys(int count)
        {
            if (Count < count)
                return false;

            Count -= count;
            ValueChanged?.Invoke(Count);

            return true;
        }
    }
}