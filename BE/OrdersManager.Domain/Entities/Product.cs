using Newtonsoft.Json;
using OrdersManager.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Domain.Entities
{
    public class Product 
        //: BaseEntity
    {
        //[JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        //[JsonProperty(PropertyName = "Unitprice")]
        public decimal Unitprice { get; set; }

        //[JsonProperty(PropertyName = "UnitInStock")]
        public int UnitInStock { get; set; }

    }
}
