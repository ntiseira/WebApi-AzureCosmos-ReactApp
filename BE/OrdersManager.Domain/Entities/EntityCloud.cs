using Newtonsoft.Json;
using OrdersManager.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Domain.Entities
{
    public class EntityCloud //: BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public  string Id { get; set; }


        public string OrderCustomer { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}
