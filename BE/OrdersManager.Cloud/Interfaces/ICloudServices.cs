using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Cloud.Interfaces
{
   public interface ICloudServices : IDisposable
    {
         Task<bool> UploadFileAsync(string filePath, string fileName);

    }
}
