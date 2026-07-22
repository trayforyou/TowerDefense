using System;
using TMPro;
using TowerDefense.Saver;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense
{
    [RequireComponent(typeof(Saver.Saver))]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private TextMeshProUGUI _metaCoins;

        private Saver.Saver _saver;
        private string _metaCoinsText;

        public event Action TriedLoadGame;

        private void Awake()
        {
            _saver = GetComponent<Saver.Saver>();
            
            SaveData data = _saver.Load();
            _metaCoins.text += data.MetaCurrency;
            _start.onClick.AddListener(ClickButton);
        }

        private void OnDisable() =>
            _start.onClick.RemoveListener(ClickButton);

        private void ClickButton() =>
            TriedLoadGame?.Invoke();
    }
}