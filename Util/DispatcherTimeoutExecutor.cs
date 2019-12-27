using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Xaml;

namespace UWPUtilities.Util
{
    public sealed class DispatcherTimeoutExecutor
    {
        private readonly DispatcherTimer DispatcherTimer = new DispatcherTimer();

        private readonly Action Action;

        private readonly TimeSpan Delay;

        public void Start()
        {
            Stop();
            DispatcherTimer.Interval = Delay;
            DispatcherTimer.Tick += OnTimerTick;
            DispatcherTimer.Start();
        }

        public void Stop()
        {
            DispatcherTimer.Stop();
            DispatcherTimer.Tick -= OnTimerTick;
        }

        private void OnTimerTick(object sender, object e)
        {
            Stop();
            if (Action != null)
            {
                Action?.Invoke();
            }
        }
    }
}
