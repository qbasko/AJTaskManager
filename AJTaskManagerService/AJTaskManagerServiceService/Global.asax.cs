using System.Data.Entity;
using System.Web.Http;
using System.Web.Routing;

namespace AJTaskManagerServiceService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            WebApiConfig.Register();
        }
    }
}