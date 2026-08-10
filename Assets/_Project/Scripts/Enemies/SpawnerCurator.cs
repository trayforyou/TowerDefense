using System;
using System.Threading;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.ScriptableObjects;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Enemies
{
    public class SpawnerCurator : IDisposable
    {
        private EnemySpawner _spawner;
        private EnemiesConfig _config;
        private CancellationTokenSource _waveCts;

        public int WaveNumber { get; private set; }
        public int EnemiesDeaths { get; private set; }

        public event Action RegisteredKill;
        public event Action<int> TimeChanged;
        public event Action<int> WaveChanged;
        public event Action<int> ChangedEnemiesCount;
        public event Action<int> InitializedEnemiesCount;

        public SpawnerCurator(EnemiesConfig config, EnemySpawner spawner)
        {
            _spawner = spawner;
            _config = config;
            SubscribeAll();
        }

        public void Stop()
        {
            _spawner.Stop();
            CancelWaveTimer();
            UnSubscribeAll();
        }
        
        public void Dispose() => 
            Stop();

        public void StartWave()
        {
            WaveChanged?.Invoke(++WaveNumber);
            _spawner.StartWave();
        }

        private void RegisterKill()
        {
            RegisteredKill?.Invoke();
            EnemiesDeaths++;
        }

        private void ReloadWave()
        {
            CancelWaveTimer();
            _waveCts = new CancellationTokenSource();
            WaitWaveAsync(_waveCts.Token).Forget();
        }

        private void CancelWaveTimer()
        {
            _waveCts?.Cancel();
            _waveCts?.Dispose();
            _waveCts = null;
        }

        private async UniTaskVoid WaitWaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                int currentTime = _config.WavesDelay;
                int oneSecond = 1000;

                while (currentTime >= 0 && cancellationToken.IsCancellationRequested == false)
                {
                    TimeChanged?.Invoke(currentTime);
                    await UniTask.Delay(oneSecond, cancellationToken: cancellationToken);
                    currentTime--;
                }

                if (cancellationToken.IsCancellationRequested == false)
                {
                    WaveChanged?.Invoke(++WaveNumber);
                    _spawner.StartWave();
                }
            }
            catch (OperationCanceledException)
            {
            }
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
}