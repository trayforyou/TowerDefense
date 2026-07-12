using System;

public class CastleHealth : Health
{
    private int _currentLevel;
    private int _maxLevel;
    private float _healthMultiply;

    public int CurrentLevel => _currentLevel;
    public int MaxLevel => _maxLevel;
    public bool CanUpLevel => _currentLevel < _maxLevel;

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
            
            _maxPoints = (int)(_maxPoints * _healthMultiply);
            _points = (int)(_points * _healthMultiply);

            ValueChange();

            return true;
    }

    protected override void ValueChange() => 
        ValueChanged?.Invoke(_points, _maxPoints);
}