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
    public class TaskItemController : TableController<TaskItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<TaskItem>(context, Request, Services);
        }

        // GET tables/TaskItem
        public IQueryable<TaskItem> GetAllTaskItem()
        {
            return Query(); 
        }

        // GET tables/TaskItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TaskItem> GetTaskItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TaskItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TaskItem> PatchTaskItem(string id, Delta<TaskItem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TaskItem
        public async Task<IHttpActionResult> PostTaskItem(TaskItem item)
        {
            TaskItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TaskItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTaskItem(string id)
        {
             return DeleteAsync(id);
        }

    }
}