using System;
using System.Collections;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Mover : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private EnemiesConfig _config;
        private Transform _target;
        private Coroutine _coroutine;

        public event Action HasCome;
        public event Action Running;

        private void Awake() =>
            _agent = GetComponent<NavMeshAgent>();

        public void Stop()
        { 
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _agent.isStopped = true;
        }

        public void SetParams(Transform target, EnemiesConfig config)
        {
            _target = target;
            _config = config;

            _agent.stoppingDistance = _config.EnemyStopDistance;
            _agent.speed = _config.EnemySpeed;
        }

        public void GoToTarget()
        {
            if (!_agent.isOnNavMesh)
            {
                float maxDistance = 1.5f;

                if (NavMesh.SamplePosition(_target.position, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
                    _agent.destination = hit.position;
                else
                    return;
            }

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _agent.destination = _target.position;
            _coroutine = StartCoroutine(RunToTower());
        }

        private IEnumerator RunToTower()
        {
            _agent.isStopped = false;

            Running?.Invoke();

            yield return null;

            while (_agent.remainingDistance > _agent.stoppingDistance)
                yield return null;

            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            HasCome?.Invoke();
        }
    }
}