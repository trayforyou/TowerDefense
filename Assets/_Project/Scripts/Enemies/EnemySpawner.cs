using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Enemies.SpawnPoints;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _enemyPrefab;

        private Castle _castle;
        private ObjectPool<Enemy> _enemiesPool;
        private readonly HashSet<Enemy> _enemies = new();
        private Coroutine _coroutine;
        private EnemiesConfig _config;
        private int _enemiesCount;
        private int _currentEnemiesCount;
        private PointGenerator _pointGenerator;

        public event Action EnemyDied;
        public event Action WaveEnded;
        public event Action<int> ChangedAliveEnemies;
        public event Action<int> StartedNewWave;

        public void Initialize(Castle castle, EnemiesConfig config)
        {
            _config = config;
            _castle = castle;
            _enemiesCount = _config.EnemiesPerWave;
            _pointGenerator = new PointGenerator(_config.SpawnOffset, Camera.main, 0);
        }

        public void StartWave()
        {
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
                tempEnemy.transform.position = _pointGenerator.GetRandom();

                tempEnemy.GoToTarget();
            }

            while (_currentEnemiesCount > 0)
                yield return null;

            _enemiesCount = (int)(_enemiesCount * _config.MultiplierEnemiesPerWave);

            WaveEnded?.Invoke();
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
        }

        private void DestroyEnemy(Enemy enemy)
        {
            enemy.Died -= _enemiesPool.Release;
            _enemies.Remove(enemy);
            Destroy(enemy.gameObject);
        }

        private Enemy CreateEnemy()
        {
            var enemy = Instantiate(_enemyPrefab);
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
    }
}