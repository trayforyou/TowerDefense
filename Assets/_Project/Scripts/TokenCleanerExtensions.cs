using System;
using System.Threading;

namespace _Project.Scripts
{
    public static class TokenCleanerExtensions
    {
        public static void Clear(this CancellationTokenSource token)
        {
            if (token == null)
                return;

            try
            {
                token.Cancel();
            }
            catch (ObjectDisposedException)
            {
            }
            finally
            {
                try
                {
                    token.Dispose();
                }
                catch (ObjectDisposedException)
                { 
                }
            }
        }
    }
}