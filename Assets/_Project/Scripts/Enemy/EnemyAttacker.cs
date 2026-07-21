using System;
using System.Collections;
using TowerDefense.Builds.Castle;
using TowerDefense.ScriptableObjects;
using UnityEngine;

namespace TowerDefense.Enemy
{
    public class EnemyAttacker : MonoBehaviour
    {
        private GameConfig _config;
        private Castle _castle;
        private float _sqrStopDistance;
        private Transform _transform;

        public event Action Attacking;
        public event Action NeedingRun;

        public void Initialize(GameConfig config, Castle castle, float sqrStopDistance, Transform transform)
        {
            _config = config;
            _castle = castle;
            _sqrStopDistance = sqrStopDistance;
            _transform = transform;
        }

        public void Attack()
        {
            StopAllCoroutines();
            StartCoroutine(StartAttackCoroutine());
        }

        public void Stop() => 
            StopAllCoroutines();

        private IEnumerator StartAttackCoroutine()
        {
            var wait = new WaitForSeconds(_config.EnemyAttackDelay);
            float sqrDistance = Vector3.SqrMagnitude(_castle.transform.position - _transform.position);

            while (sqrDistance < _sqrStopDistance)
            {
                _castle.TakeDamage(_config.EnemyDamage);
                Attacking?.Invoke();
                sqrDistance = Vector3.SqrMagnitude(_castle.transform.position - transform.position);

                yield return wait;
            }

            NeedingRun?.Invoke();
        }
    }
}