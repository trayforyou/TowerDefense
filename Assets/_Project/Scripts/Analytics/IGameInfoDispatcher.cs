namespace _Project.Scripts.Analytics
{
    public interface IGameInfoDispatcher
    {
        public void SendInfoWaveStarted(int waveNumber, int enemiesToSpawn);

        public void SendInfoWaveCompleted(int waveNumber, int towersBuilt, int coinsRemaining);
        
        public void SendInfoTowerBuilt(int waveNumber, int coinsSpent, int coinsRemaining, int towersTotal);
        
        public void SendInfoBuildRejected(string reason, int waveNumber);
        
        public void SendInfoCastleDamaged(int waveNumber, int castleHpRemaining);
        
        public void SendInfoGameOver(int wavesSurvived, int enemiesKilled, int towersBuilt, int metaEarned);

        public void SendInfoWavesSurvived(int wavesSurvived);
        
        public void SendInfoReturnedToMenu(int wavesSurvived, int metaCurrencyTotal);
    }
}