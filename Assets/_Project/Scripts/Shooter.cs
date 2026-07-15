using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Shooter : MonoBehaviour
{
    [SerializeField] private int _minPoolSize = 3;
    [SerializeField] private int _maxPoolSize = 10;
    [SerializeField] private float _findDelay = 0.5f;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _shootPoint;

    private ObjectPool<Bullet> _bulletsPool;
    private GameConfig _config;
    private Coroutine _attackCoroutine;
    private Enemy _target;
    private float _sqrRadius;
    private float _currentDelay;
    private int _currentDamage;
    private int _currentForceLevel = 1;
    private int _currentSpeedLevel = 1;

    private void Awake()
    {
        _config = GameManager.Instance.Config;
        _sqrRadius = _config.RadiusRangeCastle * _config.RadiusRangeCastle;
        _currentDamage = _config.StartDamageCastle;
        _currentDelay = _config.StartDelayShootCastle;

        _bulletsPool = new ObjectPool<Bullet>
            (
            createFunc: () => Instantiate(_bulletPrefab),
            actionOnGet: bullet => GetBullet(bullet),
            actionOnRelease: bullet => ReleaseBullet(bullet),
            actionOnDestroy: bullet => Destroy(bullet.gameObject),
            collectionCheck: true,
            defaultCapacity: _minPoolSize,
            maxSize: _maxPoolSize
            );
    }

    private void Start() =>
    StartCoroutine(StartFindNearestEnemy());

    public void Stop() =>
        StopAllCoroutines();

    public void UpSpeedLevel()
    {
        if (_currentSpeedLevel > _config.MaxCastleLevel)
            return;

        _currentSpeedLevel++;
        _currentDelay = _config.StartDelayShootCastle;

        for (int i = 1; i < _currentSpeedLevel; i++)
            _currentDelay = _currentDelay / _config.UpgradeMultiplier;

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
        var wait = new WaitForSeconds(_findDelay);

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

    private Enemy FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(_shootPoint.position, _config.RadiusRangeCastle);

        if (colliders.Length == 0)
            return null;

        float minSqrDistance = float.MaxValue;
        Enemy nearestEnemy = null;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
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

    private IEnumerator StartAttack(Enemy currentTarget)
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

        Enemy currentTarget = _target;
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
                bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, currentTarget.AimPoint.position, _config.BulletSpeed * Time.deltaTime);
            }
            else
            {
                isEnemyAlive = false;

                bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, lastTargetPosition, _config.BulletSpeed * Time.deltaTime);

                if (Vector3.Distance(bullet.transform.position, lastTargetPosition) < 0.1f)
                {
                    _bulletsPool.Release(bullet);
                    isBulletFlying = false;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void ClearTarget(Enemy enemy)
    {
        enemy.Died -= ClearTarget;
        _target = null;
    }
}