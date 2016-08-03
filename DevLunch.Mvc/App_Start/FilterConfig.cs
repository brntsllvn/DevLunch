using System.Web;
using System.Web.Mvc;
using DevLunch.Attributes;

namespace DevLunch.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LogExceptionActionFilter());
            filters.Add(new LogEveryActionFilter());
        }
    }
}
