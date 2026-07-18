using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private TextMeshProUGUI _metaCoins;

        private string _metaCoinsText;

        public event Action TriedLoadGame;

        private void Awake()
        {
            _metaCoins.text += PlayerPrefs.GetInt(GameStarter.META_CURRENCY, 0);
            _start.onClick.AddListener(ClickButton);
        }

        private void OnDisable() =>
            _start.onClick.RemoveListener(ClickButton);

        private void ClickButton() =>
            TriedLoadGame?.Invoke();
    }
}