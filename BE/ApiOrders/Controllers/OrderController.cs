using OrdersManager.Common;
using OrdersManager.Domain;
using OrdersManager.Domain.DTOs;
using OrdersManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OrdersManager.Api.Controllers
{    
    [RoutePrefix("api")]
    public class OrderController : ApiController
    {

        private readonly IOrderService orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The device service.</param>
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
               

        /// <summary>
        /// Upload Image
        /// </summary>
        /// <returns></returns>
        [Route("Order/PostOrdersData")]
        public async  Task<HttpResponseMessage> PostFormData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            
            string root = ManageDirectory.GetDirectory();
                            
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                //Get name of file
                string nameOfFile = provider.FileData[0].Headers.ContentDisposition.FileName;

                //Parse if it's JSON
                if (StringHelper.ValidateJSON(nameOfFile))
                { 
                   
                    using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(provider.FileData[0].Headers.ContentDisposition.FileName)))
                    {
                        // Deserialization from JSON  
                        DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(string));
                        nameOfFile = (string)deserializer.ReadObject(ms);                 
                    }
                }

                await orderService.UploadImageContainer(provider.FileData[0].LocalFileName, nameOfFile);
                               
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }




    }
}
