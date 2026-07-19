using System;

namespace TowerDefense
{
    public class Health
    {
        protected int Points;
        protected int MaxPoints;

        public event Action Died;

        public int PointsCount => Points;
        public int MaxPointsCount => MaxPoints;

        public Health(int points)
        {
            Points = points;
            MaxPoints = points;
        }

        public void Reset() =>
            Points = MaxPoints;

        public void TakeDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException("Отрицательное значение урона");

            if (damage > Points)
            {
                Points = 0;
                ValueChange();
                Died?.Invoke();
            }
            else
            {
                Points -= damage;
                ValueChange();
            }
        }

        protected virtual void ValueChange() {}
    }
}