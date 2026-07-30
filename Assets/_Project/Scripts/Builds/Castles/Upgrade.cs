using System;
using _Project.Scripts.Session;

namespace _Project.Scripts.Builds.Castles
{
    public class Upgrade
    {
        private readonly int _maxLevel;
        private readonly float _costMultiplier;
        private Action _onApplyUpgrade;
        private Action<int> _onCostChanged;

        public int Level { get; private set; }
        public int CurrentCost { get; private set; }

        public Upgrade(int startCost, int maxLevel, float costMultiplier,
            Action onApplyUpgrade, Action<int> onCostChanged)
        {
            _onApplyUpgrade = onApplyUpgrade;
            _onCostChanged = onCostChanged;
            _costMultiplier = costMultiplier;
            _maxLevel = maxLevel;
            Level = 1;
            CurrentCost = startCost;
        }

        public bool TryUpgrade(Wallet wallet)
        {
            if (Level >= _maxLevel)
                return false;

            if (!wallet.TryTakeMoney(CurrentCost))
                return false;

            _onApplyUpgrade?.Invoke();

            CurrentCost = (int)(CurrentCost * _costMultiplier);
            _onCostChanged.Invoke(CurrentCost);

            Level++;

            return true;
        }
    }
}