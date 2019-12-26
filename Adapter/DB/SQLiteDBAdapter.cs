using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.DB.Contract;
using SQLite;
using UWPUtilities.Util;
using System.IO;

namespace UWPUtilities.Adapter.DB
{
    public sealed class SQLiteDBAdapter : IDBAdapter
    {
        private SQLiteConnection DBConnection;

        public bool IsDBInitialized { get { return DBConnection != null; } }

        public void Initialize(string name)
        {
            var localFolder = FileSystemUtil.GetApplicationFolder(ApplicationFolderType.Local);
            var dbPath = Path.Combine(localFolder.Path, name);
            if (!IsDBInitialized || DBConnection.DatabasePath != dbPath)
            {
                DBConnection = new SQLiteConnection(dbPath);
            }
        }

        public void CreateTable<T>() where T : new()
        {
            ThrowIfDBNotInitialized();
            DBConnection.CreateTable<T>();
        }

        public int InsertOrReplace<T>(T element) where T : new()
        {
            ThrowIfDBNotInitialized();
            return DBConnection.InsertOrReplace(element);
        }

        public int InsertOrReplaceAll<T>(IEnumerable<T> elements) where T : new()
        {
            ThrowIfDBNotInitialized();
            int count = 0;
            DBConnection.RunInTransaction(() =>
            {
                foreach (var element in elements)
                {
                    count += DBConnection.InsertOrReplace(element);
                }
            });
            return count;
        }

        public IList<T> Table<T>() where T : new()
        {
            ThrowIfDBNotInitialized();
            return DBConnection.Table<T>().ToList();
        }

        public IList<T> Query<T>(string query, params object[] queryParams) where T : new()
        {
            ThrowIfDBNotInitialized();
            return DBConnection.Query<T>(query, queryParams);
        }

        private void ThrowIfDBNotInitialized()
        {
            if (!IsDBInitialized)
            {
                throw new InvalidOperationException("DB not initialized");
            }
        }
    }
}
