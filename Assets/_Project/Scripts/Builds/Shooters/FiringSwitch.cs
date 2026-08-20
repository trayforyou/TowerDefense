using System;

namespace _Project.Scripts.Builds.Shooters
{
    public class FiringSwitch
    {
        public event Action OnStopFire;
        
        public void TurnOff() => 
            OnStopFire?.Invoke();
    }
}