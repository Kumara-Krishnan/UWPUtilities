using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Extension;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;

namespace UWPUtilities.View
{
    public sealed class ThemeManager
    {
        public static ThemeManager Instance { get { return ThemeManagerSingleton.Instance; } }

        private ThemeManager() { }

        public void UpdateTheme(ElementTheme theme)
        {
            foreach (var view in CoreApplication.Views)
            {
                _ = view.Dispatcher.RunOnUIThread(() =>
                {
                    //TODO: Update View's theme.
                });
            }
        }

        internal sealed class ThemeManagerSingleton
        {
            static ThemeManagerSingleton()
            {

            }

            internal static readonly ThemeManager Instance = new ThemeManager();
        }
    }
}
