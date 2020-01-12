using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Extension
{
    public static class StringExtension
    {
        public static string ToStringLower<T>(this T value)
        {
            return value.ToString().ToLower();
        }


    }
}
