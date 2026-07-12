using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public event Action<int> ValueChanged;

    public int Count { get; private set; }

    private void Start() => 
        Count = 0;

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