using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud
{
    class CosmosDb
    {
        private static string accountURI = "Your URI";
        private static string accountKey = "Your Key";
        public static string DatabaseId { get; private set; } = "ToDoList";
        public static string ToDoItemsId { get; private set; } = "ToDoItems";

        //public static IDocumentDBConnection GetConnection()
        //{
        //    return new DefaultDocumentDBConnection(accountURI, accountKey, DatabaseId);
        //}

        //public static async Task Initialize()
        //{
        //    var connection = GetConnection();

        //    await connection.Client
        //        .CreateDatabaseIfNotExistsAsync(
        //            new Database { Id = DatabaseId });

        //    DocumentCollection myCollection = new DocumentCollection();
        //    myCollection.Id = ToDoItemsId;
        //    myCollection.IndexingPolicy =
        //       new IndexingPolicy(new RangeIndex(DataType.String)
        //       { Precision = -1 });
        //    myCollection.IndexingPolicy.IndexingMode = IndexingMode.Consistent;
        //    var res = await connection.Client.CreateDocumentCollectionIfNotExistsAsync(
        //        UriFactory.CreateDatabaseUri(DatabaseId),
        //        myCollection);
        //    if (res.StatusCode == System.Net.HttpStatusCode.Created)
        //        await InitData(connection);
        //}
    }
}
