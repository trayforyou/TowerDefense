using UnityEngine;

namespace TowerDefense.Builds.Castle
{
    public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);

    public class CastleHealth : Health
    {
        private float _healthMultiply;

        public event HealthChangedEventHandler ValueChanged;

        public CastleHealth(int points, float healthPercentMultiply) : base(points) =>
            _healthMultiply = healthPercentMultiply;

        public void RefreshInfo() =>
            ValueChange();

        public void TryUpLevel()
        {
            MaxPoints = (int)(MaxPoints * _healthMultiply);
            int tempHp = (int)(Points * _healthMultiply);

            if (Points == tempHp)
                Points++;
            else
                Points = tempHp;

            ValueChange();
        }

        protected override void ValueChange() =>
            ValueChanged?.Invoke(Points, MaxPoints);
    }
}