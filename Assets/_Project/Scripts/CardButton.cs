using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class CardButton : MonoBehaviour
    {
        private const string MultiplySymbol = "X";
        private const string LevelText = "Lvl";
        private const string CurrencySymbol = "$";

        private readonly Color _affordableColor = Color.green;
        private readonly Color _unaffordableColor = Color.red;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _nameTextMesh;
        [SerializeField] private TextMeshProUGUI _levelTextMesh;
        [SerializeField] private TextMeshProUGUI _growthTextMesh;
        [SerializeField] private TextMeshProUGUI _costTextMesh;

        public event Action OnClick;

        public void Initialize(int cost)
        {
            _button.onClick.AddListener(ClickButton);
            _costTextMesh.text = cost + CurrencySymbol;
        }

        private void ClickButton() =>
            OnClick?.Invoke();

        private void OnDestroy()
        {
            if (_button == null)
                return;

            _button.onClick.RemoveListener(ClickButton);
        }

        public void SetlLevel(int level, float multiplier)
        {
            _growthTextMesh.text = MultiplySymbol + level;
            _levelTextMesh.text = level + LevelText;
        }

        public void SetCanBuy(bool canBuy)
        {
            _button.interactable = canBuy;
            _costTextMesh.color = canBuy ? _affordableColor : _unaffordableColor;
        }
    }
}