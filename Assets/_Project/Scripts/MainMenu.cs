using System;
using _Project.Scripts.Savers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button _start;
        [SerializeField] private TextMeshProUGUI _metaCoins;

        private Saver _saver;
        private string _metaCoinsText;

        public event Action TriedLoadGame;

        private void Awake()
        {
            _saver = new Saver();
            
            SaveData data = _saver.Load();
            _metaCoins.text += data.MetaCurrency;
            _start.onClick.AddListener(ClickButton);
        }

        private void OnDestroy() => 
            _start.onClick.RemoveListener(ClickButton);

        private void ClickButton() =>
            TriedLoadGame?.Invoke();
    }
}