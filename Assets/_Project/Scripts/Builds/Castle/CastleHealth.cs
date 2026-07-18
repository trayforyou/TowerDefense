using System;
using UnityEngine;

namespace TowerDefense.Builds.Castle
{
    public class CastleHealth : Health
    {
        private float _healthMultiply;
        private int _maxLevel;

        [field: SerializeField] public int CurrentLevel { get; private set; }

        public bool CanUpLevel => CurrentLevel < _maxLevel;

        public event Action<int, int> ValueChanged;

        public CastleHealth(int points, int maxLevel, float healthPercentMultiply) : base(points)
        {
            _maxLevel = maxLevel;
            _healthMultiply = healthPercentMultiply;
        }

        public void RefreshInfo() =>
            ValueChange();

        public bool TryUpLevel()
        {
            if (!CanUpLevel)
                return false;

            MaxPoints = (int)(MaxPoints * _healthMultiply);
            Points = (int)(Points * _healthMultiply);

            ValueChange();

            return true;
        }

        protected override void ValueChange() =>
            ValueChanged?.Invoke(Points, MaxPoints);
    }
}