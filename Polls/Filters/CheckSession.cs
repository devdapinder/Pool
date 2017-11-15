using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Polls.Filters
{
    public class CheckSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["UserDetails"] == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}