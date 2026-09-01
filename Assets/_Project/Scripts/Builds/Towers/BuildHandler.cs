using System;
using System.Collections.Generic;
using _Project.Scripts.Session;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    public class BuildHandler : IDisposable
    {
        private readonly List<Tower> _towers = new();
        private BuildValidator _validator;
        private TowerBuilder _strongTowerBuilder;
        private TowerBuilder _fastTowerBuilder;
        private BuildMenu _buildMenu;
        private Wallet _wallet;
        private Vector3 _buildPosition;

        public bool IsActive => _buildMenu.IsActive;

        public void TurnOff() =>
            _buildMenu.Hide();

        public BuildHandler(Wallet wallet, BuildValidator validator, BuildMenu buildMenu,
            TowerBuilder strongBuilder, TowerBuilder fastBuilder)
        {
            _buildMenu = buildMenu;
            _fastTowerBuilder = fastBuilder;
            _strongTowerBuilder = strongBuilder;
            _validator = validator;
            _wallet = wallet;

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

        private void SubscribeAll()
        {
            _wallet.ValueChanged += ChangeOpportunitiesBuy;
            _buildMenu.TriedBuyFastTower += BuildFastTower;
            _buildMenu.TriedBuyStrongTower += BuildStrongTower;
            _fastTowerBuilder.Builded += ProcessTower;
            _strongTowerBuilder.Builded += ProcessTower;
        }

        private void UnSubscribeAll()
        {
            _buildMenu.TriedBuyFastTower -= BuildFastTower;
            _buildMenu.TriedBuyStrongTower -= BuildStrongTower;
            _wallet.ValueChanged -= ChangeOpportunitiesBuy;
            _fastTowerBuilder.Builded -= ProcessTower;
            _strongTowerBuilder.Builded -= ProcessTower;
        }

        private void ProcessTower(Tower tower)
        {
            if (tower != null)
            {
                _validator.AddTower(tower);
                _towers.Add(tower);
            }

            _buildMenu.Hide();
        }

        private void BuildFastTower() =>
            _fastTowerBuilder.Build(_buildPosition);

        private void BuildStrongTower() =>
            _strongTowerBuilder.Build(_buildPosition);

        private void ChangeOpportunitiesBuy(int count)
        {
            _buildMenu.SetCanBuyFast(count >= _fastTowerBuilder.Cost);
            _buildMenu.SetCanBuyStrongTower(count >= _strongTowerBuilder.Cost);
        }

        public void Dispose() => 
            UnSubscribeAll();
    }
}