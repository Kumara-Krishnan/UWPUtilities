using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Adapter.DB.Contract
{
    public interface IDBAdapter
    {
        void Initialize(string dbfileName, string path = default);

        void CreateTable<T>() where T : new();

        void CreateTables(CreateFlags createFlags = CreateFlags.None, params Type[] types);

        void DropTable<T>() where T : new();

        void DropTables(params Type[] types);

        int InsertOrReplace<T>(T element) where T : new();

        int InsertOrReplace(object element, Type objType);

        int InsertOrReplaceAll<T>(IEnumerable<T> elements) where T : new();

        int InsertOrReplaceAll(IEnumerable<object> elements, Type objType);

        IList<T> Table<T>() where T : new();

        IList<T> Query<T>(string query, params object[] queryParams) where T : new();

        int Execute(string query, params object[] args);

        T ExecuteScalar<T>(string query, params object[] args);

        T Find<T>(object pk) where T : new();

        T FindWithQuery<T>(string query, params object[] args) where T : new();

        int DeleteAll<T>() where T : new();

        void RunInTransaction(Action action, bool reThrow = false);

        void RunInTransaction(Func<Task> func, bool reThrow = false);

        void Close();
    }
}
