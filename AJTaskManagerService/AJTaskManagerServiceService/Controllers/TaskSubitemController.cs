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
    public class TaskSubitemController : TableController<TaskSubitem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<TaskSubitem>(context, Request, Services);
        }

        // GET tables/TaskSubitem
        public IQueryable<TaskSubitem> GetAllTaskSubitem()
        {
            return Query(); 
        }

        // GET tables/TaskSubitem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TaskSubitem> GetTaskSubitem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TaskSubitem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TaskSubitem> PatchTaskSubitem(string id, Delta<TaskSubitem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TaskSubitem
        public async Task<IHttpActionResult> PostTaskSubitem(TaskSubitem item)
        {
            TaskSubitem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TaskSubitem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTaskSubitem(string id)
        {
             return DeleteAsync(id);
        }

    }
}