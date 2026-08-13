using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BuildConfig", menuName = "Scriptable Objects/BuildConfig")]
    public class BuildConfig : ScriptableObject
    {
        [field: SerializeField] public float MinDistanceForBuilding { get; private set; } = 1f;
        [field: SerializeField] public int FastTowerCost { get; private set; } = 10;
        [field: SerializeField] public int StrongTowerCost { get; private set; } = 10;
    }
}