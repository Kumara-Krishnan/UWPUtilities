using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace UWPUtilities.Extension
{
    public static class ListViewExtension
    {
        public static void Dispose(this ListView listView)
        {
            //if (listView.ItemsPanelRoot is ItemsStackPanel itemsStackPanel)
            //{
            //    for (var i = itemsStackPanel.FirstCacheIndex; i <= itemsStackPanel.LastCacheIndex; i++)
            //    {
            //        if (listView.ContainerFromIndex(i) is ListViewItem listViewItem && listViewItem.ContentTemplateRoot is IDisposable disposable)
            //        {
            //            disposable.Dispose();
            //        }
            //    }
            //}
            //else
            //{
            for (var i = 0; i < listView.Items.Count; i++)
            {
                if (listView.ContainerFromIndex(i) is ListViewItem listViewItem && listViewItem.ContentTemplateRoot is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            //}
        }
    }
}
