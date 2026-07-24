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

        private GameConfig _config;
        private Enemy _currentTarget;
        private EnemyFinder _enemyFinder;
        private Shooter _shooter;
        private int _currentForceLevel;
        private int _currentSpeedLevel;
        private float _currentDelay;
        private int _currentDamage;

        private void Awake()
        {
            _enemyFinder = GetComponent<EnemyFinder>();
            _shooter = GetComponent<Shooter>();
        }

        private void OnDestroy() =>
            _enemyFinder.FoundEnemy -= StartShoot;

        public void Initialize(GameConfig config, float range, float shootDelay, int damage)
        {
            if (_enemyFinder == null)
                Debug.Log("EnemyFinder равен нулю"); 
            
            _currentDelay = shootDelay;
            _currentDamage = damage;
            _currentForceLevel++;
            _currentSpeedLevel++;
            _config = config;
            _enemyFinder.Initialize(range, _config);
            _shooter.Initialize(_config, damage, shootDelay, _bulletPrefab, _shootPoint.transform.position);

            _enemyFinder.FoundEnemy += StartShoot;
            _enemyFinder.Find();
        }

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

        public void UpSpeedLevel()
        {
            if (_currentSpeedLevel > _config.MaxCastleLevel)
                return;

            _currentSpeedLevel++;
            _currentDelay = _config.StartDelayShootCastle;

            for (int i = 1; i < _currentSpeedLevel; i++)
                _currentDelay /= _config.UpgradeMultiplier;

            _shooter.UpLevelSpeed(_currentDelay);
        }

        public void UpForceLevel()
        {
            if (_currentForceLevel > _config.MaxCastleLevel)
                return;

            _currentForceLevel++;
            _currentDamage = _config.StartDamageCastle;

            for (int i = 1; i < _currentForceLevel; i++)
                _currentDamage = (int)(_currentDamage * _config.UpgradeMultiplier);

            _shooter.UpLevelDamage(_currentDamage);
        }
    }
}