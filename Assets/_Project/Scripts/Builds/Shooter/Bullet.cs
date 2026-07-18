using UnityEngine;

namespace TowerDefense.Builds.Shooter
{
    [RequireComponent(typeof(SphereCollider))]
    public class Bullet : MonoBehaviour
    {
        private SphereCollider _collider;
        private int _damage;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy.Enemy enemy))
                enemy.TakeDamage(_damage);
        }

        public void SetDamage(int damage) =>
            _damage = damage;
    }
}