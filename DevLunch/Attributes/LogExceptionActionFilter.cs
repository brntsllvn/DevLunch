using System.Web.Mvc;
using log4net;

namespace DevLunch.Attributes
{
    public class LogExceptionActionFilter:ActionFilterAttribute
    {
        private static ILog _logger = LogManager.GetLogger(typeof (LogExceptionActionFilter));

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                var message = string.Format("Error in {0}.{1}",filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,filterContext.ActionDescriptor.ActionName);
                _logger.Error(message, filterContext.Exception);        
            }
        }
    }
}