using UnityEngine;

namespace _Project.Scripts.Builds.Shooters
{
    public readonly struct GunParameters
    {
        public readonly int Damage;
        public readonly float MainMultiplierDamage;
        public readonly float ShootDelay;
        public readonly float MainMultiplierDelay;
        public readonly float Radius;
        public readonly Vector3 CenterFindPosition;

        public GunParameters(int damage, float shootDelay, float radius, Vector3 centerFindPosition,
            float mainMultiplierDamage,float mainMultiplierDelay )
        {
            MainMultiplierDamage = mainMultiplierDamage;
            MainMultiplierDelay = mainMultiplierDelay;
            Damage = damage;
            ShootDelay = shootDelay;
            Radius = radius;
            CenterFindPosition = centerFindPosition;
        }
    }
}