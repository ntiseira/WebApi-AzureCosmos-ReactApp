using Microsoft.Azure.Documents.Client;
using OrdersManager.Cloud.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud
{
    public class DocumentDBConnection : IDocumentDBConnection
    {
        private static string accountURI = "---Put here your account uri---";
        private static string accountKey = "---Put here your key-----";
        public static string DatabaseId { get; private set; } = "ToDoList";
        public static string ToDoItemsId { get; private set; } = "ToDoItems";
        

        public DocumentClient GetConnection()
        {
            return   new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);

        }
    }
}
