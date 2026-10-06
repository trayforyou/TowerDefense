using Firebase.Analytics;

namespace _Project.Scripts.Analytics.Firebase
{
    public class FirebaseMenuInfoDispatcher : IMenuInfoDispatcher
    {
        public void SendInfoStartGame(int metaCurrencyTotal) => 
            FirebaseAnalytics.LogEvent("game_started",new Parameter("meta_currency_total", metaCurrencyTotal));
    }
}