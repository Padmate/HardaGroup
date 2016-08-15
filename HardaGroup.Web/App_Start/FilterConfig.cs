using HardaGroup.Web.Attributes;
using System.Web;
using System.Web.Mvc;

namespace HardaGroup.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new ApplicationHandleErrorAttribute());
        }
    }
}
