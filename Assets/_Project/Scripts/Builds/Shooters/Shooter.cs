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
        private BulletsStorage _bulletsPool;
        private Vector3 _shootPoint;
        private int _currentDelay;
        private Coroutine _attackCoroutine;
        private CancellationTokenSource _tokenSource;

        public Shooter(ShooterConfig config, int damage, float shootDelay, Bullet bulletPrefab)
        {
            _bulletsPool = new BulletsStorage(bulletPrefab, config, damage);
            SetShootDelay(shootDelay);
            _shootPoint = Vector3.zero;
        }

        public void Attack(Enemy enemy)
        {
            if(_shootPoint == Vector3.zero)
                throw new NullReferenceException(nameof(_shootPoint));
            
            TokenCleaner.Clear(ref _tokenSource);
            _tokenSource = new CancellationTokenSource();
            StartAttack(_tokenSource.Token, enemy).Forget();
        }

        public void Dispose() => 
            Stop();

        public void Stop() => 
            TokenCleaner.Clear(ref _tokenSource);

        public void SetShootDelay(float newDelay) =>
            _currentDelay = (int)(newDelay * 1000);

        public void SetDamage(int damage) =>
            _bulletsPool.ChangeDamage(damage);

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