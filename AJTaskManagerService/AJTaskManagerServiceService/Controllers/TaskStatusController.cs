using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using AJTaskManagerServiceService.Models;
using TaskStatus = AJTaskManagerServiceService.DataObjects.TaskStatus;

namespace AJTaskManagerServiceService.Controllers
{
    public class TaskStatusController : TableController<TaskStatus>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<TaskStatus>(context, Request, Services);
        }

        // GET tables/TaskStatus
        public IQueryable<TaskStatus> GetAllTaskStatus()
        {
            return Query(); 
        }

        // GET tables/TaskStatus/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TaskStatus> GetTaskStatus(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TaskStatus/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TaskStatus> PatchTaskStatus(string id, Delta<TaskStatus> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TaskStatus
        public async Task<IHttpActionResult> PostTaskStatus(TaskStatus item)
        {
            TaskStatus current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TaskStatus/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTaskStatus(string id)
        {
             return DeleteAsync(id);
        }

    }
}