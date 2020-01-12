using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Extension
{
    public static class BoolExtension
    {
        public static int GetInt(this bool? value, int defaultValue = default)
        {
            if (value == default) { return defaultValue; }
            return Convert.ToInt32((bool)value);
        }
    }
}
