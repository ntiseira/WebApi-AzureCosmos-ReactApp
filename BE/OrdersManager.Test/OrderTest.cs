using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Results;
using NUnit.Framework;
using OrdersManager.Api.Controllers;
using OrdersManager.Cloud;
using OrdersManager.Cloud.Interfaces;
using OrdersManager.Domain.DTOs;
using OrdersManager.Domain.Entities;
using OrdersManager.Services;

namespace OrdersManager.Test
{
    [TestFixture]
    public class OrderTest
    {
        [Test]
        public void GetOrdersWithoutFilter()
        {
            var repo = UnityConfig.Resolve<IDocumentDbRepository<Order>>();

            var orderController = UnityConfig.Resolve<OrderController>();

            BaseCriteriaDTO criteria = new BaseCriteriaDTO
            {
                Filter = "",
                OrderAsc = true,
                OrderBy = "",
                PageNumber = 1
            };

            var postResult = orderController.PostGetOrders(criteria).Result;

            var listOrders =  postResult as OkNegotiatedContentResult<PagedListDTO<OrderDTO>>;


            Assert.IsTrue(listOrders.Content.TotalItems > 0);
        }

        [Test]
        [Ignore ("This method is used to generate mock data")]
        public void DoMockDataInAzureCosmos()
        {
            var repo = UnityConfig.Resolve<IDocumentDbRepository<Order>>();


            for (int i = 1; i < 60; i++)
            {
                Order orderMock = new Order();

                orderMock.Id = i.ToString();
                orderMock.Created_At = DateTime.Now;
                orderMock.CustomerId = 1;
                orderMock.OrderCustomer = new Customer 
                {
                    ContactName = "Pedro"
                };
                orderMock.ShipAdress = "testAddress";
                orderMock.ShipCity = "testShipCity";
                orderMock.ShipCity = "testShipCity";
                orderMock.ShipCountry = "testShipCountry";
                orderMock.ShipPostalCode = "testShipPostalCode";
                orderMock.TotalAmount = 4;
                orderMock.OrdersDetails = new OrderDetail[10];

                for (int j = 0; j < 6; j++)
                {
                    orderMock.OrdersDetails[j] =
                        new OrderDetail
                        {
                            OrderDetailId = j + 1,
                            Discount = 10,
                            OrderId = i,
                            ProductId = 1,
                            ProductSold = new Product
                            {                               
                                Name = "Mesas",
                                UnitInStock = 10,
                                Unitprice = 1000
                            },
                            Quantity = 10

                        };

                }
 
                var resu = repo.CreateItemAsync(orderMock.Id, orderMock).Result;

                Assert.AreEqual(resu.Id, orderMock.Id);
            }


        }


        [Test]
        public void GetOrdersFilterShipCity()
        {

            var orderController = UnityConfig.Resolve<OrderController>();

            BaseCriteriaDTO criteria = new BaseCriteriaDTO
            {
                Filter = "testAddress",
                OrderAsc = true,
                OrderBy = "",
                PageNumber = 1
            };

            var postResult = orderController.PostGetOrders(criteria).Result;

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

            var postResult = orderController.PostGetOrders(criteria).Result;

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

            var postResult = orderController.PostGetOrders(criteria).Result;

            var listOrders = postResult as OkNegotiatedContentResult<PagedListDTO<OrderDTO>>;

            //Modify Entity
            var entity = listOrders.Content.CurrentPageItems[0];
            entity.shipAdress = "sarasa";

            entity.Details[0].Quantity = 10000;
            entity.Details[0].Discount = 10000;

           var result = orderController.PostEditOrder(entity).Result;

            var posRes = result as OkResult;
            Assert.IsNotNull(posRes);
        }








    }
}
