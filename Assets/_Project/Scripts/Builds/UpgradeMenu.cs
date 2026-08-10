using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Builds
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UpgradeMenu : MonoBehaviour
    {
        private const string CURRENCY_SYMBOL = "$";

        [SerializeField] private Button _buttonHealthier;
        [SerializeField] private Button _buttonFaster;
        [SerializeField] private Button _buttonForce;
        [SerializeField] private TextMeshProUGUI _tMPHealthier;
        [SerializeField] private TextMeshProUGUI _tMPFaster;
        [SerializeField] private TextMeshProUGUI _tMPForce;

        private CanvasGroup _canvasGroup;

        public event Action TriedUpHealth;
        public event Action TriedUpSpeed;
        public event Action TriedUpForce;

        [field: SerializeField] public bool IsActive { get; private set; }

        private void Awake() =>
            _canvasGroup = GetComponent<CanvasGroup>();

        private void Start()
        {
            Hide();

            ChangeView(_tMPHealthier, false);
            ChangeView(_tMPFaster, false);
            ChangeView(_tMPForce, false);
            _buttonFaster.onClick.AddListener(TryUpSpeed);
            _buttonHealthier.onClick.AddListener(TryUpHealth);
            _buttonForce.onClick.AddListener(TryUpForce);
        }

        private void OnDestroy()
        {
            _buttonFaster.onClick.RemoveListener(TryUpSpeed);
            _buttonHealthier.onClick.RemoveListener(TryUpHealth);
            _buttonForce.onClick.RemoveListener(TryUpForce);
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            IsActive = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            IsActive = false;
        }

        public void SetStartCost(int cost)
        {
            ChangeCostUpgradeHealth(cost);
            ChangeCostUpgradeSpeed(cost);
            ChangeCostUpgradeForce(cost);
        }

        public void ChangeCostUpgradeHealth(int cost) =>
            _tMPHealthier.text = cost + CURRENCY_SYMBOL;

        public void ChangeCostUpgradeSpeed(int cost) =>
            _tMPFaster.text = cost + CURRENCY_SYMBOL;

        public void ChangeCostUpgradeForce(int cost) =>
            _tMPForce.text = cost + CURRENCY_SYMBOL;

        public void SetCanUpHealth(bool value) =>
            ChangeView(_tMPHealthier, value);

        public void SetCanUpSpeed(bool value) =>
            ChangeView(_tMPFaster, value);

        public void SetCanUpForce(bool value) =>
            ChangeView(_tMPForce, value);

        private void TryUpHealth() =>
            TriedUpHealth?.Invoke();

        private void TryUpSpeed() =>
            TriedUpSpeed?.Invoke();

        private void TryUpForce() =>
            TriedUpForce?.Invoke();

        private void ChangeView(TextMeshProUGUI tMpTower, bool value) =>
            tMpTower.color = value ? Color.green : Color.red;
    }
}