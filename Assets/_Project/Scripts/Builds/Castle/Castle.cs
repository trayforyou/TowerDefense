using System;
using TowerDefense.ScriptableObjects;
using UnityEngine;

namespace TowerDefense.Builds.Castle
{
    [RequireComponent(typeof(Shooter.Shooter))]
    [RequireComponent(typeof(ParticleSystem))]
    public class Castle : MonoBehaviour
    {
        private ParticleSystem _particles;
        private CastleHealth _health;
        private GameConfig _config;
        private Shooter.Shooter _shooter;

        public event Action<int, int> ValueChanged;
        public event Action Died;

        private void Awake()
        {
            _shooter = GetComponent<Shooter.Shooter>();
            _particles = GetComponent<ParticleSystem>();
        }

        private void OnDestroy()
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

        public void TakeDamage(int damage)
        {
            if (_health == null)
                throw new Exception("_health не инициализирован");

            _particles.Play();
            _health.TakeDamage(damage);
        }

        public void SetConfig(GameConfig config)
        {
            if (_config != null)
                return;

            _config = config;
            _shooter.Initialize(_config, _config.RadiusRangeCastle, _config.StartDelayShootCastle,
                _config.StartDamageCastle);
            CreateHealth();
            ValueChanged?.Invoke(_health.MaxPointsCount, _health.MaxPointsCount);
        }

        public void UpHealth() =>
            _health.TryUpLevel();

        public void UpStrong() =>
            _shooter.UpForceLevel();

        public void UpSpeed() =>
            _shooter.UpSpeedLevel();

        private void CreateHealth()
        {
            if (_config == null)
                throw new ArgumentException("Config is null");

            _health = new CastleHealth(_config.StartHealthCastle, _config.MaxCastleLevel, _config.UpgradeMultiplier);
            _health.ValueChanged += ChangedHealthValue;
            _health.Died += Die;
            _health.RefreshInfo();
        }
    }
}