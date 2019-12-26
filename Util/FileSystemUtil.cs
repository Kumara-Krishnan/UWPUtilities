using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace UWPUtilities.Util
{
    public enum ApplicationFolderType
    {
        Local,
        LocalCache,
        Roaming,
        Temporary,
        InstalledLocation,
        PublisherCache,
        SharedLocal
    }

    public static class FileSystemUtil
    {
        public static async Task<StorageFolder> GetStorageFolderAsync(string path, ApplicationFolderType applicationFolderType = ApplicationFolderType.Local)
        {
            StorageFolder localFolder = GetApplicationFolder(applicationFolderType);
            if (string.IsNullOrEmpty(path))
            {
                return localFolder;
            }

            if (!(await localFolder.TryGetItemAsync(path.Replace('/', '\\')) is StorageFolder currFolder))
            {
                if (applicationFolderType == ApplicationFolderType.InstalledLocation)
                {
                    throw new FileNotFoundException("Folder not found", path);
                }
                // One or more folders in the path don't exist, create and return
                string[] subfoldersAry = path.Split(new string[] { "//", "/" }, StringSplitOptions.RemoveEmptyEntries);
                currFolder = localFolder;
                foreach (string subfolder in subfoldersAry)
                {
                    currFolder = await currFolder.CreateFolderAsync(subfolder, CreationCollisionOption.OpenIfExists);
                }
            }
            return currFolder;
        }

        public static StorageFolder GetApplicationFolder(ApplicationFolderType applicationFolderType)
        {
            switch (applicationFolderType)
            {
                case ApplicationFolderType.Local:
                    return ApplicationData.Current.LocalFolder;
                case ApplicationFolderType.LocalCache:
                    return ApplicationData.Current.LocalCacheFolder;
                case ApplicationFolderType.Roaming:
                    return ApplicationData.Current.RoamingFolder;
                case ApplicationFolderType.SharedLocal:
                    return ApplicationData.Current.SharedLocalFolder;
                case ApplicationFolderType.Temporary:
                    return ApplicationData.Current.TemporaryFolder;
                case ApplicationFolderType.InstalledLocation:
                    return Package.Current.InstalledLocation;
                case ApplicationFolderType.PublisherCache:
                    throw new ArgumentException("Cannot get root publisher cache folder");
                default:
                    throw new NotImplementedException();
            }
        }

        public static async Task<bool> CleanUpTemporaryFolderAsync()
        {
            var temporaryFolder = GetApplicationFolder(ApplicationFolderType.Temporary);
            return await DeleteFolderContentsAsync(temporaryFolder).ConfigureAwait(false);
        }

        public static async Task<bool> DeleteFolderContentsAsync(StorageFolder storageFolder, bool reThrow = false,
            StorageDeleteOption deleteOption = StorageDeleteOption.PermanentDelete)
        {
            var storageItems = await storageFolder.GetItemsAsync();
            bool isSucceeded = true;
            foreach (var storageItem in storageItems)
            { 
                try
                {
                    await storageItem.DeleteAsync(deleteOption);
                }
                catch
                {
                    isSucceeded = false;
                    if (reThrow) { throw; }
                }
            }

            try
            {
                storageItems = await storageFolder.GetItemsAsync();
                isSucceeded = storageItems.Count == 0;
            }
            catch { }

            return isSucceeded;
        }

        public static string RemoveInvalidCharactersFromFileName(string fileName, char replacementCharacter = '_')
        {
            ThrowIfFileNameIsNullOrWhitespace(fileName);
            if (replacementCharacter == default || Path.GetInvalidFileNameChars().Contains(replacementCharacter))
            {
                throw new ArgumentException("Invalid replacement character");
            }
            if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                return string.Join(replacementCharacter, fileName.Split(Path.GetInvalidFileNameChars()));
            }
            ThrowIfFileNameIsNullOrWhitespace(fileName);
            return fileName;

            void ThrowIfFileNameIsNullOrWhitespace(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentNullException("Filename is invalid");
                }
            }
        }

    }
}
