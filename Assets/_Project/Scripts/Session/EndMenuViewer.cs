using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TowerDefense.Session
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

        public bool IsActive { get; private set; }

        private void Awake()
        {
            _wavesText = _waves.text;
            _enemiesKilledText = _enemiesKilled.text;
            _moneyPerSessionText = _moneyPerSession.text;

            Hide();
        }

        private void OnEnable()
        {
            _restart.onClick.AddListener(RestartScene);
            _menu.onClick.AddListener(GoToMenu);
        }

        private void OnDisable()
        {
            _restart.onClick.RemoveListener(RestartScene);
            _menu.onClick.RemoveListener(GoToMenu);
        }

        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            IsActive = true;
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
            IsActive = false;
        }

        private void RestartScene()
        {
            _restart.onClick.RemoveListener(RestartScene);
            ButtonRestartClicked?.Invoke();
        }

        private void GoToMenu()
        {
            _menu.onClick.RemoveListener(GoToMenu);
            ButtonMenuClicked?.Invoke();
        }
    }
}