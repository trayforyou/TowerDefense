using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Builds
{
    [RequireComponent(typeof(CanvasGroup))]
    public class BuildMenu : MonoBehaviour
    {
        private const string CURRENCY_SYMBOL = "$";

        [SerializeField] private Button _buttonFastTower;
        [SerializeField] private TextMeshProUGUI _tMPFastTower;
        [SerializeField] private Button _buttonStrongTower;
        [SerializeField] private TextMeshProUGUI _tMPStrongTower;

        private CanvasGroup _canvasGroup;
        private bool _canBuyFast;
        private bool _canBuyStrong;

        public event Action TriedBuyFastTower;
        public event Action TriedBuyStrongTower;

        public bool IsActive { get; private set; }

        private void Awake() =>
            _canvasGroup = GetComponent<CanvasGroup>();

        private void Start()
        {
            Hide();
            ChangeView(_tMPFastTower, _canBuyFast);
            ChangeView(_tMPStrongTower, _canBuyStrong);
        }

        private void OnEnable()
        {
            _buttonFastTower.onClick.AddListener(ClickBuyFastTower);
            _buttonStrongTower.onClick.AddListener(ClickBuyStrongTower);
        }

        private void OnDisable()
        {
            _buttonFastTower.onClick.RemoveListener(ClickBuyFastTower);
            _buttonStrongTower.onClick.RemoveListener(ClickBuyStrongTower);
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

        public void SetCostTowers(int fastTower, int strongTower)
        {
            _tMPFastTower.text = fastTower + CURRENCY_SYMBOL;
            _tMPStrongTower.text = strongTower + CURRENCY_SYMBOL;
        }

        public void SetCanBuyFast(bool value)
        {
            if (_canBuyFast == value)
                return;

            _canBuyFast = value;
            ChangeView(_tMPFastTower, value);
        }

        private void ClickBuyFastTower() =>
            TriedBuyFastTower?.Invoke();

        private void ClickBuyStrongTower() =>
            TriedBuyStrongTower?.Invoke();

        private void ChangeView(TextMeshProUGUI tMpTower, bool value)
        {
            tMpTower.color = value ? Color.green : Color.red;
        }

        public void SetCanBuyStrong(bool value)
        {
            if (_canBuyStrong == value)
                return;

            _canBuyStrong = value;
            ChangeView(_tMPStrongTower, value);
        }
    }
}