using UnityEngine.SceneManagement;

namespace _Project.Scripts.Session
{
    public class SceneChanger
    {
        private string _mainMenuScene = "Menu";
        
        public void GoToMainMenu() => 
            SceneManager.LoadScene(_mainMenuScene);

        public void RestartScene() => 
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}