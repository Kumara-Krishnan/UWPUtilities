using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPUtilities.View.Contract
{
    public interface IRootShell : IView
    {
        Frame Frame { get; }

        void SetTitle(string title);

        void UpdateTheme(ElementTheme theme);
    }
}
