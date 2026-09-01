using System;
using System.Collections;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyAttacker : MonoBehaviour
    {
        private EnemiesConfig _config;
        private Castle _castle;
        private float _sqrStopDistance;
        private Transform _transform;
        private Coroutine _attackCoroutine;

        public event Action Attacking;
        public event Action NeedingRun;

        public void Initialize(EnemiesConfig config, Castle castle, float sqrStopDistance, Transform enemy)
        {
            _config = config;
            _castle = castle;
            _sqrStopDistance = sqrStopDistance;
            _transform = enemy;
        }

        public void Attack()
        {
            StopAllCoroutines();
            _attackCoroutine = StartCoroutine(StartAttackCoroutine());
        }

        public void Stop()
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);
        }

        private IEnumerator StartAttackCoroutine()
        {
            var wait = new WaitForSeconds(_config.EnemyAttackDelay);
            float sqrDistance = Vector3.SqrMagnitude(_castle.transform.position - _transform.position);

            while (sqrDistance < _sqrStopDistance)
            {
                _castle.TakeDamage(_config.EnemyDamage);
                Attacking?.Invoke();
                sqrDistance = Vector3.SqrMagnitude(_castle.transform.position - _transform.position);

                yield return wait;
            }

            NeedingRun?.Invoke();
        }
    }
}