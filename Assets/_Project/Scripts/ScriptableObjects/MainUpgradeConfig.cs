using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
    public class MainUpgradeConfig : ScriptableObject
    {
        [field: SerializeField] public int CastleHealthUpCost { get; private set; } = 20;
        [field: SerializeField] public int CastleSpeedUpCost { get; private set; } = 15;
        [field: SerializeField] public int CastleDamageUpCost { get; private set; } = 5;
        [field: SerializeField] public int TowerSpeedUpCost { get; private set; } = 7;
        [field: SerializeField] public int TowerDamageUpCost { get; private set; } = 5000;
        [field: SerializeField] public float CastleHealthMultiplier { get; private set; } = 1.2f;
        [field: SerializeField] public float CastleSpeedMultiplier { get; private set; } = 0.92f;
        [field: SerializeField] public float CastleDamageMultiplier { get; private set; } = 1.15f;
        [field: SerializeField] public float TowerSpeedMultiplier { get; private set; } = 0.92f;
        [field: SerializeField] public float TowerDamageMultiplier { get; private set; } = 1.15f;
        [field: SerializeField] public float MaxTowerSpeed { get; private set; } = 0.3f;
        [field: SerializeField] public float MaxCastleSpeed { get; private set; } = 0.3f;
    }
}