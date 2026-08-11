using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Enemies.SpawnPoints;
using _Project.Scripts.ScriptableObjects;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.Object;

namespace _Project.Scripts.Enemies
{
    public class EnemySpawner : IDisposable
    {
        private Enemy _prefab;

        private Castle _castle;
        private ObjectPool<Enemy> _enemiesPool;
        private readonly HashSet<Enemy> _enemies = new();
        private Coroutine _coroutine;
        private EnemiesConfig _config;
        private int _enemiesCount;
        private int _currentEnemiesCount;
        private PointGenerator _pointGenerator;
        private CancellationTokenSource _cancellationTokenSource;

        public event Action EnemyDied;
        public event Action WaveEnded;
        public event Action<int> ChangedAliveEnemies;
        public event Action<int> StartedNewWave;

        public EnemySpawner(Castle castle, EnemiesConfig config, Enemy prefab)
        {
            _prefab = prefab;
            _config = config;
            _castle = castle;
            _enemiesCount = _config.EnemiesPerWave;
            _pointGenerator = new PointGenerator(_config.SpawnOffset, Camera.main, 0);
            InitializePool();
        }

        public void StartWave()
        {
            TokenCleaner.Clear(ref _cancellationTokenSource);
            _cancellationTokenSource = new CancellationTokenSource();
            StartSpawning(_cancellationTokenSource.Token).Forget();
        }
        
            
        public void Stop()
        {
            TokenCleaner.Clear(ref _cancellationTokenSource);
            
            foreach (Enemy enemy in _enemies)
                enemy.Stop();
        }

        private async UniTaskVoid StartSpawning(CancellationToken token)
        {
            try
            {
                Enemy tempEnemy;

                int wait = (int)(_config.EnemySpawnDelay * 1000);
                _currentEnemiesCount = _enemiesCount;
                StartedNewWave?.Invoke(_currentEnemiesCount);

                for (int i = 0; i < _enemiesCount; i++)
                {
                    await UniTask.Delay(wait, cancellationToken: token);

                    tempEnemy = _enemiesPool.Get();
                    tempEnemy.transform.position = _pointGenerator.GetRandom();

                    tempEnemy.GoToTarget();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private void InitializePool()
        {
            if (_enemiesPool != null)
                return;

            _enemiesPool = new ObjectPool<Enemy>(
                createFunc: CreateEnemy,
                actionOnGet: GetEnemy,
                actionOnRelease: EnemyRelease,
                actionOnDestroy: DestroyEnemy,
                collectionCheck: false,
                defaultCapacity: _config.MinEnemyPoolSize,
                maxSize: _config.MaxEnemyPoolSize);
        }

        private void EnemyRelease(Enemy enemy)
        {
            EnemyDied?.Invoke();
            ChangedAliveEnemies?.Invoke(--_currentEnemiesCount);
            enemy.gameObject.SetActive(false);

            if (_currentEnemiesCount == 0)
            {
                _enemiesCount = (int)(_enemiesCount * _config.MultiplierEnemiesPerWave);
                WaveEnded?.Invoke();
            }
        }

        private void DestroyEnemy(Enemy enemy)
        {
            enemy.Died -= _enemiesPool.Release;
            _enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        private Enemy CreateEnemy()
        {
            var enemy = Instantiate(_prefab);
            _enemies.Add(enemy);
            enemy.SetParams(_castle, _config);
            enemy.Died += _enemiesPool.Release;

            return enemy;
        }

        private void GetEnemy(Enemy enemy)
        {
            enemy.ResetHealth();
            enemy.gameObject.SetActive(true);
        }

        public void Dispose() =>
            Stop();
    }
}