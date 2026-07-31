using _Project.Scripts.Enemies;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    [RequireComponent(typeof(EnemyFinder))]
    [RequireComponent(typeof(Shooter))]
    public class Gun : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _shootPoint;

        private Enemy _currentTarget;
        private EnemyFinder _enemyFinder;
        private Shooter _shooter;

        private void Awake()
        {
            _enemyFinder = GetComponent<EnemyFinder>();
            _shooter = GetComponent<Shooter>();
        }

        private void OnDestroy() =>
            _enemyFinder.FoundEnemy -= StartShoot;

        public void Initialize(ShooterConfig config, float range, float shootDelay, int damage)
        {
            _enemyFinder.Initialize(range, config);
            _shooter.Initialize(config, damage, shootDelay, _bulletPrefab, _shootPoint.transform.position);

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
            StopAllCoroutines();
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
    }
}