using TMPro;
using UnityEngine;

namespace TowerDefense.Session
{
    public class SessionViewer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _wavesCount;
        [SerializeField] private TextMeshProUGUI _moneysInfo;
        [SerializeField] private TextMeshProUGUI _healthInfo;
        [SerializeField] private TextMeshProUGUI _waveTimeInfo;
        [SerializeField] private TextMeshProUGUI _enemiesAlive;
        [SerializeField] private CanvasGroup _canvasGroup;

        private readonly string _splitter = "|";
        private string _wavesCountText;
        private string _moneysText;
        private string _healthText;
        private string _waveTimeText;
        private string _enemiesAliveText;
        private int _maxEnemiesCount;

        private void Awake()
        {
            _wavesCountText = _wavesCount.text;
            _waveTimeText = _waveTimeInfo.text;
            _moneysText = _moneysInfo.text;
            _healthText = _healthInfo.text;
            _enemiesAliveText = _enemiesAlive.text;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            Show();
        }

        public void Show() =>
            _canvasGroup.alpha = 1;

        public void Hide() =>
            _canvasGroup.alpha = 0;

        public void ChangeWaveTime(int time)
        {
            _waveTimeInfo.text = _waveTimeText + time;
        }

        public void InitializeEnemiesCount(int maxEnemies)
        {
            _maxEnemiesCount = maxEnemies;
            ChangeEnemiesCount(maxEnemies);
        }

        public void ChangeEnemiesCount(int currentEnemies) =>
            _enemiesAlive.text = _enemiesAliveText + currentEnemies + _splitter + _maxEnemiesCount;

        public void ChangeCountMoneys(int count) => 
            _moneysInfo.text = _moneysText + count.ToString();

        public void ChangeWaveNumber(int count) => 
            _wavesCount.text = _wavesCountText + count.ToString();

        public void ChangeHealthInfo(int maxCount, int currentCount) => 
            _healthInfo.text = _healthText + currentCount.ToString() + _splitter + maxCount.ToString();
    }
}