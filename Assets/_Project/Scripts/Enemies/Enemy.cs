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
        private static readonly int IsRun = Animator.StringToHash("IsRun");
        private static readonly int Attack = Animator.StringToHash("Attack");
        
        private Mover _mover;
        private Health _health;
        private Animator _animator;
        private EnemyAttacker _attacker;
        private ParticleSystem _particles;

        public event Action<Enemy> Died;

        [field: SerializeField] public Transform AimPoint { get; private set; }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _mover = GetComponent<Mover>();
            _particles = GetComponent<ParticleSystem>();
            _attacker = GetComponent<EnemyAttacker>();
        }

        private void OnEnable()
        {
            _particles.Stop();
            SubscribeAll();
        }

        private void OnDisable() => 
            UnsubscribeAll();

        public void SetParams(Castle target, GameConfig config)
        {
            _health = new Health(config.EnemyHealth);
            float sqrStopDistance = config.EnemyStopDistance * config.EnemyStopDistance;
            _attacker.Initialize(config,target,sqrStopDistance,transform);
            _mover.SetParams(target.transform, config);
        }
        
        public void GoToTarget() => 
            _mover.GoToTarget();

        public void Stop()
        {
            StopAllCoroutines();

            _animator.SetBool(IsRun, false);
            _attacker.Stop();
            UnsubscribeAll();
        }

        public void ResetHealth()
        {
            _health.Reset();
            _health.Died += Die;
        }

        public void TakeDamage(int damage)
        {
            _particles.Play();
            _health.TakeDamage(damage);
        }

        private void UnsubscribeAll()
        {
            _mover.HasCome -= StartAttack;
            _mover.Running -= AnimateRun;
            _health.Died -= Die;
            _attacker.Attacking -= AnimateAttack;
            _attacker.NeedingRun -= GoToTarget;
        }
        
        private void SubscribeAll()
        {
            _mover.HasCome += StartAttack;
            _mover.Running += AnimateRun;
            _health.Died += Die;
            _attacker.Attacking += AnimateAttack;
            _attacker.NeedingRun += GoToTarget;
        }
        
        private void StartAttack()
        {
            _animator.SetBool(IsRun, false);
            _attacker.Attack();
        }

        private void Die()
        {
            _health.Died -= Die;
            Died?.Invoke(this);
        }
        
        private void AnimateRun() => 
            _animator.SetBool(IsRun, true);
        
        private void AnimateAttack() => 
            _animator.SetTrigger(Attack);
    }
}