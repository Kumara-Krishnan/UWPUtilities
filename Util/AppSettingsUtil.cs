using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;

namespace UWPUtilities.Util
{
    public enum SettingType
    {
        Local,
        Roaming
    }

    public static class AppSettingsUtil
    {
        private static ApplicationDataContainer LocalSettingsContainer
        {
            get
            {
                return ApplicationData.Current.LocalSettings.CreateContainer(Package.Current.DisplayName, ApplicationDataCreateDisposition.Always);
            }
        }

        private static ApplicationDataContainer RoamingSettingsContainer
        {
            get
            {
                return ApplicationData.Current.LocalSettings.CreateContainer(Package.Current.DisplayName, ApplicationDataCreateDisposition.Always);
            }
        }

        public static T GetSetting<T>(string key, T defaultValue = default, string containerName = default, SettingType settingType = SettingType.Local)
        {
            if (typeof(T) == typeof(Enum) || typeof(T).BaseType == typeof(Enum))
            {
                var val = GetSetting(key, defaultValue.ToString(), containerName, settingType);
                if (Enum.TryParse(typeof(T), val, out object result) && Enum.IsDefined(typeof(T), result))
                {
                    return (T)result;
                }
                return defaultValue;
            }

            var container = GetApplicationDataContainer(containerName, settingType);
            if (container.Values.TryGetValue(key, out object valueObject) && valueObject is T value)
            {
                return value;
            }
            return defaultValue;
        }

        public static bool SetSetting<T>(string key, T value, string containerName = default, SettingType settingType = SettingType.Local)
        {
            if (typeof(T) == typeof(Enum) || typeof(T).BaseType == typeof(Enum))
            {
                SetSetting(key, value.ToString(), containerName, settingType);
            }
            else
            {
                var container = GetApplicationDataContainer(containerName, settingType);
                container.Values[key] = value;
            }

            var retrievedValue = GetSetting(key, default(T), containerName, settingType);
            return Equals(value, retrievedValue);
        }

        public static bool DeleteSetting<T>(string key, string containerName = default, SettingType settingType = SettingType.Local)
        {
            var container = GetApplicationDataContainer(containerName, settingType);
            return container.Values.Remove(key);
        }

        public static bool DeleteContainer(string containerName, SettingType settingType = SettingType.Local)
        {
            if (string.IsNullOrWhiteSpace(containerName)) { throw new ArgumentNullException("Invalid container name"); }
            (var containerToDelete, var parentPath) = GetLastContainerNameAndParentPath(containerName);
            var parentContainer = GetApplicationDataContainer(parentPath, settingType);
            if (parentContainer.Containers.ContainsKey(containerToDelete))
            {
                parentContainer.DeleteContainer(containerToDelete);
                return true;
            }
            return false;
        }

        public static bool ClearAllSettings(SettingType settingType = SettingType.Local, bool reThrow = false)
        {
            var rootContainer = SettingType.Local == settingType ? ApplicationData.Current.LocalSettings : ApplicationData.Current.RoamingSettings;
            var appContainer = GetApplicationDataContainer(settingType: settingType);
            rootContainer.DeleteContainer(appContainer.Name);
            return rootContainer.Containers.Count == 0;
        }

        public static ApplicationDataContainer GetApplicationDataContainer(string containerName = default, SettingType settingType = SettingType.Local)
        {
            if (SettingType.Local == settingType)
            {
                return GetLocalSettingsContainer(containerName);
            }
            return GetRoamingSettingsContainer(containerName);
        }

        private static ApplicationDataContainer GetLocalSettingsContainer(string containerName)
        {
            return GetSubContainer(containerName, LocalSettingsContainer);
        }

        private static ApplicationDataContainer GetRoamingSettingsContainer(string containerName)
        {
            return GetSubContainer(containerName, RoamingSettingsContainer);
        }

        private static ApplicationDataContainer GetSubContainer(string containerName, ApplicationDataContainer parentContainer)
        {
            if (!string.IsNullOrEmpty(containerName))
            {
                var containerPath = GetContainerPath(containerName);
                if (containerPath is null || containerPath.Length == 0) { throw new ArgumentException("Invalid container name"); }
                foreach (var container in containerPath)
                {
                    parentContainer = parentContainer.CreateContainer(container, ApplicationDataCreateDisposition.Always);
                }
            }
            return parentContainer;
        }

        private static (string containerName, string containerPath) GetLastContainerNameAndParentPath(string containerPath)
        {
            var lastIndex = containerPath.LastIndexOf('/');
            if (lastIndex >= 0 && lastIndex + 1 > containerPath.Length)
            {
                var containerName = containerPath.Substring(lastIndex + 1);
                return (containerName, containerPath.Substring(0, containerName.Length + 1));
            }
            return (containerPath, default);
        }

        private static string[] GetContainerPath(string containerName)
        {
            return containerName.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
