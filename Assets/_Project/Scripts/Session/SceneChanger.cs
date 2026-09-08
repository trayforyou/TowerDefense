using UnityEngine.SceneManagement;

namespace _Project.Scripts.Session
{
    public class SceneChanger
    {
        private readonly string _mainMenuScene;
        
        public SceneChanger(string sceneName) => 
            _mainMenuScene = sceneName;

        public void GoToMainMenu() => 
            SceneManager.LoadScene(_mainMenuScene);

        public void RestartScene() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}