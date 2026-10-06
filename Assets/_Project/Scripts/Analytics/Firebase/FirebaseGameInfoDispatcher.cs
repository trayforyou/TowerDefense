using Firebase.Analytics;

namespace _Project.Scripts.Analytics.Firebase
{
    public class FirebaseGameInfoDispatcher : IGameInfoDispatcher
    {
        public void SendInfoWaveStarted(int waveNumber, int enemiesToSpawn)
        {
            FirebaseAnalytics.LogEvent("wave_started", new Parameter[]
            {
                new Parameter("wave_number", waveNumber),
                new Parameter("enemies_to_spawn", enemiesToSpawn)
            });
        }

        public void SendInfoWaveCompleted(int waveNumber, int towersBuilt, int coinsRemaining)
        {
            FirebaseAnalytics.LogEvent("wave_completed", new Parameter[]
            {
                new Parameter("wave_number", waveNumber),
                new Parameter("towers_built", towersBuilt),
                new Parameter("coins_remaining", coinsRemaining)
            });
        }

        public void SendInfoTowerBuilt(int waveNumber, int coinsSpent, int coinsRemaining, int towersTotal)
        {
            FirebaseAnalytics.LogEvent("tower_built", new Parameter[]
            {
                new Parameter("wave_number", waveNumber),
                new Parameter("coins_spent", coinsSpent),
                new Parameter("coins_remaining", coinsRemaining),
                new Parameter("towers_total", towersTotal)
            });
        }

        public void SendInfoBuildRejected(string reason, int waveNumber)
        {
            FirebaseAnalytics.LogEvent("build_rejected", new Parameter[]
            {
                new Parameter("reason", reason),
                new Parameter("wave_number", waveNumber)
            });
        }

        public void SendInfoCastleDamaged(int waveNumber, int castleHpRemaining)
        {
            FirebaseAnalytics.LogEvent("castle_damaged", new Parameter[]
            {
                new Parameter("wave_number", waveNumber),
                new Parameter("castle_hp_remaining", castleHpRemaining)
            });
        }

        public void SendInfoGameOver(int wavesSurvived, int enemiesKilled, int towersBuilt, int metaEarned)
        {
            FirebaseAnalytics.LogEvent("game_over", new Parameter[]
            {
                new Parameter("waves_survived", wavesSurvived),
                new Parameter("enemies_killed", enemiesKilled),
                new Parameter("towers_built", towersBuilt),
                new Parameter("meta_earned", metaEarned)
            });
        }

        public void SendInfoWavesSurvived(int wavesSurvived) => 
            FirebaseAnalytics.LogEvent("session_restarted", new Parameter("waves_survived", wavesSurvived));

        public void SendInfoReturnedToMenu(int wavesSurvived, int metaCurrencyTotal)
        {
            FirebaseAnalytics.LogEvent("returned_to_menu", new Parameter[]
            {
                new Parameter("waves_survived", wavesSurvived),
                new Parameter("meta_currency_total", metaCurrencyTotal)
            });
        }
    }
}