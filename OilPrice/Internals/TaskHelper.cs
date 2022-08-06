using System.Threading;
using System.Threading.Tasks;

namespace OilPricesServer.Internals
{
    internal static class TaskHelper
    {
        internal static Task WaitAsync(this CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> cancelationTaskCompletionSource = new TaskCompletionSource<bool>();
            _ = cancellationToken.Register(CancellationTokenCallback, cancelationTaskCompletionSource);

            return cancellationToken.IsCancellationRequested ? Task.CompletedTask : cancelationTaskCompletionSource.Task;
        }

        private static void CancellationTokenCallback(object taskCompletionSource)
        {
            ((TaskCompletionSource<bool>)taskCompletionSource).SetResult(true);
        }
    }
}
