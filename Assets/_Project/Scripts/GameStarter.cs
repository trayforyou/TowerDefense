using System;
using System.Threading;
using _Project.Scripts.Analytics;
using _Project.Scripts.Analytics.Firebase;
using _Project.Scripts.Savers;
using Cysharp.Threading.Tasks;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private MainMenu _startMenu;
        [SerializeField] private string _gameSceneName = "Game";
        [SerializeField] private LoadingScreen _loadingScreen;

        private FirebaseCreator _firebaseCreator;
        private FirebaseApp _firebaseApp;
        private MenuInfoCollector _menuInfoCollector;
        private Saver _saver;
        private int _metaCurrency;

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
                SaveData data = _saver.Load();
                _metaCurrency = data.MetaCurrency;
                _startMenu.Initialize(_metaCurrency);
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