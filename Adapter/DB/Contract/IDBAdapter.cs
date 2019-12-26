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
        void Initialize(string name);

        void CreateTable<T>() where T : new();

        int InsertOrReplace<T>(T element) where T : new();

        int InsertOrReplaceAll<T>(IEnumerable<T> elements) where T : new();

        IList<T> Table<T>() where T : new();

        IList<T> Query<T>(string query, params object[] queryParams) where T : new();
    }
}
