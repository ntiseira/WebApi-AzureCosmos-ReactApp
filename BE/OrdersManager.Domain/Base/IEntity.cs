using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersManager.Domain.Base
{
    public interface IEntity
    {
        string Id { get; set; }
    }
}
