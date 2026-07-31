using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "CastleConfig", menuName = "Scriptable Objects/CastleConfig")]
    public class CastleConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxCastleLevel { get; private set; } = 10;
        [field: SerializeField] public int UpgradeCastleCost { get; private set; } = 10;
        [field: SerializeField, Min(1.1f)] public float UpgradeMultiplier { get; private set; } = 1.2f;
        [field: SerializeField, Min(1.1f)] public float CostMultiplier { get; private set; } = 1.2f;
        [field: SerializeField] public float RadiusRangeCastle { get; private set; } = 1;
        [field: SerializeField] public float StartDelayShootCastle { get; private set; } = 1;
        [field: SerializeField] public int StartDamageCastle { get; private set; } = 1;
        [field: SerializeField] public int StartHealthCastle { get; private set; } = 25;
    }
}