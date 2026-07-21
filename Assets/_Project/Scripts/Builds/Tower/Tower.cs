using TowerDefense.ScriptableObjects;
using UnityEngine;

namespace TowerDefense.Builds.Tower
{
    [RequireComponent(typeof(Shooter.Gun))]
    public class Tower : MonoBehaviour
    {
        private Shooter.Gun _gun;

        private void Awake() =>
            _gun = GetComponent<Shooter.Gun>();

        public void Initialize(GameConfig config, float range,float delay,int damage) =>
            _gun.Initialize(config, range, delay, damage);

        public void Stop() =>
            _gun.Stop();
    } 
}