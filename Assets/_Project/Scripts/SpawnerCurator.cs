using System;
using System.Collections;
using UnityEngine;

public class SpawnerCurator : MonoBehaviour
{
    [SerializeField] private EnemySpawner _spawner;

    private GameConfig _config;
    private Coroutine _coroutine;
    private Castle _castle;
    private Wallet _wallet;
    private int _waveNumber = 0;
    private int _enemiesDeaths = 0;
    private bool _isInitialized = false;

    public event Action<int> TimeChanged;
    public event Action<int> WaveChanged;
    public event Action<int> ChangedEnemiesCount;
    public event Action<int> InitializedEnemiesCount;
    
    public void Initialize(Castle castle, GameConfig config, Wallet wallet)
    {
        if (_isInitialized)
            throw new Exception("Already Initialized");

        if ( castle == null || config == null || wallet == null)
            throw new ArgumentNullException();

        _isInitialized = true;

        _wallet = wallet;
        _castle = castle;
        _config = config;
        _spawner.Initialize(castle, config);
        SubscribeAll();
    }

    private void OnDestroy() =>
        UnSubscribeAll();

    public void StartWave()
    {
        if (_coroutine == null)
            _spawner.StartWave();
    }

    public void Stop()
    {
        _spawner.Stop();
        StopAllCoroutines();
    }

    private void RegisterKill()
    {
        _wallet.AddMoney(_config.MoneyPerKill);
        _enemiesDeaths++;
    }

    private void ReloadWave()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(WaitWave());
    }

    private IEnumerator WaitWave()
    {
        var wait = new WaitForSeconds(1);
        int currentTime = _config.WavesDelay;

        while (currentTime >= 0)
        {
            TimeChanged?.Invoke(currentTime);
            yield return wait;
            currentTime--;
        }

        WaveChanged?.Invoke(++_waveNumber);
        _spawner.StartWave();
    }

    private void ChangeEnemiesCount(int count) => 
        ChangedEnemiesCount?.Invoke(count);

    private void SetEnemiesCount(int count) => 
        InitializedEnemiesCount?.Invoke(count);

    private void SubscribeAll()
    {
        _spawner.EnemyDied += RegisterKill;
        _spawner.WaveEnded += ReloadWave;
        _spawner.StartedNewWave += SetEnemiesCount;
        _spawner.ChangedAliveEnemies += ChangeEnemiesCount;
    }

    private void UnSubscribeAll()
    {
        _spawner.EnemyDied -= RegisterKill;
        _spawner.WaveEnded -= ReloadWave;
        _spawner.ChangedAliveEnemies -= ChangeEnemiesCount;
        _spawner.StartedNewWave -= SetEnemiesCount;
    }
}