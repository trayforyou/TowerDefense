using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemiesConfig", menuName = "Scriptable Objects/EnemiesConfig")]
    public class EnemiesConfig : ScriptableObject
    {
        [field: SerializeField] public float SpawnOffset { get; private set; } = 0.5f;
        [field: SerializeField] public int MinEnemyPoolSize { get; private set; } = 5;
        [field: SerializeField] public int MaxEnemyPoolSize { get; private set; } = 15;
        [field: SerializeField] public int WavesDelay { get; private set; } = 15;
        [field: SerializeField] public float EnemySpawnDelay { get; private set; } = 1f;
        [field: SerializeField] public float EnemySpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float EnemyAttackDelay { get; private set; } = 1f;
        [field: SerializeField] public int EnemyDamage { get; private set; } = 1;
        [field: SerializeField] public int EnemyHealth { get; private set; } = 1;
        [field: SerializeField] public int EnemiesPerWave { get; private set; } = 5;
        [field: SerializeField] public float EnemyStopDistance { get; private set; } = 1f;
        [field: SerializeField, Min(1.1f)] public float MultiplierEnemiesPerWave { get; private set; } = 1.2f;
    }
}