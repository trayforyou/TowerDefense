using System.Collections;
using TowerDefense.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace TowerDefense.Builds.Shooter
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _shootPoint;

        private ObjectPool<Bullet> _bulletsPool;
        private GameConfig _config;
        private BulletsStorage _bulletsPool;
        private Vector3 _shootPoint;
        private WaitForSeconds _currentDelay;
        private Coroutine _attackCoroutine;
        private Enemy.Enemy _target;
        private float _sqrRadius;
        private float _currentDelay;
        private float _range;
        private int _currentDamage;
        private int _currentForceLevel;
        private int _currentSpeedLevel;

        public void Initialize(GameConfig config, float range, float delay, int damage)
        {
            if (_config != null)
                return;

            _currentForceLevel++;
            _currentSpeedLevel++;
            _config = config;
            _range = range;
            _sqrRadius = range * range;
            _currentDamage = damage;
            _currentDelay = delay;

            _bulletsPool = new ObjectPool<Bullet>
            (
                createFunc: () => Instantiate(_bulletPrefab),
                actionOnGet: GetBullet,
                actionOnRelease: ReleaseBullet,
                actionOnDestroy: bullet => Destroy(bullet.gameObject),
                collectionCheck: true,
                defaultCapacity: _config.MinBulletPullSize,
                maxSize: _config.MaxBulletPullSize
            );

            StartCoroutine(StartFindNearestEnemy());
        }

        public void Stop()
        {
            if (_target != null)
                _target.Died -= ClearTarget;

            StopAllCoroutines();
        }

        public void UpSpeedLevel()
        {
            if (_currentSpeedLevel > _config.MaxCastleLevel)
                return;

            _currentSpeedLevel++;
            _currentDelay = _config.StartDelayShootCastle;

            for (int i = 1; i < _currentSpeedLevel; i++)
                _currentDelay /= _config.UpgradeMultiplier;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _attackCoroutine = null;
        }

        public void UpForceLevel()
        {
            if (_currentForceLevel > _config.MaxCastleLevel)
                return;

            _currentForceLevel++;
            _currentDamage = _config.StartDamageCastle;

            for (int i = 1; i < _currentForceLevel; i++)
                _currentDamage = (int)(_currentDamage * _config.UpgradeMultiplier);

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _attackCoroutine = null;
        }

        private void ReleaseBullet(Bullet bullet)
        {
            bullet.transform.position = Vector3.down;
            bullet.gameObject.SetActive(false);
        }

        private void GetBullet(Bullet bullet)
        {
            bullet.gameObject.SetActive(true);
            bullet.SetDamage(_currentDamage);
        }

        private IEnumerator StartFindNearestEnemy()
        {
            var wait = new WaitForSeconds(_config.FindDelay);

            while (enabled)
            {
                yield return wait;

                if (_attackCoroutine != null)
                    continue;

                _target = FindNearestEnemy();

                if (_target == null)
                    continue;

                _target.Died += ClearTarget;
                _attackCoroutine = StartCoroutine(StartAttack(_target));
            }
        }

        private Enemy.Enemy FindNearestEnemy()
        {
            Collider[] colliders = Physics.OverlapSphere(_shootPoint.position, _range);

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

                    float sqrDistance = Vector3.SqrMagnitude(_shootPoint.position - enemy.transform.position);

                    if (sqrDistance <= _sqrRadius && sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        nearestEnemy = enemy;
                    }
                }
            }

            return nearestEnemy;
        }

        private IEnumerator StartAttack(Enemy.Enemy currentTarget)
        {
            var wait = new WaitForSeconds(_currentDelay);

            while (_target != null && _target == currentTarget)
            {
                if (_target == null)
                {
                    _attackCoroutine = null;
                    yield break;
                }

                StartCoroutine(Shoot());

                yield return wait;
            }

            _attackCoroutine = null;
        }

        private IEnumerator Shoot()
        {
            if (_target == null)
                yield break;

            bool isEnemyAlive = true;
            Bullet bullet = _bulletsPool.Get();

            Enemy.Enemy currentTarget = _target;
            bullet.transform.position = _shootPoint.position;
            Vector3 lastTargetPosition = currentTarget.AimPoint.position;
            bool isBulletFlying = true;

            while (isBulletFlying)
            {
                if (bullet == null)
                    yield break;

                if (currentTarget != null && currentTarget.isActiveAndEnabled && isEnemyAlive)
                {
                    lastTargetPosition = currentTarget.AimPoint.position;
                    bullet.transform.position = Vector3.MoveTowards(bullet.transform.position,
                        currentTarget.AimPoint.position, _config.BulletSpeed * Time.deltaTime);
                }
                else
                {
                    isEnemyAlive = false;

                    bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, lastTargetPosition,
                        _config.BulletSpeed * Time.deltaTime);

                    if (Vector3.Distance(bullet.transform.position, lastTargetPosition) < 0.1f)
                    {
                        _bulletsPool.Release(bullet);
                        isBulletFlying = false;
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }

        private void ClearTarget(Enemy.Enemy enemy)
        {
            enemy.Died -= ClearTarget;
            _target = null;
        }
    }
}