using System.Collections;
using UnityEngine;

public class SessionHandler : MonoBehaviour
{
    [SerializeField] private SessionViewer _sessionViewer;
    [SerializeField] private BuildManager _buildManager;
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private EndMenuViewer _endMenu;
    [SerializeField] private Castle _castle;
    [SerializeField] private Wallet _wallet;

    private bool _isGameOver = false;
    private int _kills = 0;
    private GameConfig _config;
    private Coroutine _coroutine;
    private int _waveNumber = 0;
    private int _moneyPerKill;
    private int _wavesDelay;

    private void Awake()
    {
        _config = GameManager.Instance.Config;
        _moneyPerKill = _config.CostEnemyDeath;
        _wavesDelay = _config.WavesDelay;

        _castle.SetConfig(_config);
        _sessionViewer.Show();
    }

    private void Start()
    {
        SubscribeAll();
        _sessionViewer.ChangeWaveNumber(++_waveNumber);
        _spawner.StartWave();
        _buildManager.SetParameters(_wallet);
    }

    private void OnDestroy() =>
        UnSubscribeAll();

    private void SubscribeAll()
    {
        _spawner.EnemyDied += RegisterKill;
        _spawner.WaveEnded += ReloadWave;
        _spawner.StartedNewWave += SetEnemiesCount;
        _spawner.ChangedAliveEnemies += ChangeEnemiesCount;
        _castle.Died += EndSession;
        _castle.ValueChanged += ChangeHealthValue;
        _wallet.ValueChanged += ChangeMoneyValue;
        _endMenu.ButtonRestartClicked += RestartSession;
        _endMenu.ButtonMenuClicked += GoToMenu;
    }

    private void UnSubscribeAll()
    {
        _endMenu.ButtonMenuClicked -= GoToMenu;
        _endMenu.ButtonRestartClicked -= RestartSession;
        _spawner.EnemyDied -= RegisterKill;
        _spawner.WaveEnded -= ReloadWave;
        _spawner.ChangedAliveEnemies -= ChangeEnemiesCount;
        _spawner.StartedNewWave -= SetEnemiesCount;
        _castle.ValueChanged -= ChangeHealthValue;
        _castle.Died -= EndSession;
        _wallet.ValueChanged -= ChangeMoneyValue;
    }

    private void EndSession()
    {
        if (_isGameOver)
            return;

        _isGameOver = true;
        int reward = _waveNumber * _config.MoneyPerWave + _kills * _config.MoneyPerKill;

        _buildManager.StopAllTowers();
        _sessionViewer.Hide();
        _endMenu.SetValue(_waveNumber, _kills, reward);
        _endMenu.Show();

        int metaMoney = PlayerPrefs.GetInt(GameManager.META_CURRENCY, 0);

        metaMoney += reward;

        PlayerPrefs.SetInt(GameManager.META_CURRENCY, metaMoney);
    }

    private void GoToMenu()
    {
        _endMenu.ButtonMenuClicked -= GoToMenu;
        GameManager.Instance.LoadMainMenu();
    }

    private void RestartSession()
    {
        _endMenu.ButtonRestartClicked -= RestartSession;
        GameManager.Instance.ReloadCurrentScene();
    }

    private void ChangeEnemiesCount(int count) =>
        _sessionViewer.ChangeEnemiesCount(count);

    private void SetEnemiesCount(int count) =>
        _sessionViewer.ChangeEnemiesCount(count, count);

    private void ChangeMoneyValue(int count) =>
        _sessionViewer.ChangeCountMoney(count);

    private void GetCastleInfo()
    {
        if (_castle.TryGetHealthInfo(out int maxHealth, out int currentHealth))
            _sessionViewer.ChangeHealthInfo(currentHealth, maxHealth);
    }

    private void ChangeHealthValue(int point, int maxPoint) =>
        _sessionViewer.ChangeHealthInfo(point, maxPoint);

    private void ReloadWave()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(WaitWave());
    }

    private IEnumerator WaitWave()
    {
        var wait = new WaitForSeconds(1);
        int currentTime = _wavesDelay;

        while (currentTime >= 0)
        {
            _sessionViewer.ChangeWaveTime(currentTime);
            yield return wait;
            currentTime--;
        }

        _sessionViewer.ChangeWaveNumber(++_waveNumber);
        _spawner.StartWave();
    }

    private void RegisterKill()
    {
        _wallet.AddMoney(_moneyPerKill);

        if (_isGameOver)
            return;

        _kills++;
    }
}