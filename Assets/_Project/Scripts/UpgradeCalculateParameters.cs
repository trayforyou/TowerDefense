namespace _Project.Scripts
{
    public struct UpgradeCalculateParameters
    {
        public readonly float Multiplier;
        public readonly float MaxLimit;

        public UpgradeCalculateParameters(float multiplier, float maxLimit)
        {
            Multiplier = multiplier;
            MaxLimit = maxLimit;
        }
        
        public UpgradeCalculateParameters(float multiplier)
        {
            Multiplier = multiplier;
            MaxLimit = float.MaxValue;
        }
    }
}