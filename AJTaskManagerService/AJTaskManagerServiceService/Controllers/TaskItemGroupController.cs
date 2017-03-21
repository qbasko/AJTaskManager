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
    public class TaskItemGroupController : TableController<TaskItemGroup>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<TaskItemGroup>(context, Request, Services);
        }

        // GET tables/TaskItemGroup
        public IQueryable<TaskItemGroup> GetAllTaskItemGroup()
        {
            return Query(); 
        }

        // GET tables/TaskItemGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TaskItemGroup> GetTaskItemGroup(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TaskItemGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TaskItemGroup> PatchTaskItemGroup(string id, Delta<TaskItemGroup> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TaskItemGroup
        public async Task<IHttpActionResult> PostTaskItemGroup(TaskItemGroup item)
        {
            TaskItemGroup current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TaskItemGroup/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTaskItemGroup(string id)
        {
             return DeleteAsync(id);
        }

    }
}