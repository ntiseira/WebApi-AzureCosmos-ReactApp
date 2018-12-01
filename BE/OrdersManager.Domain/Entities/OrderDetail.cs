using Newtonsoft.Json;
using OrdersManager.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Domain.Entities
{
   public class OrderDetail: BaseEntity
    {

        //[JsonProperty(PropertyName = "ProductId")]

        public int ProductId { get; set; }


        //[JsonProperty(PropertyName = "OrderId")]
        public int OrderId { get; set; }

        //[JsonProperty(PropertyName = "Quantity")]
        public int Quantity { get; set; }

        //[JsonProperty(PropertyName = "Discount")]
        public int Discount { get; set; }

        //[JsonProperty(PropertyName = "ProductSold")]
        public Product ProductSold { get; set; }
    }
}
