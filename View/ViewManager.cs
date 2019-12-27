using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Extension;
using UWPUtilities.View.Contract;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPUtilities.View
{
    public sealed class ViewManager
    {
        public static ViewManager Instance { get { return ViewManagerSingleton.Instance; } }

        public int ActiveViewId { get; set; } = -1;

        private static readonly ConcurrentDictionary<string, int> ViewWindowMapping = new ConcurrentDictionary<string, int>();

        private static readonly ConcurrentDictionary<int, AppWindow> AppWindows = new ConcurrentDictionary<int, AppWindow>();

        private ViewManager()
        {
            PopulateMainWindow();
        }

        public async Task OpenInNewWindow(Type pageType, object navigationParameter = null, string title = "", Size preferredMinSize = default,
            ViewSizePreference viewSizePreference = ViewSizePreference.Default, params string[] viewReferenceIds)
        {
            var appWindow = await CreateNewView(preferredMinSize);
            AddViewWindowMapping(appWindow.Id, viewReferenceIds);
            AppWindows[appWindow.Id] = appWindow;
            await appWindow.Dispatcher.RunOnUIThread(() =>
            {
                var frame = new Frame();
                frame.Navigate(pageType, navigationParameter);
                Window.Current.Content = frame;
                Window.Current.Activate();
                Window.Current.CoreWindow.Activated += OnWindowActivated;
                appWindow.ApplicationView.Consolidated += OnWindowConsolidated;
            });
            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(appWindow.Id, viewSizePreference);
        }

        private void OnWindowConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs args)
        {
            if (AppWindows.TryGetValue(sender.Id, out AppWindow appWindow))
            {
                _ = appWindow.Dispatcher.RunOnUIThread(() =>
                {
                    RemoveViewWindowMapping(appWindow.Id);
                    appWindow.CoreApplicationView.CoreWindow.Activated -= OnWindowActivated;
                    appWindow.ApplicationView.Consolidated -= OnWindowConsolidated;
                    AppWindows.TryRemove(appWindow.Id, out appWindow);
                    try
                    {
                        if (Window.Current.Content is Frame frame && frame.Content is IDisposable disposable)
                        {
                            disposable.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }, CoreDispatcherPriority.Low);
            }
            Window.Current.Close();
        }

        private void OnWindowActivated(CoreWindow sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState != CoreWindowActivationState.Deactivated)
            {
                ActiveViewId = ApplicationView.GetApplicationViewIdForWindow(sender);
            }
            ActiveViewId = -1;
        }

        private void AddViewWindowMapping(int windowId, params string[] viewReferenceIds)
        {
            if (viewReferenceIds.IsNonEmpty())
            {
                foreach (var viewReferenceId in viewReferenceIds)
                {
                    if (ViewWindowMapping.ContainsKey(viewReferenceId))
                    {
                        throw new InvalidOperationException("A view window mapping already exists with the same View reference id");
                    }
                    ViewWindowMapping[viewReferenceId] = windowId;
                }
            }
        }

        public void RemoveViewWindowMapping(int windowId)
        {
            foreach (var viewWindowMapping in ViewWindowMapping.Where(v => v.Value == windowId).ToList())
            {
                ViewWindowMapping.TryRemove(viewWindowMapping.Key, out windowId);
            }
        }

        private async Task<AppWindow> CreateNewView(Size preferredMinSize = default)
        {
            var coreApplicationView = CoreApplication.CreateNewView();
            ApplicationView applicationView = default;
            await coreApplicationView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                applicationView = ApplicationView.GetForCurrentView();
                if (preferredMinSize != default) { applicationView.SetPreferredMinSize(preferredMinSize); }
            });
            return new AppWindow(coreApplicationView, applicationView);
        }

        private void PopulateMainWindow()
        {
            _ = CoreApplication.MainView.Dispatcher.RunOnUIThread(() =>
            {
                var coreApplicationView = CoreApplication.GetCurrentView();
                var applicationView = ApplicationView.GetForCurrentView();
                var appWindow = new AppWindow(coreApplicationView, applicationView);
                AppWindows[appWindow.Id] = appWindow;
            });
        }

        private sealed class AppWindow
        {
            public readonly int Id;

            public bool IsMainWindow { get { return CoreApplicationView.IsMain; } }

            public CoreDispatcher Dispatcher { get { return CoreApplicationView.Dispatcher; } }

            public readonly CoreApplicationView CoreApplicationView;

            public readonly ApplicationView ApplicationView;

            public AppWindow(CoreApplicationView coreApplicationView, ApplicationView applicationView)
            {
                Id = applicationView.Id;
                CoreApplicationView = coreApplicationView;
                ApplicationView = applicationView;
            }
        }

        private class ViewManagerSingleton
        {
            static ViewManagerSingleton() { }

            internal static readonly ViewManager Instance = new ViewManager();
        }
    }
}
