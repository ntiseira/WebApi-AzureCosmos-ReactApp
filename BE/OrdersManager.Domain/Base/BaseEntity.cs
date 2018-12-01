using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Domain.Base
{
    public abstract class BaseEntity : IEntity
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; }
    }
}
