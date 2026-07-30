using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Session
{
    public class EndMenuViewer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _restart;
        [SerializeField] private Button _menu;
        [SerializeField] private TextMeshProUGUI _waves;
        [SerializeField] private TextMeshProUGUI _enemiesKilled;
        [SerializeField] private TextMeshProUGUI _moneyPerSession;

        private string _wavesText;
        private string _enemiesKilledText;
        private string _moneyPerSessionText;

        public event Action ButtonRestartClicked;
        public event Action ButtonMenuClicked;
        
        private void Awake()
        {
            _wavesText = _waves.text;
            _enemiesKilledText = _enemiesKilled.text;
            _moneyPerSessionText = _moneyPerSession.text;
            _restart.onClick.AddListener(RestartScene);
            _menu.onClick.AddListener(GoToMenu);
            
            Hide();
        }

        private void OnDestroy()
        {
            _restart.onClick.RemoveListener(RestartScene);
            _menu.onClick.RemoveListener(GoToMenu);
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void SetValue(int waves, int enemies, int currency)
        {
            _waves.text = _wavesText + waves;
            _enemiesKilled.text = _enemiesKilledText + enemies;
            _moneyPerSession.text = _moneyPerSessionText + currency;
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void RestartScene() => 
            ButtonRestartClicked?.Invoke();

        private void GoToMenu() => 
            ButtonMenuClicked?.Invoke();
    }
}