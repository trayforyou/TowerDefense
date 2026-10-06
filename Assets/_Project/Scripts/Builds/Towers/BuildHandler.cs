using System;
using _Project.Scripts.Session;
using UnityEngine;

namespace _Project.Scripts.Builds.Towers
{
    public class BuildHandler : IDisposable
    {
        private BuildValidator _validator;
        private TowerBuilder _strongTowerBuilder;
        private TowerBuilder _fastTowerBuilder;
        private BuildMenu _buildMenu;
        private Wallet _wallet;
        private Vector3 _buildPosition;

        public event Action<int> TowerBuilt;
        public event Action TooCloseRejected;
        public event Action NoMoneyRejected;

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
            else
            {
                TooCloseRejected?.Invoke();
            }
        }

        private void SubscribeAll()
        {
            _wallet.ValueChanged += ChangeOpportunitiesBuy;
            _buildMenu.TriedBuyFastTower += BuildFastTower;
            _buildMenu.TriedBuyStrongTower += BuildStrongTower;
            _fastTowerBuilder.Built += ProcessTower;
            _strongTowerBuilder.Built += ProcessTower;
        }

        private void UnSubscribeAll()
        {
            _buildMenu.TriedBuyFastTower -= BuildFastTower;
            _buildMenu.TriedBuyStrongTower -= BuildStrongTower;
            _wallet.ValueChanged -= ChangeOpportunitiesBuy;
            _fastTowerBuilder.Built -= ProcessTower;
            _strongTowerBuilder.Built -= ProcessTower;
        }

        private void ProcessTower(Tower tower, int cost)
        {
            if (tower != null)
                _validator.AddTower(tower);

            TowerBuilt?.Invoke(cost);

            _buildMenu.Hide();
        }

        private void BuildFastTower()
        {
            if (_fastTowerBuilder.Build(_buildPosition) == false)
                NoMoneyRejected?.Invoke();
        }

        private void BuildStrongTower()
        {
            if (_strongTowerBuilder.Build(_buildPosition) == false)
                NoMoneyRejected?.Invoke();
        }

        private void ChangeOpportunitiesBuy(int count)
        {
            _buildMenu.SetCanBuyFastTower(count >= _fastTowerBuilder.Cost);
            _buildMenu.SetCanBuyStrongTower(count >= _strongTowerBuilder.Cost);
        }

        public void Dispose() =>
            UnSubscribeAll();
    }
}