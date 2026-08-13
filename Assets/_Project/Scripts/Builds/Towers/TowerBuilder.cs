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
        private Tower _prefab;
        private TowerConfig _config;
        private Wallet _wallet;
        private Action<Tower, TowerConfig> _build;
        private Func<GunParameters, Gun> _createGun;

        public readonly int Cost;

        public event Action<Tower> Builded;

        public TowerBuilder(Tower prefab, TowerConfig config, Wallet wallet, int cost, BuildValidator validator,
            Func<GunParameters, Gun> createGun)
        {
            Cost = cost;
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
                    _config.RadiusAttack, buildPosition)));
            }

            Builded?.Invoke(tempTower);
        }
    }
}