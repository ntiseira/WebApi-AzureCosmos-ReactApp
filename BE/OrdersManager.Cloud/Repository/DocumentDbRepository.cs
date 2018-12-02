using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using OrdersManager.Cloud.Interfaces;
using OrdersManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud
{
    public class DocumentDbRepository <T> : IDocumentDbRepository <T> 
          where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private  static DocumentClient client;

        public static DocumentClient GetClient()
        {
            if (client == null)
                Initialize();

            return client;
        }

        public   async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public   async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(string id,T item)
        { 
            Document docResponse = new Document();
           
            try
            {
                docResponse = await GetClient().ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    docResponse = await GetClient().CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

                return docResponse;
        }


        public IQueryable<T> GetAll( )
        {
           
            var query = GetClient().CreateDocumentQuery<T>(
                  UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), new FeedOptions()
                  {
                      EnableCrossPartitionQuery = true,
                      MaxItemCount = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"].ToString())

                  });


            return query;
        }

    

        public async Task<Tuple<IQueryable<T>,int>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> filter = null, bool orderAsc = false, 
            params Expression<Func<T, object>>[] orderByExpressions)
        {
            IQueryable<T> q = GetAll();
                 
            //Add filters
            if (filter != null)
                q = q.Where(filter);

            //Get count
            int totalItems = q.Count();

            //Add order by desc or asc
            SortOrder sortOrder = SortOrder.Descending;
            if (orderAsc)
                sortOrder = SortOrder.Ascending;

            //Add order by expressions
            foreach (var itemOrder in orderByExpressions)
            {               
                q = ObjectSort(q, itemOrder, sortOrder);
            }


           // q.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            //q = q.Take(pageSize);

            var results = new List<T>();
          
            var query = q.AsDocumentQuery();
            int count = pageNumber - 1;

            while (query.HasMoreResults)
            {
#warning review here , the counter
                if (count == 0)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }
            }

            //Create response
            Tuple<IQueryable<T>, int> res = new Tuple<IQueryable<T>, int>(q, totalItems);

            return res;
        }



        public async Task<List<T>> ExecuteSimpleQuery(string databaseName, string collectionName, Expression<Func<T, bool>> expression)
        {
            // Set some common query options
          FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            List<T> queryResult;
            // Run a simple query via LINQ. DocumentDB indexes all properties, so queries can be completed efficiently and with low latency.

            if (expression != null)
            {
                queryResult = GetClient().CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), queryOptions).Where(expression).ToList();
            }
            else
            {
              

               var query = GetClient().CreateDocumentQuery<T>(
                  UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), new FeedOptions()
                  {
                      EnableCrossPartitionQuery = true
                  }).AsDocumentQuery();


                var results = new List<T>();
                while (query.HasMoreResults)
                {
                     results.AddRange(await query.ExecuteNextAsync<T>());
                }

                queryResult = results;
            }

            return queryResult;

        }



        public async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private  static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }


        #region Private Methods

        private IOrderedQueryable<T> ObjectSort(IQueryable<T> entities, Expression<Func<T, object>> expression,
      SortOrder order = SortOrder.Ascending)
        {
            UnaryExpression unaryExpression = expression.Body as UnaryExpression;
            if (unaryExpression != null)
            {
                var propertyExpression = (MemberExpression)unaryExpression.Operand;
                var parameters = expression.Parameters;

                if (propertyExpression.Type == typeof(DateTime))
                {
                    var newExpression = Expression.Lambda<Func<T, DateTime>>(propertyExpression, parameters);
                    return order == SortOrder.Ascending ? entities.OrderBy(newExpression) : entities.OrderByDescending(newExpression);
                }

                if (propertyExpression.Type == typeof(int))
                {
                    var newExpression = Expression.Lambda<Func<T, int>>(propertyExpression, parameters);
                    return order == SortOrder.Ascending ? entities.OrderBy(newExpression) : entities.OrderByDescending(newExpression);
                }

                if (propertyExpression.Type == typeof(decimal))
                {
                    var newExpression = Expression.Lambda<Func<T, decimal>>(propertyExpression, parameters);
                    return order == SortOrder.Ascending ? entities.OrderBy(newExpression) : entities.OrderByDescending(newExpression);
                }

                throw new NotSupportedException("Object type resolution not implemented for this type");
            }
            return entities.OrderBy(expression);
        }

        #endregion
    }
}
 
