using System;
using System.Collections.Generic;
using _Project.Scripts.Builds.Shooters;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;
using UnityEngine;
using static UnityEngine.Object;

namespace _Project.Scripts.Builds.Towers
{
    public class TowerBuilder
    {
        private readonly List<Tower> _towers = new();
        private Func<GunParameters, Gun> _createGun;
        private TowerConfig _strongTowerConfig;
        private TowerConfig _fastTowerConfig;
        private BuildConfig _buildingConfig;
        private BuildValidator _validator;
        private Wallet _wallet;
        private Vector3 _buildPosition;

        private BuildMenu _buildMenu;
        private Tower _strongTowerPrefab;
        private Tower _fastTowerPrefab;

        public bool IsActive => _buildMenu.IsActive;

        public void TurnOff() =>
            _buildMenu.Hide();

        public TowerBuilder(Wallet wallet, BuildConfig buildConfig,
            TowerConfig fastTowerConfig, TowerConfig strongTowerConfig, BuildValidator validator, Tower fastTowerPrefab,
            Tower strongTowerPrefab, BuildMenu buildMenu, Func<GunParameters, Gun> createGun)
        {
            _createGun = createGun;
            _fastTowerPrefab = fastTowerPrefab;
            _strongTowerPrefab = strongTowerPrefab;
            _buildMenu = buildMenu;
            _fastTowerConfig = fastTowerConfig;
            _strongTowerConfig = strongTowerConfig;
            _validator = validator;
            _buildingConfig = buildConfig;
            _wallet = wallet;
            _buildMenu.SetCostTowers(_buildingConfig.FastTowerCost, _buildingConfig.StrongTowerCost);
            SubscribeAll();
        }

        public void Activate(Vector3 buildPosition)
        {
            if (_validator.TryValidateBuildPoint(buildPosition))
            {
                _buildPosition = buildPosition;
                _buildMenu.Show();
            }
        }

        public void StopAttack()
        {
            foreach (var tower in _towers)
                tower.Stop();
        }

        private void SubscribeAll()
        {
            _wallet.ValueChanged += ChangeOpportunitiesBuy;
            _buildMenu.TriedBuyFastTower += BuildFastTower;
            _buildMenu.TriedBuyStrongTower += BuildStrongTower;
        }

        public void UnSubscribeAll()
        {
            _buildMenu.TriedBuyFastTower -= BuildFastTower;
            _buildMenu.TriedBuyStrongTower -= BuildStrongTower;
            _wallet.ValueChanged -= ChangeOpportunitiesBuy;
        }

        private void BuildStrongTower()
        {
            if (_wallet.TryTakeMoney(_buildingConfig.StrongTowerCost))
                BuildTower(_strongTowerPrefab, _strongTowerConfig);
            else
                _buildMenu.Hide();
        }

        private void BuildFastTower()
        {
            if (_wallet.TryTakeMoney(_buildingConfig.FastTowerCost))
                BuildTower(_fastTowerPrefab, _fastTowerConfig);
            else
                _buildMenu.Hide();
        }

        private void ChangeOpportunitiesBuy(int count)
        {
            _buildMenu.SetCanBuyFast(count >= _buildingConfig.FastTowerCost);
            _buildMenu.SetCanBuyStrongTower(count >= _buildingConfig.StrongTowerCost);
        }

        private void BuildTower(Tower prefab, TowerConfig towerConfig)
        {
            if (prefab == null)
                throw new NullReferenceException(nameof(prefab));

            Tower tempTower = (Instantiate(prefab, _buildPosition, Quaternion.identity));
            
            _validator.AddTower(tempTower);
            tempTower.Initialize(_createGun.Invoke(new GunParameters(towerConfig.Damage, towerConfig.ShootDelay,
                towerConfig.RadiusAttack, _buildPosition)));

            _towers.Add(tempTower);

            _buildMenu.Hide();
        }
    }
}