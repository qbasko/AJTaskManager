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
    public class GroupTaskItemController : TableController<GroupTaskItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<GroupTaskItem>(context, Request, Services);
        }

        // GET tables/GroupTaskItem
        public IQueryable<GroupTaskItem> GetAllGroupTaskItem()
        {
            return Query(); 
        }

        // GET tables/GroupTaskItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<GroupTaskItem> GetGroupTaskItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/GroupTaskItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<GroupTaskItem> PatchGroupTaskItem(string id, Delta<GroupTaskItem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/GroupTaskItem
        public async Task<IHttpActionResult> PostGroupTaskItem(GroupTaskItem item)
        {
            GroupTaskItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/GroupTaskItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteGroupTaskItem(string id)
        {
             return DeleteAsync(id);
        }

    }
}