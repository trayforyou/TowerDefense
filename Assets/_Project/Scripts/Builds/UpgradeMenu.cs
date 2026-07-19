using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.Builds
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UpgradeMenu : MonoBehaviour
    {
        private const string CURRENCY_SYMBOL = "$";

        [SerializeField] private Button _buttonHealthier;
        [SerializeField] private Button _buttonFaster;
        [SerializeField] private Button _buttonStronger;
        [SerializeField] private TextMeshProUGUI _tMPHealthier;
        [SerializeField] private TextMeshProUGUI _tMPFaster;
        [SerializeField] private TextMeshProUGUI _tMPStronger;

        private CanvasGroup _canvasGroup;

        public event Action TriedUpHealth;
        public event Action TriedUpSpeed;
        public event Action TriedUpStrong;

        [field: SerializeField] public bool IsActive { get; private set; } = false;

        private void Awake() =>
            _canvasGroup = GetComponent<CanvasGroup>();

        private void Start()
        {
            Hide();

            ChangeView(_tMPHealthier, false);
            ChangeView(_tMPFaster, false);
            ChangeView(_tMPStronger, false);
        }

        private void OnEnable()
        {
            _buttonFaster.onClick.AddListener(TryUpSpeed);
            _buttonHealthier.onClick.AddListener(TryUpHealth);
            _buttonStronger.onClick.AddListener(TryUpStrong);
        }

        private void OnDisable()
        {
            _buttonFaster.onClick.RemoveListener(TryUpSpeed);
            _buttonHealthier.onClick.RemoveListener(TryUpHealth);
            _buttonStronger.onClick.RemoveListener(TryUpStrong);
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
            ChangeCostUpgradeStrong(cost);
        }

        public void ChangeCostUpgradeHealth(int cost) =>
            _tMPHealthier.text = cost + CURRENCY_SYMBOL;

        public void ChangeCostUpgradeSpeed(int cost) =>
            _tMPFaster.text = cost + CURRENCY_SYMBOL;

        public void ChangeCostUpgradeStrong(int cost) =>
            _tMPStronger.text = cost + CURRENCY_SYMBOL;

        public void SetCanUpHealth(bool value) =>
            ChangeView(_tMPHealthier, value);

        public void SetCanUpSpeed(bool value) =>
            ChangeView(_tMPFaster, value);

        public void SetCanUpStrong(bool value) =>
            ChangeView(_tMPStronger, value);

        private void TryUpHealth() =>
            TriedUpHealth?.Invoke();

        private void TryUpSpeed() =>
            TriedUpSpeed?.Invoke();

        private void TryUpStrong() =>
            TriedUpStrong?.Invoke();

        private void ChangeView(TextMeshProUGUI tMPTower, bool value) =>
            tMPTower.color = value ? Color.green : Color.red;
    }
}