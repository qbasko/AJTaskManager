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
    public class CalendarTaskSubitemController : TableController<CalendarTaskSubitem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<CalendarTaskSubitem>(context, Request, Services);
        }

        // GET tables/CalendarTaskSubitem
        public IQueryable<CalendarTaskSubitem> GetAllCalendarTaskSubitem()
        {
            return Query(); 
        }

        // GET tables/CalendarTaskSubitem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<CalendarTaskSubitem> GetCalendarTaskSubitem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/CalendarTaskSubitem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<CalendarTaskSubitem> PatchCalendarTaskSubitem(string id, Delta<CalendarTaskSubitem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/CalendarTaskSubitem
        public async Task<IHttpActionResult> PostCalendarTaskSubitem(CalendarTaskSubitem item)
        {
            CalendarTaskSubitem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/CalendarTaskSubitem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteCalendarTaskSubitem(string id)
        {
             return DeleteAsync(id);
        }

    }
}