using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.MainMenuScene
{
    public class CardButton : MonoBehaviour
    {
        private const string MultiplySymbol = "X";
        private const string LevelText = "Lvl";
        private const string CurrencySymbol = "$";
        private const string GrowTextFormat = "0.00";

        private Color _affordableColor;
        private Color _unaffordableColor;

        private bool _canBuy;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _nameTextMesh;
        [SerializeField] private TextMeshProUGUI _levelTextMesh;
        [SerializeField] private TextMeshProUGUI _growthTextMesh;
        [SerializeField] private TextMeshProUGUI _costTextMesh;

        public event Action OnClick;

        private void OnDestroy()
        {
            if (_button == null)
                return;

            _button.onClick.RemoveListener(ClickButton);
        }

        public void Initialize(int cost)
        {
            _affordableColor = _costTextMesh.color;
            _unaffordableColor = Color.red;
            _canBuy = true;
            _button.onClick.AddListener(ClickButton);
            _costTextMesh.text = cost + CurrencySymbol;
        }

        public void SetLevel(int level, float multiplier)
        {
            _growthTextMesh.text = MultiplySymbol + multiplier.ToString(GrowTextFormat);
            _levelTextMesh.text = level + LevelText;
        }

        public void SetCanBuy(bool canBuy)
        {
            if (_canBuy == canBuy)
                return;

            _canBuy = canBuy;
            _button.interactable = canBuy;
            _costTextMesh.color = canBuy ? _affordableColor : _unaffordableColor;
        }

        private void ClickButton() =>
            OnClick?.Invoke();
    }
}