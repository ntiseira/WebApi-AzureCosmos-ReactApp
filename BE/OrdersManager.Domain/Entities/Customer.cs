using Newtonsoft.Json;
using OrdersManager.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Domain.Entities
{
  public  class Customer : BaseEntity
    {
        [JsonProperty(PropertyName = "ContactName")]
        public string ContactName { get; set; }


    }
}
