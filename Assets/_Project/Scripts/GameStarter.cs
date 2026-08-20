using _Project.Scripts.Savers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private MainMenu _startMenu;
        [SerializeField] private string _gameSceneName = "Game";

        private Saver _saver;
        
        private void Awake()
        {
            _saver = new Saver();
            SaveData data = _saver.Load();
            _startMenu.Initialize(data.MetaCurrency);
            _startMenu.TriedLoadGame += LoadGame;
        }

        private void OnDestroy() =>
            _startMenu.TriedLoadGame -= LoadGame;

        private void LoadGame() =>
            SceneManager.LoadScene(_gameSceneName);
    }
}