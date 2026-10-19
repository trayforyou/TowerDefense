using System;
using System.Threading;
using _Project.Scripts.Analytics;
using _Project.Scripts.Analytics.Firebase;
using _Project.Scripts.Savers;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.Session;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private CardButton _buttonCastleHealth;
        [SerializeField] private CardButton _buttonCastleSpeed;
        [SerializeField] private CardButton _buttonCastleDamage;
        [SerializeField] private CardButton _buttonTowerSpeed;
        [SerializeField] private CardButton _buttonTowerDamage;
        [SerializeField] private Button _closeButton;

        [SerializeField] private MainMenu _startMenu;
        [SerializeField] private string _gameSceneName = "Game";
        [SerializeField] private LoadingScreen _loadingScreen;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private MainUpgraderMenu _mainUpgraderMenuPrefab;
        [SerializeField] private MoneyConfig _moneyConfig;
        [SerializeField] private MainUpgradeConfig _mainUpgradeConfig;
        [SerializeField] private CastleConfig _castleConfig;
        [SerializeField] private TowerConfig _towerConfig;

        private MetaMoneyBank _metaMoneyBank;
        private MetaLevelController  _metaLevelController;
        private FirebaseCreator _firebaseCreator;
        private FirebaseApp _firebaseApp;
        private MenuInfoCollector _menuInfoCollector;
        private Saver _saver;
        private MainUpgraderMenu _mainUpgraderMenu;
        private int _metaCurrency;
        private UpgradeHandler _upgraderHandler;
        private MainGrowthCalculator _mainGrowthCalculator;

        public event Action<int> OnGameStarted;

        private void Awake()
        {
            _loadingScreen.gameObject.SetActive(true);
            InitializeAll(this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid InitializeAll(CancellationToken token)
        {
            _firebaseCreator = new FirebaseCreator();

            try
            {
                await _firebaseCreator.Initialize().AttachExternalCancellation(token);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            finally
            {
                _saver = new Saver();
                _metaMoneyBank = new MetaMoneyBank(_saver, _moneyConfig);
                _metaLevelController = new MetaLevelController(_saver, _mainUpgradeConfig, _castleConfig, _towerConfig);
                _mainGrowthCalculator = new MainGrowthCalculator(_mainUpgradeConfig);
                UICreator uICreator = new UICreator(_canvas);
                _mainUpgraderMenu = uICreator.Create(_mainUpgraderMenuPrefab);
                _mainUpgraderMenu.Initialize(_metaMoneyBank, _buttonCastleHealth, _buttonCastleSpeed,
                    _buttonCastleDamage, _buttonTowerSpeed, _buttonTowerDamage, _closeButton);
                _upgraderHandler = new UpgradeHandler(_startMenu, _mainUpgraderMenu,_metaLevelController, _mainGrowthCalculator);
                _startMenu.Initialize(_metaMoneyBank);
                FirebaseMenuInfoDispatcher menuInfoDispatcher = new FirebaseMenuInfoDispatcher();
                _menuInfoCollector = new MenuInfoCollector(this, menuInfoDispatcher);
                _startMenu.TriedLoadGame += LoadGame;
                _loadingScreen.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _startMenu.TriedLoadGame -= LoadGame;
            _menuInfoCollector?.Dispose();
        }

        private void LoadGame()
        {
            OnGameStarted?.Invoke(_metaCurrency);
            SceneManager.LoadScene(_gameSceneName);
        }
    }
}