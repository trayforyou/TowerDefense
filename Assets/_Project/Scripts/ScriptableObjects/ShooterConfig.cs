using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ShooterConfig", menuName = "Scriptable Objects/ShooterConfig")]
    public class ShooterConfig : ScriptableObject
    {
        [field: SerializeField] public float BulletSpeed { get; private set; } = 3f;
        [field: SerializeField] public float FindDelay { get; private set; } = 1.5f;
        [field: SerializeField] public int MinBulletPoolSize { get; private set; } = 5;
        [field: SerializeField] public int MaxBulletPoolSize { get; private set; } = 15;
        [field: SerializeField] public int FindBufferSize { get; private set; } = 15;
    }
}