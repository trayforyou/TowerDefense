using System;
using System.Threading;
using _Project.Scripts.Enemies;
using _Project.Scripts.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    public class EnemyFinder : IDisposable
    {
        private float _sqrRadius;
        private float _range;
        private bool _isEnabled = true;
        private Collider[] _collidersBuffer;
        private ShooterConfig _shooterConfig;
        private Vector3 _position;
        private CancellationTokenSource _tokenSource;

        public event Action<Enemy> FoundEnemy;

        public EnemyFinder(float radius, ShooterConfig config, Vector3 position)
        {
            _position = position;
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

            TokenCleaner.Clear(ref _tokenSource);
            _tokenSource = new CancellationTokenSource();
            StartFindNearestEnemy(_tokenSource.Token).Forget();
        }

        public void Stop() =>
            _isEnabled = false;

        public void Dispose()
        {
            Stop();
            TokenCleaner.Clear(ref _tokenSource);
        }

        private async UniTaskVoid StartFindNearestEnemy(CancellationToken token)
        {
            try
            {
                int wait = (int)(_shooterConfig.FindDelay * 1000);
                Enemy target;

                while (_isEnabled)
                {
                    target = FindNearestEnemy();

                    if (target != null)
                    {
                        FoundEnemy?.Invoke(target);
                        break;
                    }

                    await UniTask.Delay(wait, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        private Enemy FindNearestEnemy()
        {
            int sizeArray = Physics.OverlapSphereNonAlloc(_position, _range, _collidersBuffer);

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

                    float sqrDistance = Vector3.SqrMagnitude(_position - enemy.transform.position);

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