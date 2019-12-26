using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Extension
{
    public static class ListExtension
    {
        public static void InitializeIfNull<T>(this List<T> list)
        {
            if (list is null)
            {
                list = new List<T>();
            }
        }
    }
}
