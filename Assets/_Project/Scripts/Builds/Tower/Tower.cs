using TowerDefense.ScriptableObjects;
using UnityEngine;

namespace TowerDefense.Builds.Tower
{
    [RequireComponent(typeof(Shooter.Shooter))]
    public class Tower : MonoBehaviour
    {
        private Shooter.Shooter _shooter;

        private void Awake() =>
            _shooter = GetComponent<Shooter.Shooter>();

        public void Initialize(GameConfig config, float range,float delay,int damage) =>
            _shooter.Initialize(config, range, delay, damage);

        public void Stop() =>
            _shooter.Stop();
    } 
}