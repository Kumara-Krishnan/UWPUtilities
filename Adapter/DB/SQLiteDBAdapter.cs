using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPUtilities.Adapter.DB.Contract;
using SQLite;
using UWPUtilities.Util;
using System.IO;
using Windows.Storage;

namespace UWPUtilities.Adapter.DB
{
    public sealed class SQLiteDBAdapter : IDBAdapter
    {
        private SQLiteConnection DBConnection;

        public bool IsDBInitialized { get { return DBConnection != null; } }

        public void Initialize(string dbfileName, string path = default)
        {
            if (path == default)
            {
                path = FileSystemUtil.GetApplicationFolder(ApplicationFolderType.Local).Path;
            }
            var dbPath = Path.Combine(path, dbfileName);
            if (!IsDBInitialized || DBConnection.DatabasePath != dbPath)
            {
                if (IsDBInitialized) { Close(); }
                DBConnection = new SQLiteConnection(dbPath);
            }
        }

        public void CreateTable<T>() where T : new()
        {
            ThrowIfDBNotInitialized();
            DBConnection.CreateTable<T>();
        }

        public void CreateTables(CreateFlags createFlags = CreateFlags.None, params Type[] types)
        {
            ThrowIfDBNotInitialized();
            DBConnection.CreateTables(createFlags, types);
        }

        public void DropTable<T>() where T : new()
        {
            ThrowIfDBNotInitialized();
            DBConnection.DropTable<T>();
        }

        public void DropTables(params Type[] types)
        {
            ThrowIfDBNotInitialized();
            foreach (var type in types)
            {
                var tableMapping = DBConnection.GetMapping(type);
                DBConnection.DropTable(tableMapping);
            }
        }

        public int InsertOrReplace<T>(T element) where T : new()
        {
            ThrowIfDBNotInitialized();
            return DBConnection.InsertOrReplace(element);
        }

        public int InsertOrReplaceAll<T>(IEnumerable<T> elements) where T : new()
        {
            int count = 0;
            RunInTransaction(() =>
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

        public int Execute(string query, params object[] args)
        {
            ThrowIfDBNotInitialized();
            return DBConnection.Execute(query, args);
        }

        public T ExecuteScalar<T>(string query, params object[] args)
        {
            ThrowIfDBNotInitialized();
            return DBConnection.ExecuteScalar<T>(query, args);
        }

        public T FindWithQuery<T>(string query, params object[] args) where T : new()
        {
            ThrowIfDBNotInitialized();
            return DBConnection.FindWithQuery<T>(query, args);
        }

        public int DeleteAll<T>() where T : new()
        {
            ThrowIfDBNotInitialized();
            return DBConnection.DeleteAll<T>();
        }

        public void RunInTransaction(Action action, bool reThrow = false)
        {
            ThrowIfDBNotInitialized();
            try
            {
                DBConnection.RunInTransaction(action);
            }
            catch (Exception)
            {
                if (reThrow) { throw; }
            }
        }

        public void RunInTransaction(Func<Task> func, bool reThrow = false)
        {
            ThrowIfDBNotInitialized();
            try
            {
                var savePoint = DBConnection.SaveTransactionPoint();
                func();
                DBConnection.Release(savePoint);
            }
            catch (Exception)
            {
                DBConnection.Rollback();
                if (reThrow) { throw; }
            }
        }

        public void Close()
        {
            ThrowIfDBNotInitialized();
            DBConnection.Close();
            DBConnection = null;
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
