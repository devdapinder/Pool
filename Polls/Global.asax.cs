using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Polls
{
    public class MvcApplication : System.Web.HttpApplication
    {
     
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
             BundleConfig.RegisterBundles(BundleTable.Bundles);
            Common.CommonUtility.ApirUrl = ConfigurationManager.AppSettings["ApiUrl"].ToString();
            Common.CommonUtility.FullImageBaseUrl = ConfigurationManager.AppSettings["FullImageBaseUrl"].ToString();
            Common.CommonUtility.ThumbnailBaseUrl = ConfigurationManager.AppSettings["ThumbnailImageBaseUrl"].ToString();
        }
    }
}
