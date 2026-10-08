using System;
using _Project.Scripts.Builds.Castles;
using _Project.Scripts.Builds.Towers;
using _Project.Scripts.Enemies;
using _Project.Scripts.Session;

namespace _Project.Scripts.Analytics
{
    public class GameInfoCollector : IDisposable
    {
        private const string NotEnoughCoins = "not_enough_coins";
        private const string TooCloseToTower = "too_close_to_tower";

        private readonly IGameInfoDispatcher _dispatcher;
        private readonly SpawnerCurator _spawnerCurator;
        private readonly EnemySpawner _enemySpawner;
        private readonly BuildValidator _buildValidator;
        private readonly Wallet _wallet;
        private readonly BuildHandler _buildHandler;
        private readonly Castle _castle;
        private readonly CastleHealth _castleHealth;
        private readonly SessionHandler _sessionHandler;
        private readonly SceneChanger _sceneChanger;
        private int _wavesSurvived;
        private readonly MetaMoneyBank _metaMoneyBank;

        public GameInfoCollector(IGameInfoDispatcher infoDispatcher, SpawnerCurator spawnerCurator,
            EnemySpawner enemySpawner, BuildValidator buildValidator, Wallet wallet, BuildHandler buildHandler,
            Castle castle, SessionHandler sessionHandler, SceneChanger sceneChanger,
            MetaMoneyBank metaMoneyBank)
        {
            _metaMoneyBank = metaMoneyBank;
            _sceneChanger = sceneChanger;
            _sessionHandler = sessionHandler;
            _buildHandler = buildHandler;
            _castle = castle;
            _wallet = wallet;
            _enemySpawner = enemySpawner;
            _spawnerCurator = spawnerCurator;
            _dispatcher = infoDispatcher;
            _buildValidator = buildValidator;

            SubscribeAll();
        }

        public void Dispose()
        {
            _enemySpawner.StartedNewWave -= WaveStart;
            _enemySpawner.WaveEnded -= WaveComplete;
            _buildHandler.TowerBuilt -= TowerBuild;
            _buildHandler.TooCloseRejected -= RejectTooClose;
            _buildHandler.NoMoneyRejected -= RejectNoMoney;
            _castle.Damaged -= DamageCastle;
            _sessionHandler.GameEnded -= EndGame;
            _sceneChanger.SceneRestarted -= RestartSession;
            _sceneChanger.ReturnedToMenu -= ReturnedToMenu;
        }
        
        private void SubscribeAll()
        {
            _enemySpawner.StartedNewWave += WaveStart;
            _enemySpawner.WaveEnded += WaveComplete;
            _buildHandler.TowerBuilt += TowerBuild;
            _buildHandler.TooCloseRejected += RejectTooClose;
            _buildHandler.NoMoneyRejected += RejectNoMoney;
            _castle.Damaged += DamageCastle;
            _sessionHandler.GameEnded += EndGame;
            _sceneChanger.SceneRestarted += RestartSession;
            _sceneChanger.ReturnedToMenu += ReturnedToMenu;
        }

        private void ReturnedToMenu() => 
            _dispatcher.SendInfoReturnedToMenu(_wavesSurvived, _metaMoneyBank.GetBalance());

        private void RestartSession() =>
            _dispatcher.SendInfoWavesSurvived(_wavesSurvived);

        private void EndGame(int metaCurrency)
        {
            _wavesSurvived = _spawnerCurator.WaveNumber;
            _dispatcher.SendInfoGameOver(_spawnerCurator.WaveNumber, _spawnerCurator.EnemiesDeaths,
                _buildValidator.TowersBuilt, metaCurrency);
        }

        private void DamageCastle() =>
            _dispatcher.SendInfoCastleDamaged(_spawnerCurator.WaveNumber, _castle.CurrentHealth);

        private void WaveComplete() =>
            _dispatcher.SendInfoWaveCompleted(_spawnerCurator.WaveNumber, _buildValidator.TowersBuilt, _wallet.Count);

        private void WaveStart(int enemiesCount) =>
            _dispatcher.SendInfoWaveStarted(_spawnerCurator.WaveNumber, enemiesCount);

        private void TowerBuild(int cost) =>
            _dispatcher.SendInfoTowerBuilt(_spawnerCurator.WaveNumber, cost, _wallet.Count,
                _buildValidator.TowersBuilt);

        private void BuildReject(string reason) =>
            _dispatcher.SendInfoBuildRejected(reason, _spawnerCurator.WaveNumber);

        private void RejectNoMoney() =>
            BuildReject(NotEnoughCoins);

        private void RejectTooClose() =>
            BuildReject(TooCloseToTower);
    }
}