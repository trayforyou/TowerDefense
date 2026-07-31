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

        public void TakeDamage(int damage)
        {
            if (_health == null)
                throw new Exception("_health не инициализирован");

            _particles.Play();
            _health.TakeDamage(damage);
        }

        public void SetConfig(CastleConfig config, ShooterConfig shooterConfig)
        {
            _gun.Initialize(shooterConfig, config.RadiusRangeCastle, config.StartDelayShootCastle,
                config.StartDamageCastle);
            CreateHealth(config);
            ValueChanged?.Invoke(_health.MaxPoints, _health.MaxPoints);
        }

        public void UpHealth() =>
            _health.TryUpgrade();

        public void UpForce(int damage) =>
            _gun.SetDamage(damage);

        public void UpSpeed(float delay) =>
            _gun.SetShootDelay(delay);

        private void Die()
        {
            _gun.Stop();
            Died?.Invoke();
        }

        private void ChangedHealthValue(int points, int maxPoints) =>
            ValueChanged?.Invoke(points, maxPoints);
        
        private void CreateHealth(CastleConfig config)
        {
            _health = new CastleHealth(config.StartHealthCastle, config.UpgradeMultiplier);
            _health.ValueChanged += ChangedHealthValue;
            _health.Died += Die;
            _health.RefreshInfo();
        }
    }
}