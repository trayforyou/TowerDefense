using System;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Session
{
    public class SceneChanger : IDisposable
    {
        private readonly string _mainMenuScene;
        private readonly EndMenuViewer _endMenu;

        public event Action SceneRestarted;
        public event Action ReturnedToMenu;
        
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
            SceneRestarted?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void GoToMenu()
        {
            _endMenu.ButtonMenuClicked -= GoToMenu;
            ReturnedToMenu?.Invoke();
            SceneManager.LoadScene(_mainMenuScene);
        }

        public void Dispose()
        {
            _endMenu.ButtonRestartClicked -= ReloadScene;
            _endMenu.ButtonMenuClicked -= GoToMenu;
        }
    }
}