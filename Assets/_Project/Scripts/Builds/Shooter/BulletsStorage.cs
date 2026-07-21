using static UnityEngine.Object;
using TowerDefense.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;

namespace TowerDefense.Builds.Shooter
{
    public class BulletsStorage
    {
        private ObjectPool<Bullet> _bulletsPool;
        private Bullet _bulletPrefab;
        private GameConfig _config;
        private int _currentDamage;

        public BulletsStorage(Bullet bulletPrefab, GameConfig config, int damage)
        {
            _currentDamage = damage;
            _config = config;
            _bulletPrefab = bulletPrefab;

            _bulletsPool = new ObjectPool<Bullet>
            (
                createFunc: () => Instantiate(_bulletPrefab),
                actionOnGet: GetBullet,
                actionOnRelease: ReleaseBullet,
                actionOnDestroy: bullet => Destroy(bullet.gameObject),
                collectionCheck: true,
                defaultCapacity: _config.MinBulletPoolSize,
                maxSize: _config.MaxBulletPoolSize
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