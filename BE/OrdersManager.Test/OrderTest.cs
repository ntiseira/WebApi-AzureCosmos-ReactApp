using System;
using System.Collections.Generic;
using System.Web.Http.Results;
using NUnit.Framework;
using OrdersManager.Api.Controllers;
using OrdersManager.Data.UnitOfWork;
using OrdersManager.Domain.DTOs;
using OrdersManager.Services;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace OrdersManager.Test
{
    [TestFixture]
    public class OrderTest 
    {
      


        [Test]
        public void OrderPostDataBlob()
        {
            var orderController = UnityConfig.Resolve<OrderController>();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");

            HttpClient client = new HttpClient();

            System.Net.Http.MultipartFormDataContent formDataContent = new System.Net.Http.MultipartFormDataContent();
            formDataContent.Add(new System.Net.Http.StringContent("Hello World!"), name: "greeting");
            System.Net.Http.StreamContent file1 = new System.Net.Http.StreamContent(File.OpenRead(@"C:\Images\riverplate.jpg"));
            file1.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            file1.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data");
            file1.Headers.ContentDisposition.FileName = "riverplate.jpg";
            formDataContent.Add(file1);

            request.Content = formDataContent;
            orderController.Request = request;

            HttpResponseMessage response = orderController.PostFormData().Result;
                       
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        



    }
}
