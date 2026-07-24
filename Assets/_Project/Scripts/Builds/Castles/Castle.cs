using System;
using _Project.Scripts.Builds.Shooters;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Builds.Castles
{
    [RequireComponent(typeof(Gun))]
    [RequireComponent(typeof(ParticleSystem))]
    public class Castle : MonoBehaviour
    {
        private ParticleSystem _particles;
        private CastleHealth _health;
        private GameConfig _config;
        private Gun _gun;

        public event HealthChangedEventHandler ValueChanged;
        public event Action Died;

        private void Awake()
        {
            _gun = GetComponent<Gun>();
            _particles = GetComponent<ParticleSystem>();
        }

        private void OnDestroy()
        {
            _health.ValueChanged -= ChangedHealthValue;
            _health.Died -= Die;
        }

        private void Die()
        {
            _gun.Stop();
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
            _gun.Initialize(_config, _config.RadiusRangeCastle, _config.StartDelayShootCastle,
                _config.StartDamageCastle);
            CreateHealth();
            ValueChanged?.Invoke(_health.MaxPoints, _health.MaxPoints);
        }

        public void UpHealth() =>
            _health.TryUpLevel();

        public void UpStrong() =>
            _gun.UpForceLevel();

        public void UpSpeed() =>
            _gun.UpSpeedLevel();

        private void CreateHealth()
        {
            if (_config == null)
                throw new ArgumentException("Config is null");

            _health = new CastleHealth(_config.StartHealthCastle, _config.UpgradeMultiplier);
            _health.ValueChanged += ChangedHealthValue;
            _health.Died += Die;
            _health.RefreshInfo();
        }
    }
}