using System;
using System.Threading;
using UWPUtilities.View.Contract;

namespace UWPUtilities.View.Contract
{
    public abstract class ViewModelBase<T> : ObservableObject, IDisposable where T : IView
    {
        public T View { get; set; }

        public CancellationTokenSource Cts { get; private set; }

        protected ViewModelBase(T view)
        {
            View = view;
            ResetCts();
        }

        protected ViewModelBase() : this(view: default) { }

        public virtual void Dispose()
        {
            View = default;
            Cts?.Cancel();
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
