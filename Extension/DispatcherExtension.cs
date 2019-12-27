using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace UWPUtilities.Extension
{
    public static class DispatcherExtension
    {
        public static IAsyncAction RunOnUIThread(this CoreDispatcher dispatcher, Action action, CoreDispatcherPriority priority = CoreDispatcherPriority.Normal,
            bool reThrow = false)
        {
            if (dispatcher is null) { return Task.CompletedTask.AsAsyncAction(); }
            if (dispatcher.HasThreadAccess)
            {
                try
                {
                    action();
                }
                catch (Exception)
                {
                    if (reThrow) { throw; }
                }

                return Task.CompletedTask.AsAsyncAction();
            }
            else
            {
                return dispatcher.RunAsync(priority, () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception)
                    {
                        if (reThrow) { throw; }
                    }
                });
            }
        }

        public static async Task TrySwitchToMainViewAsync(this CoreDispatcher dispatcher)
        {
            await dispatcher.RunOnUIThread(async () =>
            {
                if (CoreApplication.Views.Count > 1)
                {
                    var mainViewId = ApplicationView.GetForCurrentView().Id;
                    await CoreApplication.MainView.Dispatcher.RunOnUIThread(() =>
                    {
                        mainViewId = ApplicationView.GetForCurrentView().Id;
                    });
                    await ApplicationViewSwitcher.SwitchAsync(mainViewId);
                }
            });
        }
    }
}
