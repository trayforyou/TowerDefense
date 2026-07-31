using _Project.Scripts.Builds;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Enemies;
using _Project.Scripts.Savers;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Session
{
    [RequireComponent(typeof(SpawnerCurator))]
    public class SessionHandler : MonoBehaviour
    {
        [SerializeField] private SessionViewer _sessionViewer;
        [SerializeField] private EndMenuViewer _endMenu;
        [SerializeField] private Castle _castle;
        [SerializeField] private string _mainMenuScene = "Menu";
        [SerializeField] private InteractHandler _interactHandler;

        [SerializeField] private TowerConfig _strongTowerConfig;
        [SerializeField] private TowerConfig _fastTowerConfig;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private ShooterConfig _shooterConfig;
        [SerializeField] private CastleConfig _castleConfig;
        [SerializeField] private MoneyConfig _moneyConfig;
        [SerializeField] private BuildConfig _buildConfig;

        private Saver _saver;
        private SpawnerCurator _spawnerCurator;
        private Wallet _wallet;

        private void Start()
        {
            _spawnerCurator = GetComponent<SpawnerCurator>();
            _saver = new Saver();
            _wallet = new Wallet();

            SubscribeAll();

            _interactHandler.SetParameters(_castleConfig, _shooterConfig, _buildConfig, _fastTowerConfig,
                _strongTowerConfig, _wallet, _castle);
            _spawnerCurator.Initialize(_castle, _enemiesConfig);
            _castle.SetConfig(_castleConfig, _shooterConfig);
            _sessionViewer.Show();
            _spawnerCurator.StartWave();
            _wallet.RefreshInfo();
        }

        private void OnDestroy() =>
            UnSubscribeAll();

        private void SubscribeAll()
        {
            _spawnerCurator.WaveChanged += _sessionViewer.ChangeWaveNumber;
            _spawnerCurator.TimeChanged += _sessionViewer.ChangeWaveTime;
            _spawnerCurator.ChangedEnemiesCount += _sessionViewer.ChangeEnemiesCount;
            _spawnerCurator.InitializedEnemiesCount += _sessionViewer.InitializeEnemiesCount;
            _spawnerCurator.RegisteredKill += RegisterKill;
            _castle.ValueChanged += _sessionViewer.ChangeHealthInfo;
            _wallet.ValueChanged += _sessionViewer.ChangeCountMoney;
            _castle.Died += End;
            _endMenu.ButtonRestartClicked += RestartSession;
            _endMenu.ButtonMenuClicked += GoToMenu;
        }

        private void UnSubscribeAll()
        {
            _spawnerCurator.WaveChanged -= _sessionViewer.ChangeWaveNumber;
            _spawnerCurator.TimeChanged -= _sessionViewer.ChangeWaveTime;
            _spawnerCurator.ChangedEnemiesCount -= _sessionViewer.ChangeEnemiesCount;
            _spawnerCurator.InitializedEnemiesCount -= _sessionViewer.InitializeEnemiesCount;
            _spawnerCurator.RegisteredKill -= RegisterKill;
            _castle.ValueChanged -= _sessionViewer.ChangeHealthInfo;
            _wallet.ValueChanged -= _sessionViewer.ChangeCountMoney;
            _castle.Died -= End;
            _endMenu.ButtonRestartClicked -= RestartSession;
            _endMenu.ButtonMenuClicked -= GoToMenu;
        }

        private void RegisterKill() =>
            _wallet.AddMoney(_moneyConfig.MoneyPerKill);

        private void End()
        {
            _castle.Died -= End;
            _spawnerCurator.Stop();
            _interactHandler.Stop();
            _sessionViewer.Hide();

            int reward = _spawnerCurator.WaveNumber * _moneyConfig.MoneyPerWave +
                         _spawnerCurator.EnemiesDeaths * _moneyConfig.MoneyPerKill;

            AddMetaMoney(reward);

            _endMenu.SetValue(_spawnerCurator.WaveNumber, _spawnerCurator.EnemiesDeaths, reward);
            _endMenu.Show();
        }

        private void AddMetaMoney(int count)
        {
            SaveData data = _saver.Load();
            int metaMoney = data.MetaCurrency + count;
            _saver.Save(new SaveData(metaMoney));
        }

        private void GoToMenu()
        {
            _endMenu.ButtonMenuClicked -= GoToMenu;
            SceneManager.LoadScene(_mainMenuScene);
        }

        private void RestartSession()
        {
            _endMenu.ButtonRestartClicked -= RestartSession;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}