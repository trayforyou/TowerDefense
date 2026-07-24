using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Builds.Shooters
{
    public class GameStarter : MonoBehaviour
    {
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