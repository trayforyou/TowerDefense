using System;

namespace _Project.Scripts.Analytics
{
    public class MenuInfoCollector : IDisposable
    {
        private readonly GameStarter _gameStarter;
        private readonly IMenuInfoDispatcher _menuInfoDispatcher;

        public MenuInfoCollector(GameStarter gameStarter, IMenuInfoDispatcher menuInfoDispatcher)
        {
            _menuInfoDispatcher = menuInfoDispatcher;
            _gameStarter = gameStarter;
            _gameStarter.OnGameStarted += StartGame;
        }

        public void Dispose() => 
            _gameStarter.OnGameStarted -= StartGame;

        private void StartGame(int metaCurrency)
        {
            _gameStarter.OnGameStarted -= StartGame;
            _menuInfoDispatcher.SendInfoStartGame(metaCurrency);
        }
    }
}