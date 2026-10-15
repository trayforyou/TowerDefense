using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _upgrade;
        [SerializeField] private TextMeshProUGUI _metaCoins;

        public event Action TriedLoadGame;
        public event Action OnClickUpgrade;

        public void Initialize(int metaCoins)
        {
            _metaCoins.text += metaCoins;
            
            _start.onClick.AddListener(ClickButtonStart);
            _upgrade.onClick.AddListener(ClickButtonUpgrade);
        }

        private void OnDestroy()
        {
            _start.onClick.RemoveListener(ClickButtonStart);
            _upgrade.onClick.RemoveListener(ClickButtonUpgrade);
        }

        private void ClickButtonStart() =>
            TriedLoadGame?.Invoke();
        
        private void ClickButtonUpgrade() =>
            OnClickUpgrade?.Invoke();
    }
}