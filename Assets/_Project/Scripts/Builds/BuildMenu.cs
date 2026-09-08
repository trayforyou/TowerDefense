using System;
using _Project.Scripts.Session;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Builds
{
    public class BuildMenu : GameInteractionMenu, IUIElement
    {
        [SerializeField] private Button _buttonFastTower;
        [SerializeField] private TextMeshProUGUI _tMPFastTower;
        [SerializeField] private Button _buttonStrongTower;
        [SerializeField] private TextMeshProUGUI _tMPStrongTower;

        private bool _canBuyFastTower;
        private bool _canBuyStrongTower;

        public event Action TriedBuyFastTower;
        public event Action TriedBuyStrongTower;

        protected override void Start()
        {
            base.Start();

            SetAvailabilityView(_tMPFastTower, _canBuyFastTower);
            SetAvailabilityView(_tMPStrongTower, _canBuyStrongTower);

            SubscribeButton(_buttonFastTower, OnClickBuyFastTower);
            SubscribeButton(_buttonStrongTower, OnClickBuyStrongTower);
        }

        protected override void OnDestroy()
        {
            UnsubscribeButton(_buttonFastTower, OnClickBuyFastTower);
            UnsubscribeButton(_buttonStrongTower, OnClickBuyStrongTower);
        }

        public void SetCostTowers(int fastTower, int strongTower)
        {
            SetCostText(_tMPFastTower, fastTower);
            SetCostText(_tMPStrongTower, strongTower);
        }

        public void SetCanBuyFastTower(bool value)
        {
            if (_canBuyFastTower == value)
                return;

            _canBuyFastTower = value;
            SetAvailabilityView(_tMPFastTower, value);
        }

        public void SetCanBuyStrongTower(bool value)
        {
            if (_canBuyStrongTower == value)
                return;

            _canBuyStrongTower = value;
            SetAvailabilityView(_tMPStrongTower, value);
        }

        private void OnClickBuyFastTower() =>
            TriedBuyFastTower?.Invoke();

        private void OnClickBuyStrongTower() =>
            TriedBuyStrongTower?.Invoke();
    }
}