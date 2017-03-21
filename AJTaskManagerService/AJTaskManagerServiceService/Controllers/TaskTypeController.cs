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
    public class TaskTypeController : TableController<TaskType>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<TaskType>(context, Request, Services);
        }

        // GET tables/TaskType
        public IQueryable<TaskType> GetAllTaskType()
        {
            return Query(); 
        }

        // GET tables/TaskType/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TaskType> GetTaskType(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TaskType/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TaskType> PatchTaskType(string id, Delta<TaskType> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TaskType
        public async Task<IHttpActionResult> PostTaskType(TaskType item)
        {
            TaskType current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TaskType/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTaskType(string id)
        {
             return DeleteAsync(id);
        }

    }
}