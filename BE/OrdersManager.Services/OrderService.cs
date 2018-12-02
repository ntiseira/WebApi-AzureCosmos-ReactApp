using OrdersManager.Cloud.Helper;
using OrdersManager.Cloud.Interfaces;
using OrdersManager.Domain.DTOs;
using OrdersManager.Domain.Entities;
using OrdersManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrdersManager.Services
{
    public class OrderService : IOrderService
    {

        private readonly ICloudServices cloudServices;
        private IDocumentDbRepository<Order> repositoryOrders;

        /// <inheritdoc />
        public OrderService(
              ICloudServices cloudServices
            ,IDocumentDbRepository<Order> repositoryOrders
            )
        {
            this.cloudServices = cloudServices;
            this.repositoryOrders = repositoryOrders;

        }
                

        public async Task EditOrderDetail(OrderDetailDTO orderDetailDto)
        {
            //Get document
            var orderEntity = repositoryOrders.GetItemAsync(orderDetailDto.OrderId.ToString()).Result;

            //Modify properties of order details
           foreach(var item in orderEntity.OrdersDetails)
           {                
                //Ask for id of order detail
                if (item!= null && item.OrderDetailId == orderDetailDto.Id)
                {
                    item.Quantity = orderDetailDto.Quantity;
                    item.Discount = orderDetailDto.Discount;
                }
           }
           
            await repositoryOrders.UpdateItemAsync(orderEntity.Id, orderEntity);
        }


        public Task<PagedListDTO<OrderDTO>> GetOrders(BaseCriteriaDTO criteria)
        {
            //PageSize
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"].ToString());

            //filter (expression in DB)
            Expression<Func<Order, bool>> filterExpression = x => x.Id  != "0";

            //Azure cosmos db not support method toString, and we use the next Workarround
            int filterAmount = 0;
            int.TryParse(criteria.Filter, out filterAmount);
           
            if (!string.IsNullOrWhiteSpace(criteria.Filter))

                filterExpression = filterExpression.Join(
                         x =>
                        (x.OrderCustomer != null && x.OrderCustomer.ContactName.ToUpper().Contains(criteria.Filter.ToUpper())) ||
                         x.ShipAdress.ToUpper().Contains(criteria.Filter.ToUpper())
                        || x.ShipCity.ToUpper().Contains(criteria.Filter.ToUpper())
                        || x.ShipCountry.ToUpper().Contains(criteria.Filter.ToUpper())
                        || x.ShipPostalCode.ToUpper().Contains(criteria.Filter.ToUpper())
                        || x.TotalAmount == filterAmount
                        );

            //order by
            Expression<Func<Order, object>>[] orderByExpressions = this.GetOrderByExpressions_Orders(criteria.OrderBy);


            Tuple<List<Order>, int> q = repositoryOrders.GetAllAsync(criteria.PageNumber,
                pageSize, filterExpression, criteria.OrderAsc, orderByExpressions).Result;

            //get total entities
            int totalItems = q.Item2;


            //parse to DTO
            List<OrderDTO> items = q.Item1.ToList().Select(m => new OrderDTO
            {
                Id = Convert.ToInt32(m.Id),
                Created_At = m.Created_At,
                //OrderCustomer = m.OrderCustomer,
                Details = m.OrdersDetails.Where(x=> x != null).Select(a => new OrderDetailDTO { Id = Convert.ToInt32(a.OrderDetailId), OrderId = Convert.ToInt32(m.Id), Discount = a.Discount, ProductId = a.ProductId, ProductName = a.ProductSold.Name, Quantity = a.Quantity }).ToList(),
                shipAdress = m.ShipAdress,
                shipCity = m.ShipCity,
                shipCountry = m.ShipCountry,
                shipPostalCode = m.ShipPostalCode,
                TotalAmount = m.TotalAmount
            }).ToList();


            var res = new PagedListDTO<OrderDTO>(totalItems, pageSize, items, criteria.PageNumber);
                
            return Task.FromResult(res);
        }





        #region Private Methods

        private Expression<Func<Order, object>>[] GetOrderByExpressions_Orders(string orderBy)
        {
            var result = new List<Expression<Func<Order, object>>>();

            switch (orderBy)
            {
                case nameof(OrderDTO.Id):
                result.Add((Order x) => x.Id); break;
              //  case nameof(OrderDTO.OrderCustomer.ContactName):
              //  result.Add((Order x) => x.OrderCustomer.ContactName); break;
                case nameof(OrderDTO.shipAdress):
                result.Add((Order x) => x.ShipAdress); break;
                case nameof(OrderDTO.shipCity):
                result.Add((Order x) => x.ShipCity); break;
                case nameof(OrderDTO.shipPostalCode):
                result.Add((Order x) => x.ShipPostalCode); break;
                case nameof(OrderDTO.shipCountry):
                result.Add((Order x) => x.ShipCountry); break;
                case nameof(OrderDTO.TotalAmount):
                result.Add((Order x) => x.TotalAmount); break;
                case nameof(OrderDTO.Created_At):
                break;//this is considered by default
            }

            return result.ToArray();
        }


        private Expression<Func<OrderDetail, object>>[] GetOrderDetailsByExpressions_Orders(string orderBy)
        {
            var result = new List<Expression<Func<OrderDetail, object>>>();

            switch (orderBy)
            {
                case nameof(OrderDetailDTO.Id):
                result.Add((OrderDetail x) => x.OrderDetailId); break;
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

      

        #endregion
    }
}
