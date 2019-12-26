using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Util;
using Windows.ApplicationModel;
using Windows.Storage;

namespace UWPUtilities_Test.Util_Tests
{
    [TestClass]
    public class AppSettingsUtilTest
    {
        public const string BestCriketPlayerKey = "BestCricketPlayer";
        public const string BestCricketPlayerValue = "Sachin Tendulkar";

        public const string ApplicationDataLocalityKey = "ApplicationDataLocality";
        public const ApplicationDataLocality ApplicationDataLocalityValue = ApplicationDataLocality.Temporary;

        [TestMethod]
        public void GetSetLocalSetting_String_ValidInput()
        {
            AppSettingsUtil.SetSetting(BestCriketPlayerKey, BestCricketPlayerValue);
            var retrievedValue = AppSettingsUtil.GetSetting(BestCriketPlayerKey, default(string));
            Assert.AreEqual(retrievedValue, BestCricketPlayerValue);
        }

        [TestMethod]
        public void GetSetLocalSetting_Enum_ValidInput()
        {
            AppSettingsUtil.SetSetting(ApplicationDataLocalityKey, ApplicationDataLocalityValue);
            var retrievedValue = AppSettingsUtil.GetSetting(ApplicationDataLocalityKey, default(ApplicationDataLocality));
            Assert.AreEqual(retrievedValue, ApplicationDataLocalityValue);
        }

        [TestMethod]
        public void GetLocalDataContainer_AppLevel()
        {
            var container = AppSettingsUtil.GetApplicationDataContainer();
            Assert.AreEqual(container.Name, Package.Current.DisplayName);
        }

        [TestMethod]
        public void GetLocalDataContainer_SubLevel()
        {
            var containerName = "Level1";
            _ = AppSettingsUtil.GetApplicationDataContainer(containerName);
            var parentContainer = AppSettingsUtil.GetApplicationDataContainer();
            Assert.IsTrue(parentContainer.Containers.ContainsKey(containerName));
        }

        [TestMethod]
        public void ClearAllLocalSettings()
        {
            AppSettingsUtil.ClearAllSettings();
            var rootContainer = ApplicationData.Current.LocalSettings;
            Assert.IsTrue(rootContainer.Containers.Count == 0);
        }
    }
}
