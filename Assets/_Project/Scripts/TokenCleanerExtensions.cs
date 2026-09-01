using System.Threading;

namespace _Project.Scripts
{
    public static class TokenCleanerExtensions
    {
        public static void Clear(this CancellationTokenSource token)
        {
            token?.Cancel();
            token?.Dispose();
        }
    }
}