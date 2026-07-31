using System;
using System.Collections;
using _Project.Scripts.Enemies;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    public class EnemyFinder : MonoBehaviour
    {
        private float _sqrRadius;
        private float _range;
        private bool _isEnabled = true;
        private Collider[] _collidersBuffer;
        private ShooterConfig _shooterConfig;

        public event Action<Enemy> FoundEnemy;

        public void Initialize(float radius, ShooterConfig config)
        {
            _collidersBuffer = new Collider[config.FindBufferSize];
            _range = radius;
            _sqrRadius = radius * radius;
            _shooterConfig = config;
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
            var wait = new WaitForSeconds(_shooterConfig.FindDelay);
            Enemy target;

            while (_isEnabled)
            {
                target = FindNearestEnemy();

                if (target != null)
                {
                    FoundEnemy?.Invoke(target);
                    break;
                }

                yield return wait;
            }
        }

        private Enemy FindNearestEnemy()
        {
            int sizeArray = Physics.OverlapSphereNonAlloc(transform.position, _range, _collidersBuffer);

            if (sizeArray == 0)
                return null;

            float minSqrDistance = float.MaxValue;
            Enemy nearestEnemy = null;

            for (int i = 0; i < sizeArray; i++)
            {
                if (_collidersBuffer[i].TryGetComponent(out Enemy enemy))
                {
                    if (!enemy.IsAlive)
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