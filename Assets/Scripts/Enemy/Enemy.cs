using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Mover))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ParticleSystem))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform _aimPoint;

    private Mover _mover;
    private Animator _animator;
    private Castle _target;
    private float _sqrStopDistance;
    private Health _health;
    private GameConfig _config;
    private Coroutine _coroutine;
    private ParticleSystem _particles;

    public event Action Running;
    public event Action<Enemy> Died;

    public Transform AimPoint => _aimPoint;
    public int Health => _health.Points;

    private void Awake()
    {
        _config = GameManager.Instance.Config;
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
        if (_health is null)
            _health = new Health(_config.EnemyHealth);
        else
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

    public void SetParams(Castle target)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _sqrStopDistance = _config.EnemyStopDistance * _config.EnemyStopDistance;
        _mover.SetParams(target.transform);
    }

    public void StartAttack() =>
        StartCoroutine(StartAttackCoroutine());

    private void GoAttack()
    {
        if (_coroutine is not null)
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