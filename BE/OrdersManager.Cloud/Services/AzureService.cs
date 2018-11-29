using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using OrdersManager.Cloud.Interfaces;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud.Services
{
    public class AzureService : ICloudServices       
    {
        private IDocumentDBConnection connAzureCosmos;

        public AzureService(IDocumentDBConnection connAzureCosmos)
        {
            this.connAzureCosmos = connAzureCosmos;
        }

        public async Task  CreateOrderQueryAzureCosmos (object entity)
        {
            var client = this.connAzureCosmos.GetConnection();
            
            /*PropertyInfo[] props = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)*/;

           
            Domain.Entities.Order test = new Domain.Entities.Order();
            test.Id = 1;

            await client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri("azuredbtestorders","test"), "prueba");


            var collectionLink = UriFactory.CreateDocumentUri("azuredbtestorders", "test", "test");

            await client.CreateDocumentAsync(collectionLink, "prueba");

            //await client.CreateDocumentAsync(
            //     UriFactory
            //         .CreateDocumentCollectionUri(
            //             "", ToDoItemsId),
            //     item);


            SqlQuerySpec query = new SqlQuerySpec("SELECT * FROM Families f WHERE f.id = @familyId");
            query.Parameters = new SqlParameterCollection();
            query.Parameters.Add(new SqlParameter("@familyId", "AndersenFamily"));

         //   client.CreateDocumentQuery(collectionLink, query);
            //client.ReplaceDocumentAsync() method used to replace existing document
        }


    

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~AzureService() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
