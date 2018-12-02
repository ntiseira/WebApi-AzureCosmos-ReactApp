using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud.Interfaces
{
    public interface IDocumentDbRepository <T> where T : class
    {
        Task<T> GetItemAsync(string id);

         Task<Document>  CreateItemAsync(string id, T item);

        Task<Document> UpdateItemAsync(string id, T item);

         Task<List<T>> ExecuteSimpleQuery(string databaseName, string collectionName, Expression<Func<T, bool>> expression);

         Task<Tuple<IQueryable<T>, int>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> filter = null, bool orderAsc = false,
            params Expression<Func<T, object>>[] orderByExpressions);

    }
}
