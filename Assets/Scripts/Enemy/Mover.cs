using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    private NavMeshAgent _agent;
    private GameConfig _config;
    private Transform _target;
    private Coroutine _coroutine;
    private bool _isInitialize = false;
    private float _sqrStopDistance;

    public event Action HasCome;
    public event Action Running;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _config = GameManager.Instance.Config;
    }

    public void SetParams(Transform target)
    {
        if (_isInitialize)
            return;

        _target = target ?? throw new ArgumentNullException(nameof(target));
        _sqrStopDistance = _config.EnemyStopDistance * _config.EnemyStopDistance;
        _agent.speed = _config.EnemySpeed;
        _isInitialize = true;
    }

    public void GoToTarget()
    {
        if (!_agent.isOnNavMesh)
        {
            NavMeshHit hit;
            float maxDistance = 1.5f;

            if (NavMesh.SamplePosition(_target.position, out hit, maxDistance, NavMesh.AllAreas))
                _agent.destination = hit.position;
            else
                return;
        }

        _agent.destination = _target.position;

        if (_coroutine is not null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(RunToTower());
    }

    private IEnumerator RunToTower()
    {
        _agent.isStopped = false;

        Running?.Invoke();

        float sqrDistance = Vector3.SqrMagnitude(_target.position - transform.position);

        while (sqrDistance > _sqrStopDistance)
        {
            sqrDistance = Vector3.SqrMagnitude(_target.position - transform.position);
            yield return new WaitForFixedUpdate();
        }

        _agent.isStopped = true;
        _agent.velocity = Vector3.zero;
        HasCome?.Invoke();
    }
}