using System;
using _Project.Scripts.Enemies;
using _Project.Scripts.ScriptableObjects;

namespace _Project.Scripts.Session
{
    public class MoneyCollector : IDisposable
    {
        private readonly Wallet _wallet;
        private readonly MoneyConfig _config;
        private readonly SpawnerCurator _curator;

        public MoneyCollector(Wallet wallet, MoneyConfig moneyConfig, SpawnerCurator curator)
        {
            _wallet = wallet;
            _config = moneyConfig; 
            _curator = curator;
            
            _curator.RegisteredKill += RegisterKill;
        }
        
        public void Dispose()
        {
            _curator.RegisteredKill -= RegisterKill;
        }
        
        private void RegisterKill() =>
            _wallet.AddMoney(_config.MoneyPerKill);
    }
}