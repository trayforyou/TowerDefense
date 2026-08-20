using _Project.Scripts.Builds.Shooters;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;

        private Gun _gun;
        private FiringSwitch _firingSwitch;

        private void OnDestroy()
        {
            _firingSwitch.OnStopFire -= Stop;
            _gun.Dispose();
        }

        public void Initialize(Gun gun, FiringSwitch firingSwitch)
        {
            _firingSwitch = firingSwitch;
            _gun = gun;
            _gun.SetShootPoint(_shootPoint.position);
            
            _firingSwitch.OnStopFire += Stop;
        }

        private void Stop()
        {
            _firingSwitch.OnStopFire -= Stop;
            _gun.Stop();
        }
    }
}