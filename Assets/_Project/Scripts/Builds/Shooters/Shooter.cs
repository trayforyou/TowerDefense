using System;
using System.Threading;
using _Project.Scripts.Enemies;
using _Project.Scripts.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    public class Shooter : IDisposable
    {
        private readonly float _multiplierDamage;
        private readonly float _multiplierDelay;
        
        private CancellationTokenSource _tokenSource;
        private BulletsStorage _bulletsPool;
        private Vector3 _shootPoint;
        private int _currentDelay;

        public Shooter(ShooterConfig config, int damage, float shootDelay, Bullet bulletPrefab, float multiplierDamage,
            float multiplierDelay)
        {
            _multiplierDamage = multiplierDamage;
            _multiplierDelay = multiplierDelay;
            _bulletsPool = new BulletsStorage(bulletPrefab, config, (int)(damage * _multiplierDamage));
            SetShootDelay(shootDelay);
            _shootPoint = Vector3.zero;
        }

        public void Attack(Enemy enemy)
        {
            if (_shootPoint == Vector3.zero)
                throw new NullReferenceException(nameof(_shootPoint));

            _tokenSource.Clear();
            _tokenSource = new CancellationTokenSource();
            StartAttack(_tokenSource.Token, enemy).Forget();
        }

        public void Dispose() =>
            Stop();

        public void Stop() =>
            _tokenSource.Clear();

        public void SetShootDelay(float newDelay) =>
            _currentDelay = (int)(newDelay * 1000 * _multiplierDelay);

        public void SetDamage(int damage) =>
            _bulletsPool.ChangeDamage((int)(damage * _multiplierDamage));

        private async UniTaskVoid StartAttack(CancellationToken token, Enemy currentTarget)
        {
            try
            {
                await UniTask.Delay(_currentDelay, cancellationToken: token);
                Bullet tempBullet;

                while (currentTarget.IsAlive)
                {
                    tempBullet = _bulletsPool.Get();
                    tempBullet.Releasing += Release;
                    tempBullet.Shoot(_shootPoint, currentTarget);

                    await UniTask.Delay(_currentDelay, cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void Release(Bullet bullet)
        {
            bullet.Releasing -= Release;
            _bulletsPool.Release(bullet);
        }

        public void SetShootPoint(Vector3 shootPoint) =>
            _shootPoint = shootPoint;
    }
}