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
        private  readonly string accountURI = "https://azuredbtestorders.documents.azure.com:443/";
        private readonly string accountKey = "QhPU6wTcMpmLkot8TcDP4GQNvfVqOLlilNANf0WJXsyvs9ltLtq0qTJ9rsdPWcHW4CPOXyNOEpeYL9icqHaEog==";
        public static string DatabaseId { get; private set; } = "ToDoList";
        public static string ToDoItemsId { get; private set; } = "ToDoItems";
        

        public DocumentClient GetConnection()
        {
            return   new DocumentClient(new Uri(accountURI), accountKey);

        }
    }
}
