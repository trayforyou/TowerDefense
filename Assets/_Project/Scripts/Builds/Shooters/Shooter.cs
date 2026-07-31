using System.Collections;
using _Project.Scripts.Enemies;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    public class Shooter : MonoBehaviour
    {
        private BulletsStorage _bulletsPool;
        private Vector3 _shootPoint;
        private WaitForSeconds _currentDelay;
        private Coroutine _attackCoroutine;

        public void Initialize(ShooterConfig config, int damage, float shootDelay, Bullet bulletPrefab,
            Vector3 shootPoint)
        {
            _bulletsPool = new BulletsStorage(bulletPrefab, config, damage);
            _currentDelay = new WaitForSeconds(shootDelay);
            _shootPoint = shootPoint;
        }

        public void Attack(Enemy enemy)
        {
            _attackCoroutine = StartCoroutine(StartAttack(enemy));
        }

        public void Stop()
        {
            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);
        }

        public void SetShootDelay(float newDelay) =>
            _currentDelay = new WaitForSeconds(newDelay);

        public void SetDamage(int damage) =>
            _bulletsPool.ChangeDamage(damage);

        private IEnumerator StartAttack(Enemy currentTarget)
        {
            yield return _currentDelay;
            Bullet tempBullet;

            while (currentTarget.IsAlive)
            {
                tempBullet = _bulletsPool.Get();
                tempBullet.Releasing += Release;
                tempBullet.Shoot(_shootPoint, currentTarget);

                yield return _currentDelay;
            }
        }

        private void Release(Bullet bullet)
        {
            bullet.Releasing -= Release;
            _bulletsPool.Release(bullet);
        }
    }
}