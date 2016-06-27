using System.Web.Mvc;
using log4net;

namespace DevLunch.Attributes
{
    public class LogEveryActionFilter:ActionFilterAttribute
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof (LogEveryActionFilter));

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userName = "";
            if (filterContext.HttpContext.User?.Identity != null)
            {
                userName = filterContext.HttpContext.User.Identity.Name;
            }
            
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionname = filterContext.ActionDescriptor.ActionName;

            _logger.InfoFormat($"User {userName} executed action {controllerName}/{actionname}");
        }
    }
}