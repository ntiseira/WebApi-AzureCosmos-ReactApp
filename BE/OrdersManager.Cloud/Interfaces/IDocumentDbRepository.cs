using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud.Interfaces
{
    public interface IDocumentDbRepository <T> where T : class
    {

         Task<Document>  CreateItemAsync(T item);

    }
}
