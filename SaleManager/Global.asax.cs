using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SaleCore.Binder;
using TwitterBootstrapMVC;

namespace SaleManager
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Bootstrap.Configure();
            ModelBinders.Binders.Add(typeof(DateTime), new CustomDateBinder());
            ModelBinders.Binders.Add(typeof(DateTime?), new NullableCustomDateBinder());
        }
    }
}
