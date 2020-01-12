using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Model;

namespace UWPUtilities.Extension
{
    public static class SettingsBaseExtension
    {
        public static string OptString(this SettingsBase setting, string defaultValue = default)
        {
            return setting?.Value ?? defaultValue;
        }

        public static bool OptBool(this SettingsBase setting, bool defaultValue = default)
        {
            if (bool.TryParse(setting?.Value, out bool result))
            {
                return result;
            }
            return defaultValue;
        }

        public static int OptInt(this SettingsBase setting, int defaultValue = default)
        {
            if (int.TryParse(setting?.Value, out int result))
            {
                return result;
            }
            return defaultValue;
        }

        public static double OptDouble(this SettingsBase setting, double defaultValue = default)
        {
            if (double.TryParse(setting?.Value, out double result))
            {
                return result;
            }
            return defaultValue;
        }

        public static long OptLong(this SettingsBase setting, long defaultValue = default)
        {
            if (long.TryParse(setting?.Value, out long result))
            {
                return result;
            }
            return defaultValue;
        }

        public static T OptEnum<T>(this SettingsBase setting, T defaultValue = default) where T : struct, Enum
        {
            if (Enum.TryParse<T>(setting?.Value, out T result) && Enum.IsDefined(typeof(T), result))
            {
                return result;
            }
            return defaultValue;
        }
    }
}
