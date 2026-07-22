using TowerDefense.Builds;
using TowerDefense.Builds.Castle;
using TowerDefense.Enemy;
using TowerDefense.Saver;
using TowerDefense.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense.Session
{
    [RequireComponent(typeof(SpawnerCurator))]
    [RequireComponent(typeof(Saver.Saver))]
    public class SessionHandler : MonoBehaviour
    {
        [SerializeField] private SessionViewer _sessionViewer;
        [SerializeField] private EndMenuViewer _endMenu;
        [SerializeField] private Castle _castle;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private GameConfig _config;
        [SerializeField] private string _mainMenuScene = "Menu";
        [SerializeField] private InteractHandler _interactHandler;

        private Saver.Saver _saver;
        private SpawnerCurator _spawnerCurator;
        private Coroutine _coroutine;
        private int _waveNumber;

        private void Start()
        {
            _spawnerCurator = GetComponent<SpawnerCurator>();
            _saver = GetComponent<Saver.Saver>();

            SubscribeAll();

            _interactHandler.SetParameters(_config, _wallet, _castle);
            _spawnerCurator.Initialize(_castle, _config, _wallet);
            _sessionViewer.Show();
            _sessionViewer.ChangeWaveNumber(++_waveNumber);
            _castle.SetConfig(_config);
            _spawnerCurator.StartWave();
        }

        private void OnDestroy() =>
            UnSubscribeAll();

        private void SubscribeAll()
        {
            _spawnerCurator.WaveChanged += _sessionViewer.ChangeWaveNumber;
            _spawnerCurator.TimeChanged += _sessionViewer.ChangeWaveTime;
            _spawnerCurator.ChangedEnemiesCount += _sessionViewer.ChangeEnemiesCount;
            _castle.ValueChanged += _sessionViewer.ChangeHealthInfo;
            _wallet.ValueChanged += _sessionViewer.ChangeCountMoneys;
            _spawnerCurator.InitializedEnemiesCount += _sessionViewer.InitializeEnemiesCount;
            _castle.Died += End;
            _endMenu.ButtonRestartClicked += RestartSession;
            _endMenu.ButtonMenuClicked += GoToMenu;
        }

        private void UnSubscribeAll()
        {
            _spawnerCurator.WaveChanged -= _sessionViewer.ChangeWaveNumber;
            _spawnerCurator.TimeChanged -= _sessionViewer.ChangeWaveTime;
            _spawnerCurator.ChangedEnemiesCount -= _sessionViewer.ChangeEnemiesCount;
            _castle.ValueChanged -= _sessionViewer.ChangeHealthInfo;
            _wallet.ValueChanged -= _sessionViewer.ChangeCountMoneys;
            _spawnerCurator.InitializedEnemiesCount -= _sessionViewer.InitializeEnemiesCount;
            _castle.Died -= End;
            _endMenu.ButtonRestartClicked -= RestartSession;
            _endMenu.ButtonMenuClicked -= GoToMenu;
        }

        private void End()
        {
            _castle.Died -= End;
            _spawnerCurator.Stop();
            _interactHandler.Stop();
            _sessionViewer.Hide();

            int reward = _spawnerCurator.WaveNumber * _config.MoneysPerWave +
                         _spawnerCurator.EnemiesDeaths * _config.MoneysPerKill;
            
            AddMetaMoneys(reward);
    
            _endMenu.SetValue(_spawnerCurator.WaveNumber, _spawnerCurator.EnemiesDeaths, reward);
            _endMenu.Show();
        }

        private void AddMetaMoneys(int count)
        {
            SaveData data = _saver.Load();
            int metaMoneys = data.MetaCurrency + count;
            _saver.Save(new SaveData(metaMoneys));
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