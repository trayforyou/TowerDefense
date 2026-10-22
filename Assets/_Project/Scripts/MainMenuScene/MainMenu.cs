using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.MainMenuScene
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private Button _upgrade;
        [SerializeField] private TextMeshProUGUI _metaCoins;

        private MetaMoneyBank _metaMoneyBank;

        public event Action TriedLoadGame;
        public event Action OnClickUpgrade;

        private void OnDestroy()
        {
            _metaMoneyBank.OnValueChanged -= ChangeValue;
            _start.onClick.RemoveListener(ClickButtonStart);
            _upgrade.onClick.RemoveListener(ClickButtonUpgrade);
        }
        
        public void Initialize(MetaMoneyBank bank)
        {
            ChangeValue(bank.GetBalance());
            
            _metaMoneyBank = bank;

            _metaMoneyBank.OnValueChanged += ChangeValue;
            _start.onClick.AddListener(ClickButtonStart);
            _upgrade.onClick.AddListener(ClickButtonUpgrade);
        }

        private void ChangeValue(int value) => 
            _metaCoins.text = value.ToString();

        private void ClickButtonStart() =>
            TriedLoadGame?.Invoke();

        private void ClickButtonUpgrade() =>
            OnClickUpgrade?.Invoke();
    }
}