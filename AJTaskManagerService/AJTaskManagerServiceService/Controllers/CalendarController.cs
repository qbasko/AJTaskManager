using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using AJTaskManagerServiceService.DataObjects;
using AJTaskManagerServiceService.Models;

namespace AJTaskManagerServiceService.Controllers
{
    public class CalendarController : TableController<Calendar>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<Calendar>(context, Request, Services);
        }

        // GET tables/Calendar
        public IQueryable<Calendar> GetAllCalendar()
        {
            return Query(); 
        }

        // GET tables/Calendar/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Calendar> GetCalendar(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Calendar/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Calendar> PatchCalendar(string id, Delta<Calendar> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Calendar
        public async Task<IHttpActionResult> PostCalendar(Calendar item)
        {
            Calendar current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Calendar/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCalendar(string id)
        {
             return DeleteAsync(id);
        }

    }
}