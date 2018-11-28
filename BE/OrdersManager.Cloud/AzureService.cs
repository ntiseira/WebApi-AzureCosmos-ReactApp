using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using OrdersManager.Cloud.Interfaces;
using Sandboxable.Microsoft.WindowsAzure.Storage;
using Sandboxable.Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud
{
    public class AzureService : ICloudServices
    {

        public void CreateOrderQueryAzureCosmos()
        {

            var client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);

            var collectionLink = UriFactory.CreateDocumentCollectionUri("databaseId", "collectionId");


            SqlQuerySpec query = new SqlQuerySpec("SELECT * FROM Families f WHERE f.id = @familyId");
            query.Parameters = new SqlParameterCollection();
            query.Parameters.Add(new SqlParameter("@familyId", "AndersenFamily"));

            client.CreateDocumentQuery(collectionLink, query);
            //client.ReplaceDocumentAsync() method used to replace existing document
        }


        public async Task<bool> UploadFileAsync(string filePath, string fileName)
        {
            
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;
          
            // Retrieve the connection string for use with the application. The storage connection string is stored in appsettings         
            string storageConnectionString = ConfigurationManager.AppSettings["CloudBlobConnectionString"].ToString();
            
            //ContainterName
            string containerName = ConfigurationManager.AppSettings["ContainterName"].ToString();

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);


                    if (cloudBlobContainer.CreateIfNotExistsAsync().Result)
                    {

                        // Set the permissions so the blobs are public. 
                        await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                    }

                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                                       
                    await cloudBlockBlob.UploadFromFileAsync(filePath);
                    
                    return true;

                }
                catch (StorageException ex)
                {
                    Console.WriteLine("Error returned from the service: {0}", ex.Message);

                    return false;

                }               
            }
            else
            {
                throw new Exception("Error with connection string not found");
            }
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
