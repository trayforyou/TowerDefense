using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpawnerCurator))]
public class SessionHandler : MonoBehaviour
{
    
    [SerializeField] private SessionViewer _sessionViewer;
    [SerializeField] private InteractHandler _interactHandler;
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] private EndMenuViewer _endMenu;
    [SerializeField] private Castle _castle;
    [SerializeField] private Wallet _wallet;
    [SerializeField] private GameConfig _config;

    private SpawnerCurator _spawnerCurator; // TO DO разбить хэндлер
    private bool _isGameOver = false;
    private int _kills = 0;
    private Coroutine _coroutine;
    private int _waveNumber = 0;

    private void Start()
    {
        SubscribeAll();

        _interactHandler.SetParameters(_config, _wallet, _castle);
        _spawnerCurator.Initialize(_castle, _config, _wallet);
        _sessionViewer.Show();
        _sessionViewer.ChangeWaveNumber(++_waveNumber);
        _castle.SetConfig(_config);
        _spawnerCurator.StartWave();
    }

    private void OnDestroy() =>
        UnSubscribeAll();

    private void SubscribeAll()
    {
        _spawnerCurator.WaveChanged += _sessionViewer.ChangeWaveNumber;
        _spawnerCurator.TimeChanged += _sessionViewer.ChangeWaveTime;
        _spawnerCurator.ChangedEnemiesCount += _sessionViewer.ChangeEnemiesCount;
        _spawnerCurator.InitializedEnemiesCount += InitializeEnemiesCount;
        _castle.Died += EndSession;
        _castle.ValueChanged += ChangeHealthValue;
        _wallet.ValueChanged += ChangeMoneyValue;
        _endMenu.ButtonRestartClicked += RestartSession;
        _endMenu.ButtonMenuClicked += GoToMenu;
    }

    private void InitializeEnemiesCount(int count) => 
        _sessionViewer.ChangeEnemiesCount(count, count);

    private void UnSubscribeAll()
    {
        _endMenu.ButtonMenuClicked -= GoToMenu;
        _endMenu.ButtonRestartClicked -= RestartSession;

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

        _sessionViewer.Hide();
        _interactHandler.StopAllTowers();
        _spawnerCurator.Stop();
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


    private void ChangeMoneyValue(int count) =>
        _sessionViewer.ChangeCountMoney(count);

    private void GetCastleInfo()
    {
        if (_castle.TryGetHealthInfo(out int maxHealth, out int currentHealth))
            _sessionViewer.ChangeHealthInfo(currentHealth, maxHealth);
    }

    private void ChangeHealthValue(int point, int maxPoint) =>
        _sessionViewer.ChangeHealthInfo(point, maxPoint);
}