using System;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Session
{
    public class SceneChanger : IDisposable
    {
        private readonly string _mainMenuScene;
        private readonly EndMenuViewer _endMenu;

        public SceneChanger(EndMenuViewer endMenu,string sceneName)
        {
            _endMenu = endMenu;
            _mainMenuScene = sceneName;
            
            _endMenu.ButtonRestartClicked += ReloadScene;
            _endMenu.ButtonMenuClicked += GoToMenu;
        }
        
        private void ReloadScene()
        {
            _endMenu.ButtonRestartClicked -= ReloadScene;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void GoToMenu()
        {
            _endMenu.ButtonMenuClicked -= GoToMenu;
            SceneManager.LoadScene(_mainMenuScene);
        }

        public void Dispose()
        {
            _endMenu.ButtonRestartClicked -= ReloadScene;
            _endMenu.ButtonMenuClicked -= GoToMenu;
        }
    }
}