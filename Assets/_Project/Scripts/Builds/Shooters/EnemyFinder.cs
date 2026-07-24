using System;
using System.Collections;
using _Project.Scripts.Enemies;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    public class EnemyFinder : MonoBehaviour
    {
        private GameConfig _config;
        private float _sqrRadius;
        private float _range;
        private bool _isEnabled = true;

        public event Action<Enemy> FoundEnemy;

        public void Initialize(float radius, GameConfig config)
        {
            _range = radius;
            _sqrRadius = radius * radius;
            _config = config;
        }

        public void Find()
        {
            var tempEnemy = FindNearestEnemy();

            if (tempEnemy != null)
            {
                FoundEnemy?.Invoke(tempEnemy);
                return;
            }

            StartCoroutine(StartFindNearestEnemy());
        }

        public void Stop() => 
            _isEnabled = false;

        private void OnDestroy() =>
            StopAllCoroutines();

        private IEnumerator StartFindNearestEnemy()
        {
            var wait = new WaitForSeconds(_config.FindDelay);
            Enemy target = null;
            
            while (_isEnabled)
            {
                target = FindNearestEnemy();

                if (target != null)
                    break;

                yield return wait;
            }

            FoundEnemy?.Invoke(target);
        }

        private Enemy FindNearestEnemy()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _range);

            if (colliders.Length == 0)
                return null;

            float minSqrDistance = float.MaxValue;
            Enemy nearestEnemy = null;

            foreach (Collider enemyCollider in colliders)
            {
                if (enemyCollider.TryGetComponent(out Enemy enemy))
                {
                    if (!enemy.isActiveAndEnabled)
                        continue;

                    float sqrDistance = Vector3.SqrMagnitude(transform.position - enemy.transform.position);

                    if (sqrDistance <= _sqrRadius && sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        nearestEnemy = enemy;
                    }
                }
            }

            return nearestEnemy;
        }
    }
}