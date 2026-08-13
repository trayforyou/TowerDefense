using _Project.Scripts.Builds.Shooters;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;

        private Gun _gun;

        private void OnDestroy() =>
            _gun.Dispose();

        public void Initialize(Gun gun)
        {
            _gun = gun;
            _gun.SetShootPoint(_shootPoint.position);
        }

        public void Stop() =>
            _gun.Stop();
    }
}