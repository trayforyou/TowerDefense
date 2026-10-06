using System;
using _Project.Scripts.Analytics;
using _Project.Scripts.Analytics.Firebase;
using _Project.Scripts.Savers;
using Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private MainMenu _startMenu;
        [SerializeField] private string _gameSceneName = "Game";

        private FirebaseCreator _firebaseCreator;
        private FirebaseApp _firebaseApp;
        private MenuInfoCollector _menuInfoCollector;
        private Saver _saver;
        private int _metaCurrency;

        public event Action<int> OnGameStarted;

        private void Awake()
        {
            _firebaseCreator = new FirebaseCreator();
            _saver = new Saver();
            SaveData data = _saver.Load();
            _metaCurrency = data.MetaCurrency;
            _startMenu.Initialize(_metaCurrency);
            FirebaseMenuInfoDispatcher menuInfoDispatcher = new FirebaseMenuInfoDispatcher();
            _menuInfoCollector = new MenuInfoCollector(this, menuInfoDispatcher);
            _startMenu.TriedLoadGame += LoadGame;
        }

        private async void Start()
        {
            try
            {
                await _firebaseCreator.Initialize();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void OnDestroy()
        {
            _startMenu.TriedLoadGame -= LoadGame;
            _menuInfoCollector.Dispose();
        }

        private void LoadGame()
        {
            OnGameStarted?.Invoke(_metaCurrency);
            SceneManager.LoadScene(_gameSceneName);
        }
    }
}