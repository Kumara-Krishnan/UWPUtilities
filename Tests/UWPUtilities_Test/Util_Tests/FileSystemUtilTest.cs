using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Util;
using Windows.Storage;

namespace UWPUtilities_Test.Util_Tests
{
    [TestClass]
    public class FileSystemUtilTest
    {
        [TestMethod]
        public async Task GetLocalFolderAsync()
        {
            var folder = await FileSystemUtil.GetStorageFolderAsync("BookmarkIt");
            Assert.IsNotNull(folder);
        }

        [TestMethod]
        public async Task GetNestedLocalFolderAsync()
        {
            var nestedFolder = await FileSystemUtil.GetStorageFolderAsync("BookmarkIt/Test");
            Assert.IsNotNull(nestedFolder);
        }

        [TestMethod]
        public async Task GetLocalFolderWithMoreThan255CharactersAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                //300 character name.
                var folderName = $"E4Fb3aZq4qIBq9m1L3TSZEqXH6ef9ViyEvWq0mh5Y8fZH18eqV0W7EGBdUC0ss8fjhmRLJRRikHNRsld0JbjUBJWZuexevJ5TP8d" +
                    $"E4Fb3aZq4qIBq9m1L3TSZEqXH6ef9ViyEvWq0mh5Y8fZH18eqV0W7EGBdUC0ss8fjhmRLJRRikHNRsld0JbjUBJWZuexevJ5TP8d" +
                    $"E4Fb3aZq4qIBq9m1L3TSZEqXH6ef9ViyEvWq0mh5Y8fZH18eqV0W7EGBdUC0ss8fjhmRLJRRikHNRsld0JbjUBJWZuexevJ5TP8d";
                var folder = await FileSystemUtil.GetStorageFolderAsync($"BookmarkIt/Test/{folderName}");
            });
        }

        [TestMethod]
        public async Task GetLocalFolderWithInvalidNameAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await FileSystemUtil.GetStorageFolderAsync("Test\"123");
            });
        }

        [TestMethod]
        public async Task GetFolderFromInstalledLocationAsync()
        {
            var folder = await FileSystemUtil.GetStorageFolderAsync("/Assets", ApplicationFolderType.InstalledLocation);
            Assert.IsNotNull(folder);
        }

        [TestMethod]
        public void GetNonSharedApplicationFolders()
        {
            List<string> unavailableFolders = new List<string>();
            foreach (ApplicationFolderType folderType in Enum.GetValues(typeof(ApplicationFolderType)))
            {
                if (folderType == ApplicationFolderType.PublisherCache || folderType == ApplicationFolderType.SharedLocal)
                {
                    continue;
                }
                StorageFolder folder = default;
                try
                {
                    folder = FileSystemUtil.GetApplicationFolder(folderType);
                }
                catch { }
                if (folder is null) { unavailableFolders.Add(folderType.ToString()); }
            }
            Assert.IsTrue(unavailableFolders.Count == 0, string.Join(',', unavailableFolders));
        }

        [TestMethod]
        public async Task CleanUpTemporaryFolderAsync()
        {
            Assert.IsTrue(await FileSystemUtil.CleanUpTemporaryFolderAsync());
        }

        [TestMethod]
        public async Task DeleteFolderContentsAsync()
        {
            var localFolder = FileSystemUtil.GetApplicationFolder(ApplicationFolderType.Local);
            Assert.IsTrue(await FileSystemUtil.DeleteFolderContentsAsync(localFolder));
        }

        [TestMethod]
        public void RemoveInvalidCharactersFromFileName()
        {
            var fileName = "Test\"123";
            fileName = FileSystemUtil.RemoveInvalidCharactersFromFileName(fileName);
            var invalidCharacters = Path.GetInvalidFileNameChars();
            foreach (var character in fileName.ToCharArray())
            {
                if (invalidCharacters.Contains(character))
                {
                    Assert.Fail($"Invalid character found: {character}");
                }
            }
        }
    }
}
