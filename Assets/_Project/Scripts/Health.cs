using System;

namespace _Project.Scripts
{
    public class Health
    {
        protected int Points;

        public event Action Died;

        public int MaxPoints { get; protected set; }

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
            
            if(Points <= 0)
                Died?.Invoke();
        }

        protected virtual void ValueChange() {}
    }
}