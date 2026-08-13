using System;
using System.Collections;
using _Project.Scripts.Enemies;
using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    [RequireComponent(typeof(SphereCollider))]
    public class Bullet : MonoBehaviour
    {
        private SphereCollider _collider;
        private int _damage;
        private float _speed;

        public event Action<Bullet> Releasing;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Enemy enemy))
                enemy.TakeDamage(_damage);
        }

        public void SetParameters(int damage, float speed)
        {
            _speed = speed;
            _damage = damage;
        }

        public void Shoot(Vector3 startPosition, Enemy target)
        {
            StartCoroutine(FlyBullet(target, startPosition));
        }

        private IEnumerator FlyBullet(Enemy target, Vector3 startPoint)
        {
            bool isKilledCurrentEnemy = false;
            transform.position = startPoint;
            Vector3 lastTargetPosition = target.AimPoint.position;
            bool isBulletFlying = true;
            var wait = new WaitForFixedUpdate();

            while (isBulletFlying)
            {
                if (target != null && target.IsAlive && !isKilledCurrentEnemy)
                {
                    lastTargetPosition = target.AimPoint.position;
                    transform.position = Vector3.MoveTowards(transform.position,
                        target.AimPoint.position, _speed * Time.deltaTime);
                }
                else
                {
                    isKilledCurrentEnemy = true;

                    transform.position = Vector3.MoveTowards(transform.position, lastTargetPosition,
                        _speed * Time.deltaTime);

                    if (Vector3.Distance(transform.position, lastTargetPosition) < 0.1f)
                    {
                        Releasing?.Invoke(this);
                        isBulletFlying = false;
                    }
                }

                yield return wait;
            }
        }
    }
}