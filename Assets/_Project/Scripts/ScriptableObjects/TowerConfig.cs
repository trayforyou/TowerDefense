using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "TowerConfig", menuName = "Scriptable Objects/TowerConfig")]
    public class TowerConfig : ScriptableObject
    {
        [field: SerializeField] public float ShootDelay{ get; private set; } = 0.8f;
        [field: SerializeField] public float RangeAttack { get; private set; } = 1;
        [field: SerializeField] public int Damage { get; private set; } = 3;
    }
}