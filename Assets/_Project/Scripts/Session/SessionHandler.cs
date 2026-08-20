using _Project.Scripts.Builds;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Builds.Shooters;
using _Project.Scripts.Builds.Towers;
using _Project.Scripts.Enemies;
using _Project.Scripts.Savers;
using _Project.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Session
{
    [RequireComponent(typeof(InputDispatcher))]
    public class SessionHandler : MonoBehaviour
    {
        [SerializeField] private BuildMenu _buildMenuPrefab;
        [SerializeField] private UpgradeMenu _upgradeMenuPrefab;
        [SerializeField] private EndMenuViewer _endMenuPrefab;
        [SerializeField] private SessionViewer _sessionViewerPrefab;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Tower _fastTowerPrefab;
        [SerializeField] private Tower _strongTowerPrefab;
        [SerializeField] private Castle _castlePrefab;

        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _castleLayer;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private string _mainMenuScene = "Menu";

        [SerializeField] private TowerConfig _strongTowerConfig;
        [SerializeField] private TowerConfig _fastTowerConfig;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private ShooterConfig _shooterConfig;
        [SerializeField] private CastleConfig _castleConfig;
        [SerializeField] private MoneyConfig _moneyConfig;
        [SerializeField] private BuildConfig _buildConfig;

        private InputDispatcher _inputDispatcher;
        private InteractHandler _interactHandler;
        private SpawnerCurator _spawnerCurator;
        private Camera _mainCamera;
        private Castle _castle;
        private Saver _saver;
        private Wallet _wallet;
        private UICreator _uICreator;
        private BuildMenu _buildMenu;
        private UpgradeMenu _upgradeMenu;
        private SessionViewer _sessionViewer;
        private EndMenuViewer _endMenu;
        private CastleUpper _castleUpper;
        private BuildHandler _buildHandler;
        private BuildValidator _buildValidator;
        private EnemySpawner _enemiesSpawner;
        private TowerBuilder _strongBuilder;
        private TowerBuilder _fastBuilder;
        private FiringSwitch _firingSwitch;

        private void Awake()
        {
            _inputDispatcher = GetComponent<InputDispatcher>();
            _mainCamera = Camera.main;
            _saver = new Saver();
            _wallet = new Wallet();
            _uICreator = new UICreator(_canvas);

            CastlePlacer castlePlacer = new CastlePlacer();
            _castle = castlePlacer.PlaceAtScreenCenter(_castlePrefab, _mainCamera, _groundLayer);

            _buildMenu = (BuildMenu)_uICreator.Create(_buildMenuPrefab);
            _buildMenu.SetCostTowers(_buildConfig.FastTowerCost, _buildConfig.StrongTowerCost);
            _upgradeMenu = (UpgradeMenu)_uICreator.Create(_upgradeMenuPrefab);
            _sessionViewer = (SessionViewer)_uICreator.Create(_sessionViewerPrefab);
            _endMenu = (EndMenuViewer)_uICreator.Create(_endMenuPrefab);
            _enemiesSpawner = new EnemySpawner(_castle, _enemiesConfig, _enemyPrefab);
            _spawnerCurator = new SpawnerCurator(_enemiesConfig, _enemiesSpawner);
            _buildValidator = new BuildValidator(_castle, _buildConfig.MinDistanceForBuilding);
            _firingSwitch = new FiringSwitch();
            _strongBuilder = new TowerBuilder(_strongTowerPrefab, _strongTowerConfig, _wallet,
                _buildConfig.StrongTowerCost, CreateGun, _firingSwitch );
            _fastBuilder = new TowerBuilder(_fastTowerPrefab, _fastTowerConfig, _wallet, _buildConfig.FastTowerCost, CreateGun,_firingSwitch);
            _buildHandler = new BuildHandler(_wallet, _buildValidator, _buildMenu, _strongBuilder, _fastBuilder);
            _castleUpper = new CastleUpper(_castleConfig, _wallet, _castle, _upgradeMenu);
            _interactHandler = new InteractHandler(_groundLayer, _castleLayer, _castleUpper,
                _buildHandler, _mainCamera, _inputDispatcher);
        }

        private void Start()
        {
            SubscribeAll();

            Shooter tempShooter = new Shooter(_shooterConfig, _castleConfig.StartDamageCastle,
                _castleConfig.StartDelayShootCastle, _bulletPrefab);
            EnemyFinder tempEnemyFinder = new EnemyFinder(_castleConfig.RadiusRangeCastle, _shooterConfig,
                _castle.transform.position);
            Gun tempGun = new Gun(tempShooter, tempEnemyFinder);

            _castle.SetConfig(_castleConfig, tempGun);
            _sessionViewer.Show();
            _spawnerCurator.StartWave();
            _wallet.RefreshInfo();
        }

        private void OnDestroy() =>
            UnSubscribeAll();

        private Gun CreateGun(GunParameters parameters) =>
            new(new Shooter(_shooterConfig, parameters.Damage, parameters.ShootDelay, _bulletPrefab),
                new EnemyFinder(parameters.Radius, _shooterConfig, parameters.CenterFindPosition));

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
            _buildHandler.UnSubscribeAll();
            _interactHandler.UnSubscribe();
            _castleUpper.UnSubscribeAll();
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
            _firingSwitch.TurnOff();
            _spawnerCurator.Stop();
            _castleUpper.TurnOff();
            _buildHandler.TurnOff();
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