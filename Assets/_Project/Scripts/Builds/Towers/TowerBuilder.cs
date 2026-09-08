using System;
using _Project.Scripts.Builds.Shooters;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;
using UnityEngine;
using static UnityEngine.Object;

namespace _Project.Scripts.Builds.Towers
{
    public class TowerBuilder
    {
        private readonly Func<GunParameters, Gun> _createGun;
        private readonly TowerConfig _config;
        private readonly Tower _prefab;
        private readonly Wallet _wallet;

        public readonly int Cost;
        private readonly FiringSwitch _firingSwitch;

        public event Action<Tower> Built;

        public TowerBuilder(Tower prefab, TowerConfig config, Wallet wallet, int cost,
            Func<GunParameters, Gun> createGun, FiringSwitch firingSwitch)
        {
            Cost = cost;
            _firingSwitch  = firingSwitch;
            _createGun = createGun;
            _config = config;
            _prefab = prefab;
            _wallet = wallet;
        }

        public void Build(Vector3 buildPosition)
        {
            Tower tempTower = null;

            if (_wallet.TryTakeMoney(Cost))
            {
                tempTower = (Instantiate(_prefab, buildPosition, Quaternion.identity));

                tempTower.Initialize(_createGun.Invoke(new GunParameters(_config.Damage, _config.ShootDelay,
                    _config.RadiusAttack, buildPosition)), _firingSwitch);
            }

            Built?.Invoke(tempTower);
        }
    }
}