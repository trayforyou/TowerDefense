using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class CardButton : MonoBehaviour
    {
        private readonly Color _affordableColor = Color.green;
        private readonly Color _unaffordableColor = Color.red;

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _nameTextMesh;
        [SerializeField] private TextMeshProUGUI _levelTextMesh;
        [SerializeField] private TextMeshProUGUI _growthTextMesh;
        [SerializeField] private TextMeshProUGUI _costTextMesh;

        public event Action OnClick;

        public int Cost { get; private set; } = 0;

        public void Initialize(int cost)
        {
            _button.onClick.AddListener(ClickButton);
            Cost = cost;
        }

        private void ClickButton() => 
            OnClick?.Invoke();

        private void OnDestroy()
        {
            if (_button == null)
                return;
                
            _button.onClick.RemoveListener(ClickButton);
        }

        public void SetCanBuy(bool canBuy)
        {
            _costTextMesh.color = canBuy ? _affordableColor : _unaffordableColor;
            
            // TO DO Сделать чтобы кнопка включалась\выключалась а не только перекрашивалась
        }
    }
}