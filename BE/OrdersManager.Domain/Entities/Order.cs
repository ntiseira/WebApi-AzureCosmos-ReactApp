using Newtonsoft.Json;
using OrdersManager.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Domain.Entities
{
   public class Order : BaseEntity
    {

        //public Order()
        //{
        //    OrdersDetails = new List<OrderDetail>();
        //    OrderCustomer = new Customer
        //}

        //[JsonProperty(PropertyName = "OrderCustomer")]
        public Customer OrderCustomer { get; set; }
        
        //[JsonProperty(PropertyName = "OrdersDetails")]
        public  OrderDetail[] OrdersDetails { get; set; }

        //[JsonProperty(PropertyName = "CustomerId")]
        public int CustomerId { get; set; }

        //[JsonProperty(PropertyName = "Created_At")]
        public DateTime  Created_At { get; set; }

        //[JsonProperty(PropertyName = "ShipAdress")]
        public string ShipAdress { get; set; }

        //[JsonProperty(PropertyName = "ShipCity")]
        public string ShipCity { get; set; }

      //  [JsonProperty(PropertyName = "ShipPostalCode")]
        public string ShipPostalCode { get; set; }

       // [JsonProperty(PropertyName = "ShipCountry")]
        public string ShipCountry { get; set; }

     //   [JsonProperty(PropertyName = "TotalAmount")]
        public decimal TotalAmount { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }


    }
}
