using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private TextMeshProUGUI _metaCoins;

        public event Action TriedLoadGame;

        public void Initialize(int metaCoins)
        {
            _metaCoins.text += metaCoins;
            
            _start.onClick.AddListener(ClickButton);
        }

        private void OnDestroy() =>
            _start.onClick.RemoveListener(ClickButton);

        private void ClickButton() =>
            TriedLoadGame?.Invoke();
    }
}