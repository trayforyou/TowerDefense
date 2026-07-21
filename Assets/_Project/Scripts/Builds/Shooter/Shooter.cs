using System;
using System.Collections;
using TowerDefense.ScriptableObjects;
using UnityEngine;

namespace TowerDefense.Builds.Shooter
{
    public class Shooter : MonoBehaviour
    {
        private GameConfig _config;
        private BulletsStorage _bulletsPool;
        private Vector3 _shootPoint;
        private WaitForSeconds _currentDelay;
        private Coroutine _attackCoroutine;

        public void Initialize(GameConfig config, int damage, float shootDelay, Bullet bulletPrefab, Vector3 shootPoint)
        {
            _bulletsPool = new BulletsStorage(bulletPrefab, config, damage);
            _currentDelay = new WaitForSeconds(shootDelay);
            _config = config;
            _shootPoint = shootPoint;
        }

        public void Attack(Enemy.Enemy enemy)
        {
            _attackCoroutine = StartCoroutine(StartAttack(enemy));
        }

        public void Stop()
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);
        }

        public void UpLevelSpeed(float newDelay) =>
            _currentDelay = new WaitForSeconds(newDelay);

        public void UpLevelDamage(int damage) =>
            _bulletsPool.ChangeDamage(damage);

        private IEnumerator StartAttack(Enemy.Enemy currentTarget)
        {
            yield return _currentDelay;

            while (currentTarget.isActiveAndEnabled)
            {
                if (currentTarget.isActiveAndEnabled == false)
                    yield break;

                StartCoroutine(FlyBullet(currentTarget));

                yield return _currentDelay;
            }
        }

        private IEnumerator FlyBullet(Enemy.Enemy target)
        {
            bool isEnemyAlive = true;
            Bullet bullet = _bulletsPool.Get();
            bullet.transform.position = _shootPoint;
            Vector3 lastTargetPosition = target.AimPoint.position;
            bool isBulletFlying = true;
            var wait = new WaitForFixedUpdate();

            while (isBulletFlying)
            {
                if (bullet == null)
                    yield break;

                if (target != null && target.isActiveAndEnabled && isEnemyAlive)
                {
                    lastTargetPosition = target.AimPoint.position;
                    bullet.transform.position = Vector3.MoveTowards(bullet.transform.position,
                        target.AimPoint.position, _config.BulletSpeed * Time.deltaTime);
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

                yield return wait;
            }
        }
    }
}