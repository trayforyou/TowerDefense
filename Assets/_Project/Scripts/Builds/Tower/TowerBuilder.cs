using System;
using System.Collections.Generic;
using TowerDefense.ScriptableObjects;
using TowerDefense.Session;
using UnityEngine;

namespace TowerDefense.Builds.Tower
{
    public class TowerBuilder : MonoBehaviour
    {
        [SerializeField] private BuildMenu _buildMenu;
        [SerializeField] private Tower _strongTowerPrefab;
        [SerializeField] private Tower _fastTowerPrefab;

        private GameConfig _config;
        private Wallet _wallet;
        private List<Tower> _towers = new();
        private Vector3 _buildPosition;

        public event Action<Vector3> BuildingTower;

        public bool IsActive => _buildMenu.IsActive;

        private void OnDestroy() =>
            UnsubscribeAll();

        public void TurnOff() =>
            _buildMenu.Hide();

        public void Initialize(Wallet wallet, GameConfig config)
        {
            _config = config;
            _wallet = wallet;
            _buildMenu.SetCostTowers(_config.FastTowerCost, _config.StrongTowerCost);
            SubscribeAll();
        }

        public void Activate(Vector3 buildPosition)
        {
            _buildPosition = buildPosition;
            _buildMenu.Show();
        }

        public void StopAttack()
        {
            foreach (Tower tower in _towers)
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
            _buildMenu.SetCanBuyStrong(count >= _config.StrongTowerCost);
        }

        private void BuildTower(Tower prefab, float range, float shootDelay, int damage)
        {
            if (prefab == null)
                throw new NullReferenceException(nameof(prefab));

            Tower tempTower = (Instantiate(prefab, _buildPosition, Quaternion.identity));
            tempTower.Initialize(_config, range, shootDelay, damage);
            _towers.Add(tempTower);

            BuildingTower?.Invoke(tempTower.transform.position);

            _buildMenu.Hide();
        }
    }
}