using System;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Builds.Shooters;
using _Project.Scripts.Builds.Towers;
using _Project.Scripts.Enemies;

namespace _Project.Scripts.Session
{
    public class SessionHandler : IDisposable
    {
        private readonly Castle _castle;
        private readonly FiringSwitch _firingSwitch;
        private readonly SpawnerCurator _spawnerCurator;
        private readonly CastleUpper _castleUpper;
        private readonly BuildHandler _buildHandler;
        private readonly SessionViewer _sessionViewer;
        private readonly MetaMoneyBank _bank;
        private readonly EndMenuViewer _endMenu;
        private readonly Wallet _wallet;

        public SessionHandler(Wallet wallet, Castle castle, FiringSwitch firingSwitch, SpawnerCurator spawnerCurator,
            CastleUpper castleUpper, BuildHandler buildHandler, SessionViewer sessionViewer, MetaMoneyBank bank,
            EndMenuViewer endMenu)
        {
            _wallet = wallet;
            _endMenu = endMenu;
            _castle = castle;
            _firingSwitch = firingSwitch;
            _spawnerCurator = spawnerCurator;
            _castleUpper = castleUpper;
            _buildHandler = buildHandler;
            _sessionViewer = sessionViewer;
            _bank = bank;
            
            SubscribeAll();
        }

        public void Start()
        {
            _sessionViewer.Show();
            _spawnerCurator.StartWave();
            _wallet.RefreshInfo();
        }

        public void Dispose() =>
            UnSubscribeAll();

        private void End()
        {
            _castle.Died -= End;
            _firingSwitch.TurnOff();
            _spawnerCurator.Stop();
            _castleUpper.TurnOff();
            _buildHandler.TurnOff();
            _sessionViewer.Hide();

            int reward = _bank.CalculateMoney(_spawnerCurator.WaveNumber, _spawnerCurator.EnemiesDeaths);

            _endMenu.SetValue(_spawnerCurator.WaveNumber, _spawnerCurator.EnemiesDeaths, reward);
            _endMenu.Show();
        }
        
        private void SubscribeAll()
        {
            _castle.Died += End;
            _spawnerCurator.WaveChanged += _sessionViewer.ChangeWaveNumber;
            _spawnerCurator.TimeChanged += _sessionViewer.ChangeWaveTime;
            _spawnerCurator.ChangedEnemiesCount += _sessionViewer.ChangeEnemiesCount;
            _spawnerCurator.InitializedEnemiesCount += _sessionViewer.InitializeEnemiesCount;
            _castle.ValueChanged += _sessionViewer.ChangeHealthInfo;
            _wallet.ValueChanged += _sessionViewer.ChangeCountMoney;
        }

        private void UnSubscribeAll()
        {
            _castle.Died -= End;
            _spawnerCurator.WaveChanged -= _sessionViewer.ChangeWaveNumber;
            _spawnerCurator.TimeChanged -= _sessionViewer.ChangeWaveTime;
            _spawnerCurator.ChangedEnemiesCount -= _sessionViewer.ChangeEnemiesCount;
            _spawnerCurator.InitializedEnemiesCount -= _sessionViewer.InitializeEnemiesCount;
            _castle.ValueChanged -= _sessionViewer.ChangeHealthInfo;
            _wallet.ValueChanged -= _sessionViewer.ChangeCountMoney;
        }
    }
}