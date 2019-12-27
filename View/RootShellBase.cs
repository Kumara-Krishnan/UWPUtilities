using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.View.Contract;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPUtilities.View
{
    public abstract class RootShellBase : Control, IRootShell
    {
        public const string PART_FRAME = nameof(PART_FRAME);

        public Frame Frame { get; set; }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (Frame is null)
            {
                Frame = GetTemplateChild(PART_FRAME) as Frame;
            }
        }

        public virtual void Dispose() { }

        public virtual void SetTitle(string title)
        {
            ApplicationView.GetForCurrentView().Title = title;
        }

        public virtual void UpdateTheme(ElementTheme theme)
        {
            this.RequestedTheme = theme;
        }
    }
}
