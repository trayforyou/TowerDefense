using System;
using System.Collections;
using TowerDefense.ScriptableObjects;
using UnityEngine;

namespace TowerDefense.Builds.Shooter
{
    public class EnemyFinder : MonoBehaviour
    {
        private GameConfig _config;
        private float _sqrRadius;
        private float _range;
        private bool _isEnabled;

        public event Action<Enemy.Enemy> FoundEnemy;

        public void Initialize(float radius, GameConfig config)
        {
            _range = radius;
            _sqrRadius = radius * radius;
            _config = config;
        }

        public void Find()
        {
            Enemy.Enemy tempEnemy = FindNearestEnemy();

            if (tempEnemy != null)
            {
                FoundEnemy?.Invoke(tempEnemy);
                return;
            }

            StartCoroutine(StartFindNearestEnemy());
        }

        public void OnDestroy() =>
            StopAllCoroutines();

        private IEnumerator StartFindNearestEnemy()
        {
            var wait = new WaitForSeconds(_config.FindDelay);
            Enemy.Enemy target = null;
            
            while (enabled)
            {
                target = FindNearestEnemy();

                if (target != null)
                    break;

                yield return wait;
            }

            FoundEnemy?.Invoke(target);
        }

        private Enemy.Enemy FindNearestEnemy()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _range);

            if (colliders.Length == 0)
                return null;

            float minSqrDistance = float.MaxValue;
            Enemy.Enemy nearestEnemy = null;

            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out Enemy.Enemy enemy))
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