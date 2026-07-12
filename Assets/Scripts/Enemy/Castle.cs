using System;
using UnityEngine;

[RequireComponent(typeof(Shooter))]
[RequireComponent(typeof(ParticleSystem))]
public class Castle : MonoBehaviour
{
    private ParticleSystem _particles;
    private CastleHealth _health;
    private GameConfig _config;
    private Shooter _shooter;

    public event Action<int, int> ValueChanged;
    public event Action Died;

    private void Awake()
    {
        _shooter = GetComponent<Shooter>();
        _particles = GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        _health.ValueChanged -= ChangedHealthValue;
        _health.Died -= Die;
    }

    private void Die()
    {
        _shooter.Stop();
        Died?.Invoke();
    }

    private void ChangedHealthValue(int points, int maxPoints) =>
        ValueChanged?.Invoke(points, maxPoints);

    public bool TryGetHealthInfo(out int maxHealth, out int currentHealth)
    {
        if (_health is null)
        {
            maxHealth = 0;
            currentHealth = 0;
            return false;
        }

        maxHealth = _health.MaxPoints;
        currentHealth = _health.Points;

        return true;
    }

    public void TakeDamage(int damage)
    {
        if (_health is null)
            throw new Exception("_health не инициализирован");

        _particles.Play();
        _health.TakeDamage(damage);
    }

    public void SetConfig(GameConfig config)
    {
        if (_config is not null)
            return;

        _config = config;

        CreateHealth();
    }

    public void UpHealth() => 
        _health.TryUpLevel();

    public void UpStrong() => 
        _shooter.UpForceLevel();

    public void UpSpeed() => 
        _shooter.UpSpeedLevel();

    private void CreateHealth()
    {
        _health = new CastleHealth(_config.StartHealthCastle, _config.MaxCastleLevel, _config.UpgradeMultiplier);
        _health.ValueChanged += ChangedHealthValue;
        _health.Died += Die;
        _health.RefreshInfo();
    }
}