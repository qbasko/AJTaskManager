using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication1.Services;
using IServiceProvider = System.IServiceProvider;

namespace WebApplication1.Controllers
{
    public class BaseController : Controller
    {
        protected IServiceProvider ServiceProvider
        {
            get;
            set;
        }

        public BaseController()
        {
            ServiceProvider = new ServiceProvider();

        }

        public BaseController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

        }
    }
}
