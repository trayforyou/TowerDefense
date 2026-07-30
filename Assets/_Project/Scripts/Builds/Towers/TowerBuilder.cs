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
        private Castle _castle;
        private GameConfig _config;
        private BuildValidator _validator;
        private Wallet _wallet;
        private Vector3 _buildPosition;

        public bool IsActive => _buildMenu.IsActive;

        private void OnDestroy() =>
            UnsubscribeAll();

        public void TurnOff() =>
            _buildMenu.Hide();

        public void Initialize(Wallet wallet, GameConfig config, Castle castle)
        { 
            _validator = new BuildValidator(castle, config.MinDistanceForBuilding);
            _castle = castle;
            _config = config;
            _wallet = wallet;
            _buildMenu.SetCostTowers(_config.FastTowerCost, _config.StrongTowerCost);
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
            if (_wallet.TryTakeMoney(_config.StrongTowerCost))
                BuildTower(_strongTowerPrefab, _config.RadiusRangeStrongTower, _config.DelayShootStrongTower,
                    _config.DamageStrongTower);
            else
                _buildMenu.Hide();
        }

        private void BuildFastTower()
        {
            if (_wallet.TryTakeMoney(_config.FastTowerCost))
                BuildTower(_fastTowerPrefab, _config.RadiusRangeFastTower, _config.DelayShootFastTower,
                    _config.DamageFastTower);
            else
                _buildMenu.Hide();
        }

        private void ChangeOpportunitiesBuy(int count)
        {
            _buildMenu.SetCanBuyFast(count >= _config.FastTowerCost);
            _buildMenu.SetCanBuyStrongTower(count >= _config.StrongTowerCost);
        }

        private void BuildTower(Tower prefab, float range, float shootDelay, int damage)
        {
            if (prefab == null)
                throw new NullReferenceException(nameof(prefab));

            Tower tempTower = (Instantiate(prefab, _buildPosition, Quaternion.identity));
            tempTower.Initialize(_config, range, shootDelay, damage);
            _towers.Add(tempTower);

            _buildMenu.Hide();
        }
    }
}