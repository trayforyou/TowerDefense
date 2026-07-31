using _Project.Scripts.Builds.Shooters;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    [RequireComponent(typeof(Gun))]
    public class Tower : MonoBehaviour
    {
        private Gun _gun;

        private void Awake() =>
            _gun = GetComponent<Gun>();

        public void Initialize(ShooterConfig shooterConfig, TowerConfig towerConfig) =>
            _gun.Initialize(shooterConfig, towerConfig.RangeAttack, towerConfig.ShootDelay, towerConfig.Damage);

        public void Stop() =>
            _gun.Stop();
    }
}