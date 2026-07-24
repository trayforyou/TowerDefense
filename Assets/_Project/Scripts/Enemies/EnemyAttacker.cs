using System;
using System.Collections;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyAttacker : MonoBehaviour
    {
        private GameConfig _config;
        private Castle _castle;
        private float _sqrStopDistance;
        private Transform _transform;

        public event Action Attacking;
        public event Action NeedingRun;

        public void Initialize(GameConfig config, Castle castle, float sqrStopDistance, Transform enemy)
        {
            _config = config;
            _castle = castle;
            _sqrStopDistance = sqrStopDistance;
            _transform = enemy;
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
                sqrDistance = Vector3.SqrMagnitude(_castle.transform.position - _transform.position);

                yield return wait;
            }

            NeedingRun?.Invoke();
        }
    }
}