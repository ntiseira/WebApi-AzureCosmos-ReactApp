using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OrdersManager.Common
{
    public static class ManageDirectory
    {
        public static string GetDirectory()
        {
        
        return HttpContext.Current == null
                        ? System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data")
                        : HttpContext.Current.Server.MapPath("~/App_Data");
        }

    }
}
