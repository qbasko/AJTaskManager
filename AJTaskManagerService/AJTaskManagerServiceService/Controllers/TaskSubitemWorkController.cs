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
    public class TaskSubitemWorkController : TableController<TaskSubitemWork>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<TaskSubitemWork>(context, Request, Services);
        }

        // GET tables/TaskSubitemWork
        public IQueryable<TaskSubitemWork> GetAllTaskSubitemWork()
        {
            return Query(); 
        }

        // GET tables/TaskSubitemWork/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TaskSubitemWork> GetTaskSubitemWork(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TaskSubitemWork/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TaskSubitemWork> PatchTaskSubitemWork(string id, Delta<TaskSubitemWork> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TaskSubitemWork
        public async Task<IHttpActionResult> PostTaskSubitemWork(TaskSubitemWork item)
        {
            TaskSubitemWork current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TaskSubitemWork/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTaskSubitemWork(string id)
        {
             return DeleteAsync(id);
        }

    }
}