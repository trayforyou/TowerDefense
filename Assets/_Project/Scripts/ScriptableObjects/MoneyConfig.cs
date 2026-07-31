using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "MoneyConfig", menuName = "Scriptable Objects/MoneyConfig")]
    public class MoneyConfig : ScriptableObject
    {
        [field: SerializeField] public int MoneyPerWave { get; private set; } = 50;
        [field: SerializeField] public int MoneyPerKill { get; private set; } = 10;
    }
}