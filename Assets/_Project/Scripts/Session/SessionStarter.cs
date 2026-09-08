using System;
using System.Collections.Generic;
using _Project.Scripts.Builds;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Builds.Shooters;
using _Project.Scripts.Builds.Towers;
using _Project.Scripts.Enemies;
using _Project.Scripts.Savers;
using _Project.Scripts.ScriptableObjects;
using UnityEngine; 

namespace _Project.Scripts.Session
{
    public class SessionStarter : MonoBehaviour
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

        [SerializeField] private TowerConfig _strongTowerConfig;
        [SerializeField] private TowerConfig _fastTowerConfig;
        [SerializeField] private EnemiesConfig _enemiesConfig;
        [SerializeField] private ShooterConfig _shooterConfig;
        [SerializeField] private CastleConfig _castleConfig;
        [SerializeField] private MoneyConfig _moneyConfig;
        [SerializeField] private BuildConfig _buildConfig;
        [SerializeField] private string _menuSceneName = "Menu";

        private HashSet<IDisposable> _disposables;
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
        private SceneChanger _sceneChanger;
        private MoneyCollector _moneyCollector;
        private SessionHandler _sessionHandler;
        private MetaMoneyBank _bank;

        private void Awake()
        {
            _disposables = new();
            _mainCamera = Camera.main;
            _inputDispatcher = new InputDispatcher();
            _disposables.Add(_inputDispatcher);
            _saver = new Saver();
            _wallet = new Wallet();
            _uICreator = new UICreator(_canvas);
            _bank = new MetaMoneyBank(_saver,_moneyConfig);

            CastlePlacer castlePlacer = new CastlePlacer();
            _castle = castlePlacer.PlaceAtScreenCenter(_castlePrefab, _mainCamera, _groundLayer);

            _buildMenu = _uICreator.Create(_buildMenuPrefab);
            _buildMenu.SetCostTowers(_buildConfig.FastTowerCost, _buildConfig.StrongTowerCost);
            _upgradeMenu = _uICreator.Create(_upgradeMenuPrefab);
            _sessionViewer = _uICreator.Create(_sessionViewerPrefab);
            _endMenu = _uICreator.Create(_endMenuPrefab);
            _sceneChanger = new SceneChanger(_endMenu,_menuSceneName);
            _disposables.Add(_sceneChanger);
            _enemiesSpawner = new EnemySpawner(_castle, _enemiesConfig, _enemyPrefab, _mainCamera);
            _disposables.Add(_enemiesSpawner);
            _spawnerCurator = new SpawnerCurator(_enemiesConfig, _enemiesSpawner);
            _disposables.Add(_spawnerCurator);
            _moneyCollector = new MoneyCollector(_wallet,_moneyConfig,_spawnerCurator);
            _disposables.Add(_moneyCollector);
            _buildValidator = new BuildValidator(_castle, _buildConfig.MinDistanceForBuilding);
            _firingSwitch = new FiringSwitch();
            _strongBuilder = new TowerBuilder(_strongTowerPrefab, _strongTowerConfig, _wallet,
                _buildConfig.StrongTowerCost, CreateGun, _firingSwitch);
            _fastBuilder = new TowerBuilder(_fastTowerPrefab, _fastTowerConfig, _wallet, _buildConfig.FastTowerCost,
                CreateGun, _firingSwitch);
            _buildHandler = new BuildHandler(_wallet, _buildValidator, _buildMenu, _strongBuilder, _fastBuilder);
            _disposables.Add(_buildHandler);
            _castleUpper = new CastleUpper(_castleConfig, _wallet, _castle, _upgradeMenu);
            _disposables.Add(_castleUpper);
            _sessionHandler = new SessionHandler(_wallet,_castle,_firingSwitch,_spawnerCurator,_castleUpper,_buildHandler,_sessionViewer,_bank,_endMenu);
            _disposables.Add(_sessionHandler);
            _interactHandler = new InteractHandler(_groundLayer, _castleLayer, _castleUpper,
                _buildHandler, _mainCamera, _inputDispatcher);
            _disposables.Add(_interactHandler);
        }

        private void Start()
        {
            Gun tempGun = CreateGun(new GunParameters(_castleConfig.StartDamageCastle,
                _castleConfig.StartDelayShootCastle, _castleConfig.RadiusRangeCastle, _castle.transform.position));

            _castle.SetConfig(_castleConfig, tempGun);
            _sessionHandler.Start();
        }

        private void OnDestroy() => 
            DisposeAll();

        private void DisposeAll()
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();
        }

        private Gun CreateGun(GunParameters parameters) =>
            new(new Shooter(_shooterConfig, parameters.Damage, parameters.ShootDelay, _bulletPrefab),
                new EnemyFinder(parameters.Radius, _shooterConfig, parameters.CenterFindPosition));
    }
}