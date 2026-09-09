using _Project.Scripts.Builds.Castles;
using _Project.Scripts.ScriptableObjects;
using System;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyBehaviourHandler : IDisposable
    { 
        private readonly Mover _mover;
        private readonly ParticleSystem _particles;
        private readonly EnemyAttacker _attacker;
        private readonly EnemyAnimator _animator;
        private readonly Transform _transform;

        private Health _health;

        public event Action Died;

        public EnemyBehaviourHandler(Mover mover, ParticleSystem particles, EnemyAttacker attacker,
            EnemyAnimator animator, Transform transform)
        {
            _transform = transform;
            _mover = mover;
            _particles = particles;
            _attacker = attacker;
            _animator = animator;
        }

        public void SetParams(Castle target, EnemiesConfig config)
        {
            _health = new Health(config.EnemyHealth);
            float sqrStopDistance = config.EnemyStopDistance * config.EnemyStopDistance;
            _health.Died += Die;

            SubscribeAll();

            _attacker.Initialize(config, target, sqrStopDistance, _transform);
            _mover.SetParams(target.transform, config);
        }

        public void Start()
        {
            SubscribeAll();
            _particles.Stop();
            _animator.TurnOn();
        }

        public void Stop()
        {
            if (_attacker != null)
                _attacker.Stop();

            if (_mover != null)
                _mover.Stop();

            _animator?.Stop();
            UnsubscribeAll();
        }

        public void TurnOff()
        {
            _animator.TurnOff();
            UnsubscribeAll();
        }

        public void Dispose() =>
            Stop();

        public void GoToTarget()
        {
            _animator.TurnOn();
            _mover.GoToTarget();
        }

        public void TakeDamage(int damage)
        {
            _particles.Play();
            _health.TakeDamage(damage);
        }

        public void ResetHealth()
        {
            _health.Reset();
            _health.Died += Die;
        }

        private void StartAttack()
        {
            _animator.Stop();
            _attacker.Attack();
        }

        private void Die()
        {
            _health.Died -= Die;
            Died?.Invoke();
        }

        private void UnsubscribeAll()
        {
            _mover.HasCome -= StartAttack;
            _health.Died -= Die;
            _attacker.NeedingRun -= GoToTarget;
        }

        private void SubscribeAll()
        {
            _mover.HasCome += StartAttack;
            _attacker.NeedingRun += GoToTarget;
        }
    }
}