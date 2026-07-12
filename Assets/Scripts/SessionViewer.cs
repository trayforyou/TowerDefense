using System;
using TMPro;
using UnityEngine;

public class SessionViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _wavesCount;
    [SerializeField] private TextMeshProUGUI _moneysInfo;
    [SerializeField] private TextMeshProUGUI _healthInfo;
    [SerializeField] private TextMeshProUGUI _waveTimeInfo;
    [SerializeField] private TextMeshProUGUI _enemiesKilled;
    [SerializeField] private CanvasGroup _canvasGroup;

    private string _splitter = "|";
    private string _wavesCountText;
    private string _moneysText;
    private string _healthText;
    private string _waveTimeText;
    private string _enemiesKilledText;
    private bool _isWaveTimeInitialize = false;
    private bool _isMoneysInfoInitialize = false;
    private bool _isWavesCountInitialize = false;
    private bool _isHealthInfoInitialize = false;
    private int _maxEnemiesCount;

    public event Action FindingInfo;

    private void Awake()
    {
        _wavesCountText = _wavesCount.text;
        _waveTimeText = _waveTimeInfo.text;
        _moneysText = _moneysInfo.text;
        _healthText = _healthInfo.text;
        _enemiesKilledText = _enemiesKilled.text;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        Show();
    }

    private void FixedUpdate()
    {
        if (_isWaveTimeInitialize || _isMoneysInfoInitialize || _isWavesCountInitialize || _isHealthInfoInitialize)
            FindingInfo?.Invoke();
    }

    public void Show() =>
        _canvasGroup.alpha = 1;

    public void Hide() =>
        _canvasGroup.alpha = 0;

    public void ChangeWaveTime(int time)
    {
        _isWaveTimeInitialize = true;
        _waveTimeInfo.text = _waveTimeText + time;
    }

    public void ChangeEnemiesCount(int maxEnemies, int currentEnemies)
    {
        _maxEnemiesCount = maxEnemies;
        ChangeEnemiesCount(currentEnemies);
    }

    public void ChangeEnemiesCount(int currentEnemies) => 
        _enemiesKilled.text = _enemiesKilledText + currentEnemies + _splitter + _maxEnemiesCount;

    public void ChangeCountMoney(int count)
    {
        _isMoneysInfoInitialize = true;
        _moneysInfo.text = _moneysText + count.ToString();
    }

    public void ChangeWaveNumber(int count)
    {
        _isWavesCountInitialize = true;
        _wavesCount.text = _wavesCountText + count.ToString();
    }

    public void ChangeHealthInfo(int currentCount, int maxCount)
    {
        _isHealthInfoInitialize = true;
        _healthInfo.text = _healthText + currentCount.ToString() + _splitter + maxCount.ToString();
    }
}