using System;
using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    public class Gun : IDisposable
    {
        private Enemy _currentTarget;
        private EnemyFinder _enemyFinder;
        private Shooter _shooter;

        public void Dispose()
        {
            _enemyFinder.FoundEnemy -= StartShoot;
            _enemyFinder.Dispose();
            _shooter.Dispose();
        }

        public Gun(Shooter shooter, EnemyFinder enemyFinder)
        {
            _enemyFinder = enemyFinder;
            _shooter = shooter;

            _enemyFinder.FoundEnemy += StartShoot;
            _enemyFinder.Find();
        }

        public void SetShootDelay(float delay) =>
            _shooter.SetShootDelay(delay);

        public void SetDamage(int damage) =>
            _shooter.SetDamage(damage);

        public void Stop()
        {
            if (_currentTarget != null)
                _currentTarget.Died -= RefreshTarget;

            _shooter.Stop();
            _enemyFinder.Stop();
        }

        private void StartShoot(Enemy target)
        {
            _currentTarget = target;
            _currentTarget.Died += RefreshTarget;
            _shooter.Stop();
            _shooter.Attack(_currentTarget);
        }

        private void RefreshTarget(Enemy enemy)
        {
            enemy.Died -= RefreshTarget;
            RefreshTarget();
        }

        private void RefreshTarget() =>
            _enemyFinder.Find();

        public void SetShootPoint(Vector3 shootPoint) =>
            _shooter.SetShootPoint(shootPoint);
    }
}