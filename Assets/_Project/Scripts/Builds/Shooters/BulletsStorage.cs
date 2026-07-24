using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.Object;

namespace _Project.Scripts.Builds.Shooters
{
    public class BulletsStorage
    {
        private readonly ObjectPool<Bullet> _bulletsPool;
        private int _currentDamage;

        public BulletsStorage(Bullet bulletPrefab, GameConfig config, int damage)
        {
            _currentDamage = damage;
            var bulletPrefab1 = bulletPrefab;

            _bulletsPool = new ObjectPool<Bullet>
            (
                createFunc: () => Instantiate(bulletPrefab1),
                actionOnGet: GetBullet,
                actionOnRelease: ReleaseBullet,
                actionOnDestroy: bullet => Destroy(bullet.gameObject),
                collectionCheck: true,
                defaultCapacity: config.MinBulletPoolSize,
                maxSize: config.MaxBulletPoolSize
            );
        }

        public Bullet Get() =>
            _bulletsPool.Get();

        public void Release(Bullet bullet) =>
            _bulletsPool.Release(bullet);

        public void ChangeDamage(int damage) => 
            _currentDamage = damage;

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
    }
}