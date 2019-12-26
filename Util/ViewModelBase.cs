using System;
using System.Threading;
using UWPUtilities.View.Contract;

namespace UWPUtilities.Util
{
    public abstract class ViewModelBase<T> : ObservableObject, IDisposable where T : IView
    {
        public readonly T View;

        public CancellationTokenSource Cts { get; private set; }

        public ViewModelBase(T view)
        {
            View = view;
            ResetCts();
        }

        public virtual void Dispose()
        {
            View?.Dispose();
            Cts?.Dispose();
        }

        private void ResetCts()
        {
            if (Cts != null)
            {
                Cts.Cancel();
            }
            Cts = new CancellationTokenSource();
        }
    }
}
