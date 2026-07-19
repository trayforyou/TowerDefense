using System;
using System.Collections;
using TowerDefense.Builds.Castle;
using TowerDefense.ScriptableObjects;
using UnityEngine;

namespace TowerDefense.Enemy
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(ParticleSystem))]
    public class Enemy : MonoBehaviour
    {
        private Mover _mover;
        private Animator _animator;
        private Castle _target;
        private float _sqrStopDistance;
        private Health _health;
        private GameConfig _config;
        private Coroutine _coroutine;
        private ParticleSystem _particles;

        private bool _isInitialized => (_target != null && _config != null);

        public event Action Running;
        public event Action<Enemy> Died;

        [field: SerializeField] public Transform AimPoint { get; private set; }
        public int Health => _health.PointsCount;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<Mover>();
            _particles = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            _particles.Stop();
            _mover.HasCome += GoAttack;
            _mover.Running += Run;
        }

        private void OnDisable()
        {
            _mover.HasCome -= GoAttack;
            _mover.Running -= Run;
            _health.Died -= Die;
        }

        public void Stop()
        {
            StopAllCoroutines();

            _animator.SetBool("IsRun", false);
            _mover.HasCome -= GoAttack;
            _mover.Running -= Run;
            _health.Died -= Die;
        }

        public void ResetHealth()
        {
            if (!_isInitialized)
                return;

            _health.Reset();
            _health.Died += Die;
        }

        private void Run() =>
            Running?.Invoke();

        public void TakeDamage(int damage)
        {
            _particles.Play();
            _health.TakeDamage(damage);
        }

        public void SetParams(Castle target, GameConfig config)
        {
            if (_isInitialized)
                return;

            if (target == null || config == null)
                throw new ArgumentNullException();

            _target = target;
            _config = config;
            _health = new Health(_config.EnemyHealth);
            _sqrStopDistance = _config.EnemyStopDistance * _config.EnemyStopDistance;
            
            _mover.SetParams(target.transform, config);
        }

        public void StartAttack() =>
            StartCoroutine(StartAttackCoroutine());

        private void GoAttack()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _animator.SetBool("IsRun", false);

            _coroutine = StartCoroutine(StartAttackCoroutine());
        }

        private IEnumerator StartAttackCoroutine()
        {
            var wait = new WaitForSeconds(_config.EnemyAttackDelay);
            float sqrDistance = Vector3.SqrMagnitude(_target.transform.position - transform.position);

            while (sqrDistance < _sqrStopDistance)
            {
                _target.TakeDamage(_config.EnemyDamage);
                _animator.SetTrigger("Attack");
                sqrDistance = Vector3.SqrMagnitude(_target.transform.position - transform.position);

                yield return wait;
            }

            _mover.GoToTarget();

            _animator.SetBool("IsRun", true);
            _coroutine = null;
        }

        private void Die()
        {
            _health.Died -= Die;
            Died?.Invoke(this);
        }
    }
}