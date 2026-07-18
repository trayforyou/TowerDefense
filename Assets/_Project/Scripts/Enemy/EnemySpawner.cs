using System;
using System.Collections;
using System.Collections.Generic;
using TowerDefense.Builds.Castle;
using TowerDefense.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace TowerDefense.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private int _minPoolSize = 5;
        [SerializeField] private int _maxPoolSize = 20;
        [SerializeField] private List<SpawnPoint> _spawnPoints;
        [SerializeField] private Vector3 _createEnemiesPoint;

        private Castle _castle;
        private ObjectPool<Enemy> _enemiesPool;
        private HashSet<Enemy> _enemies = new();
        private Coroutine _coroutine;
        private GameConfig _config;
        private int _enemiesCount;
        private int _currentEnemiesCount;
        private bool _isInitialized = false;

        public event Action EnemyDied;
        public event Action WaveEnded;
        public event Action<int> ChangedAliveEnemies;
        public event Action<int> StartedNewWave;

        public void Initialize(Castle castle, GameConfig config)
        {
            if (_isInitialized)
                throw new Exception("Already Initialized");

            if (castle == null || config == null)
                throw new ArgumentNullException();

            _isInitialized = true;
            _config = config;
            _castle = castle;
            _enemiesCount = _config.EnemiesPerWave;
        }

        public void StartWave()
        {
            if (!_isInitialized)
                throw new Exception("Not Initialized");

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartSpawning());
        }

        public void Stop()
        {
            StopAllCoroutines();

            foreach (Enemy enemy in _enemies)
                enemy.Stop();
        }

        private IEnumerator StartSpawning()
        {
            InitializePool();

            Enemy tempEnemy;

            var wait = new WaitForSeconds(_config.EnemySpawnDelay);
            _currentEnemiesCount = _enemiesCount;
            StartedNewWave?.Invoke(_currentEnemiesCount);

            for (int i = 0; i < _enemiesCount; i++)
            {
                yield return wait;

                tempEnemy = _enemiesPool.Get();
                tempEnemy.transform.position = GetRandomSpawnPoint();

                tempEnemy.StartAttack();
            }

            while (_enemiesPool.CountActive > 0)
                yield return null;

            _enemiesCount = (int)(_enemiesCount * _config.MultiplierEnemiesPerWave);

            WaveEnded?.Invoke();
        }

        private void InitializePool()
        {
            if (_enemiesPool != null)
                return;

            _enemiesPool = new ObjectPool<Enemy>(
                createFunc: () => CreateEnemy(),
                actionOnGet: enemy => GetEnemy(enemy),
                actionOnRelease: enemy => EnemyRelease(enemy),
                actionOnDestroy: enemy => DestroyEnemy(enemy),
                collectionCheck: false,
                defaultCapacity: _minPoolSize,
                maxSize: _maxPoolSize);
        }

        private void EnemyRelease(Enemy enemy)
        {
            EnemyDied?.Invoke();
            ChangedAliveEnemies?.Invoke(--_currentEnemiesCount);
            enemy.gameObject.SetActive(false);
        }

        private void DestroyEnemy(Enemy enemy)
        {
            enemy.Died -= _enemiesPool.Release;
            _enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        private Enemy CreateEnemy()
        {
            Enemy enemy = Instantiate(_enemyPrefab);
            _enemies.Add(enemy);
            enemy.SetParams(_castle, _config);
            enemy.Died += _enemiesPool.Release;

            return enemy;
        }

        private Enemy GetEnemy(Enemy enemy)
        {
            enemy.ResetHealth();
            enemy.gameObject.SetActive(true);

            return enemy;
        }

        private Vector3 GetRandomSpawnPoint()
        {
            SpawnPoint spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];
            return spawnPoint.transform.position;
        }
    }
}