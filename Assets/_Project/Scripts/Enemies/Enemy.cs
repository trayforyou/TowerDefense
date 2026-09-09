using System;
using UnityEngine;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ParticleSystem))]
    [RequireComponent(typeof(EnemyAttacker))]
    public class Enemy : MonoBehaviour
    {
        private EnemyBehaviourHandler _handler;
        private EnemyAnimator _animator;

        public event Action<Enemy> Died;

        public bool IsAlive { get; private set; }

        [field: SerializeField] public Transform AimPoint { get; private set; }

        private void Awake()
        {
            _animator = new EnemyAnimator(GetComponent<Animator>(), GetComponent<Mover>(),
                GetComponent<EnemyAttacker>());

            _handler = new EnemyBehaviourHandler(GetComponent<Mover>(), GetComponent<ParticleSystem>(),
                GetComponent<EnemyAttacker>(),
                _animator, transform);
        }

        private void OnEnable()
        {
            IsAlive = true;
            _handler.Died += Die;
            _handler.Start();
        }

        private void OnDisable()
        {
            _handler.Died -= Die;
            _handler.TurnOff();
        }

        private void OnDestroy()
        {
            _handler.Dispose();
            _animator.Dispose();
        }

        public void GoToTarget() =>
            _handler.GoToTarget();

        public void ResetHealth() =>
            _handler.ResetHealth();

        public void TakeDamage(int damage) =>
            _handler.TakeDamage(damage);

        public void SetParams(Castle target, EnemiesConfig config) =>
            _handler.SetParams(target, config);

        public void Stop() =>
            _handler?.Stop();

        private void Die()
        {
            _handler.Died -= Die;
            IsAlive = false;
            Died?.Invoke(this);
        }
    }
}