using System;
using _Project.Scripts.Session;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Builds
{
    public class UpgradeMenu : GameInteractionMenu, IUIElement
    {
        [SerializeField] private Button _buttonHealthier;
        [SerializeField] private Button _buttonFaster;
        [SerializeField] private Button _buttonForce;
        [SerializeField] private TextMeshProUGUI _tMPHealthier;
        [SerializeField] private TextMeshProUGUI _tMPFaster;
        [SerializeField] private TextMeshProUGUI _tMPForce;

        public event Action TriedUpHealth;
        public event Action TriedUpSpeed;
        public event Action TriedUpForce;

        protected override void Start()
        {
            base.Start();

            SetAvailabilityView(_tMPHealthier, false);
            SetAvailabilityView(_tMPFaster, false);
            SetAvailabilityView(_tMPForce, false);

            SubscribeButton(_buttonHealthier, OnTryUpHealth);
            SubscribeButton(_buttonFaster, OnTryUpSpeed);
            SubscribeButton(_buttonForce, OnTryUpForce);
        }

        protected override void OnDestroy()
        {
            UnsubscribeButton(_buttonHealthier, OnTryUpHealth);
            UnsubscribeButton(_buttonFaster, OnTryUpSpeed);
            UnsubscribeButton(_buttonForce, OnTryUpForce);
        }

        public void SetStartCost(int cost)
        {
            SetCostText(_tMPHealthier, cost);
            SetCostText(_tMPFaster, cost);
            SetCostText(_tMPForce, cost);
        }

        public void ChangeCostUpgradeHealth(int cost) =>
            SetCostText(_tMPHealthier, cost);

        public void ChangeCostUpgradeSpeed(int cost) =>
            SetCostText(_tMPFaster, cost);

        public void ChangeCostUpgradeForce(int cost) =>
            SetCostText(_tMPForce, cost);

        public void SetCanUpHealth(bool value) =>
            SetAvailabilityView(_tMPHealthier, value);

        public void SetCanUpSpeed(bool value) =>
            SetAvailabilityView(_tMPFaster, value);

        public void SetCanUpForce(bool value) =>
            SetAvailabilityView(_tMPForce, value);

        private void OnTryUpHealth() =>
            TriedUpHealth?.Invoke();

        private void OnTryUpSpeed() =>
            TriedUpSpeed?.Invoke();

        private void OnTryUpForce() =>
            TriedUpForce?.Invoke();
    }
}