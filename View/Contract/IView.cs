using System;
using Windows.UI.Core;

namespace UWPUtilities.View.Contract
{
    public interface IView : IDisposable
    {
        CoreDispatcher Dispatcher { get; }
    }
}
