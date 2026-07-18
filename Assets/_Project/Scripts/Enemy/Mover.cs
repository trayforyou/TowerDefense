using System;
using System.Collections;
using TowerDefense.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDefense.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private GameConfig _config;
        private Transform _target;
        private Coroutine _coroutine;
        private float _sqrStopDistance;

        private bool _isInitialize => (_target != null && _config != null);

        public event Action HasCome;
        public event Action Running;

        private void Awake() =>
            _agent = GetComponent<NavMeshAgent>();

        public void SetParams(Transform target, GameConfig config)
        {
            if (_isInitialize)
                return;

            if (target == null || config == null)
                throw new ArgumentNullException(nameof(target));

            _target = target;
            _config = config;

            _sqrStopDistance = _config.EnemyStopDistance * _config.EnemyStopDistance;
            _agent.speed = _config.EnemySpeed;
        }

        public void GoToTarget()
        {
            if (!_isInitialize)
                return;

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

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(RunToTower());
        }

        private IEnumerator RunToTower()
        {
            if (!_isInitialize)
                yield break;

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
}