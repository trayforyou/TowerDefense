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

        public void Initialize(GameConfig config, float range,float delay,int damage) =>
            _gun.Initialize(config, range, delay, damage);

        public void Stop() =>
            _gun.Stop();
    } 
}