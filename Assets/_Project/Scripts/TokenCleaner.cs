using System.Threading;

namespace _Project.Scripts
{
    public static class TokenCleaner
    {
        public static void Clear(ref CancellationTokenSource token)
        {
            token?.Cancel();
            token?.Dispose();
            token = null;
        }
    }
}