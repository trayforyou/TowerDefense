using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    public class TowerBuilder : MonoBehaviour
    {
        [SerializeField] private BuildMenu _buildMenu;
        [SerializeField] private Tower _strongTowerPrefab;
        [SerializeField] private Tower _fastTowerPrefab;

        private readonly List<Tower> _towers = new();
        private TowerConfig _strongTowerConfig;
        private TowerConfig _fastTowerConfig;
        private Castle _castle;
        private BuildConfig _buildingConfig;
        private BuildValidator _validator;
        private Wallet _wallet;
        private Vector3 _buildPosition;
        private ShooterConfig _shooterConfig;

        public bool IsActive => _buildMenu.IsActive;

        private void OnDestroy() =>
            UnsubscribeAll();

        public void TurnOff() =>
            _buildMenu.Hide();

        public void Initialize(ShooterConfig shooterConfig, Wallet wallet, BuildConfig buildConfig,
            TowerConfig fastTowerConfig, TowerConfig strongTowerConfig, Castle castle)
        {
            _fastTowerConfig = fastTowerConfig;
            _strongTowerConfig = strongTowerConfig;
            _validator = new BuildValidator(castle, buildConfig.MinDistanceForBuilding);
            _shooterConfig = shooterConfig;
            _castle = castle;
            _buildingConfig = buildConfig;
            _wallet = wallet;
            _buildMenu.SetCostTowers(_buildingConfig.FastTowerCost, _buildingConfig.StrongTowerCost);
            SubscribeAll();
        }

        public void Activate(Vector3 buildPosition)
        {
            if (_validator.TryValidateBuildPoint(buildPosition, _towers.ToList()))
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

        private void UnsubscribeAll()
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
            tempTower.Initialize(_shooterConfig, towerConfig);
            _towers.Add(tempTower);

            _buildMenu.Hide();
        }
    }
}