using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Results;
using NUnit.Framework;
using OrdersManager.Api.Controllers;
using OrdersManager.Cloud;
using OrdersManager.Cloud.Interfaces;
using OrdersManager.Cloud.Services;
using OrdersManager.Data.UnitOfWork;
using OrdersManager.Domain.DTOs;
using OrdersManager.Domain.Entities;
using OrdersManager.Services;

namespace OrdersManager.Test
{
    [TestFixture]
    public class OrderTest
    {
        [Test]
        public async Task GetOrdersWithoutFilter()
        {
            var repo = UnityConfig.Resolve<IDocumentDbRepository<EntityCloud>>();

            //var azureClient = new AzureService(conn);


            var orderTest = new EntityCloud { Id = "1"  };

            repo.CreateItemAsync(orderTest).Wait();



           //await azureClient.CreateOrderQueryAzureCosmos(orderTest);

           // var orderController = UnityConfig.Resolve<OrderController>();

           // BaseCriteriaDTO criteria = new BaseCriteriaDTO
           // {
           //     Filter = "",
           //     OrderAsc = true,
           //     OrderBy = "",
           //     PageNumber = 1
           // };

           // var postResult = orderController.PostGetOrders(criteria);

           // var listOrders = postResult as OkNegotiatedContentResult<PagedListDTO<OrderDTO>>;


           // Assert.IsTrue(listOrders.Content.TotalItems > 0);
        }

        private Expression<Func<OrderDetail, object>>[] GetOrderDetailsByExpressions_Orders(string orderBy)
        {
            var result = new List<Expression<Func<OrderDetail, object>>>();

            switch (orderBy)
            {
                case nameof(OrderDetailDTO.Id):
                result.Add((OrderDetail x) => x.Id); break;
                case nameof(OrderDetailDTO.ProductName):
                result.Add((OrderDetail x) => x.ProductSold.Name); break;
                case nameof(OrderDetailDTO.Quantity):
                result.Add((OrderDetail x) => x.Quantity); break;
                case nameof(OrderDetailDTO.Discount):
                result.Add((OrderDetail x) => x.Discount);
                break;//this is considered by default
            }

            result.Add((OrderDetail x) => x.OrderId);

            return result.ToArray();
        }

        [Test]
        public void CreateDocumentAzureCosmos()
        {
            var repo = UnityConfig.Resolve<IDocumentDbRepository<EntityCloud>>();

            //var azureClient = new AzureService(conn);


            var orderTest = new EntityCloud { Id = "8" };


            Expression<Func<EntityCloud, bool>> expression = x => x.Id == orderTest.Id;
            
            


            var result = repo.ExecuteSimpleQuery("azuredbtestorders3", "test3", null).Result;

            List<EntityCloud> orders = new List<EntityCloud>();
            orders = result;

            var resu =  repo.CreateItemAsync(orderTest).Result;

            

            //await azureClient.CreateOrderQueryAzureCosmos(orderTest);

            // var orderController = UnityConfig.Resolve<OrderController>();

            // BaseCriteriaDTO criteria = new BaseCriteriaDTO
            // {
            //     Filter = "",
            //     OrderAsc = true,
            //     OrderBy = "",
            //     PageNumber = 1
            // };

            // var postResult = orderController.PostGetOrders(criteria);

            // var listOrders = postResult as OkNegotiatedContentResult<PagedListDTO<OrderDTO>>;


             Assert.AreEqual(resu.Id , orderTest.Id);
        }


        [Test]
        public void GetOrdersFilterShipCity()
        {

            var orderController = UnityConfig.Resolve<OrderController>();

            BaseCriteriaDTO criteria = new BaseCriteriaDTO
            {
                Filter = "Buenos Aires",
                OrderAsc = true,
                OrderBy = "",
                PageNumber = 1
            };

            var postResult = orderController.PostGetOrders(criteria);

            var listOrders = postResult as OkNegotiatedContentResult<PagedListDTO<OrderDTO>>;


            Assert.IsTrue(listOrders.Content.TotalItems > 0);
        }


        [Test]
        public void GetOrdersOrderByShipAdress()
        {

            var orderController = UnityConfig.Resolve<OrderController>();

            BaseCriteriaDTO criteria = new BaseCriteriaDTO
            {
                Filter = "",
                OrderAsc = true,
                OrderBy = "shipAdress",
                PageNumber = 1
            };

            var postResult = orderController.PostGetOrders(criteria);

            var listOrders = postResult as OkNegotiatedContentResult<PagedListDTO<OrderDTO>>;


            Assert.IsTrue(listOrders.Content.TotalItems > 0);
        }



        [Test]
        public void EditOrder()
        {


            var orderController = UnityConfig.Resolve<OrderController>();

            BaseCriteriaDTO criteria = new BaseCriteriaDTO
            {
                Filter = "",
                OrderAsc = true,
                OrderBy = "",
                PageNumber = 1
            };

            var postResult = orderController.PostGetOrders(criteria);

            var listOrders = postResult as OkNegotiatedContentResult<PagedListDTO<OrderDTO>>;

            //Modify Entity
            var entity = listOrders.Content.CurrentPageItems[0];
            entity.shipAdress = "sarasa";

            entity.Details[0].Quantity = 10000;
            entity.Details[0].Discount = 10000;

            // orderController.PostEditOrder(entity);

            UnityConfig.Resolve<IUnitOfWork>().Commit();

            //  unitOfWorkSession.Commit();
            //Assert.IsTrue(unitOfWorkSession.Commit()));
        }



      




    }
}
