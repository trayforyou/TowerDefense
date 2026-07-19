using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class GameStarter : MonoBehaviour
    {
        public const string META_CURRENCY = "MetaCurrency";

        [SerializeField] private MainMenu _startMenu;
        [SerializeField] private string _gameSceneName = "Game";

        private void Awake() =>
            _startMenu.TriedLoadGame += LoadGame;

        private void OnDestroy() =>
            _startMenu.TriedLoadGame -= LoadGame;

        private void LoadGame() =>
            SceneManager.LoadScene(_gameSceneName);
    }
}