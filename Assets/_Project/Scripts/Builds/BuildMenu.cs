using System;
using _Project.Scripts.Builds.Castles;
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
        private bool _canBuyFastTower;
        private bool _canBuyStrongTower;

        public event Action TriedBuyFastTower;
        public event Action TriedBuyStrongTower;

        public bool IsActive { get; private set; }

        private void Awake() =>
            _canvasGroup = GetComponent<CanvasGroup>();

        private void Start()
        {
            Hide();
            ChangeView(_tMPFastTower, _canBuyFastTower);
            ChangeView(_tMPStrongTower, _canBuyStrongTower);
            _buttonFastTower.onClick.AddListener(ClickBuyFastTower);
            _buttonStrongTower.onClick.AddListener(ClickBuyStrongTower);
        }

        private void OnDestroy()
        {
            _buttonFastTower.onClick.RemoveListener(ClickBuyFastTower);
            _buttonStrongTower.onClick.RemoveListener(ClickBuyStrongTower);
        }

        public void Show()
        {
            _canvasGroup.Show();
            IsActive = true;
        }

        public void Hide()
        {
            _canvasGroup.Hide();
            IsActive = false;
        }

        public void SetCostTowers(int fastTower, int strongTower)
        {
            _tMPFastTower.text = fastTower + CURRENCY_SYMBOL;
            _tMPStrongTower.text = strongTower + CURRENCY_SYMBOL;
        }

        public void SetCanBuyFast(bool value)
        {
            if (_canBuyFastTower == value)
                return;

            _canBuyFastTower = value;
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

        public void SetCanBuyStrongTower(bool value)
        {
            if (_canBuyStrongTower == value)
                return;

            _canBuyStrongTower = value;
            ChangeView(_tMPStrongTower, value);
        }
    }
}