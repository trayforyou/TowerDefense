using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public const string META_CURRENCY = "MetaCurrency";

    [SerializeField] private string _mainMenuScene = "Menu";
    [SerializeField] private string _gameScene = "Game";
    [SerializeField] private GameConfig _config;

    public GameConfig Config => _config;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
         
        DontDestroyOnLoad(gameObject);
    }

    public void LoadGame() => 
        SceneManager.LoadScene(_gameScene);

    public void LoadMainMenu() => 
        SceneManager.LoadScene(_mainMenuScene);

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            throw new NullReferenceException(sceneName);

        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene() => 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}