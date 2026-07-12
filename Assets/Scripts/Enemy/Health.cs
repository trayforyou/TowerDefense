using System;

public class Health
{
    protected int _points;
    protected int _maxPoints;

    public event Action Died;

    public int Points => _points;
    public int MaxPoints => _maxPoints;

    public Health(int points)
    {
        _points = points;
        _maxPoints = points;
    }

    public void Reset() => 
        _points = _maxPoints;

    public void TakeDamage(int damage)
    {
        if (damage < 0)
            throw new ArgumentException("Отрицательное значение урона");

        if (damage > _points)
        {
            _points = 0;
            ValueChange();
            Died?.Invoke();
        }
        else
        {
            _points -= damage;
            ValueChange();
        }
    }

    protected virtual void ValueChange(){}
}