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
    public class UserGroupController : TableController<UserGroup>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<UserGroup>(context, Request, Services);
        }

        // GET tables/UserGroupRole
        public IQueryable<UserGroup> GetAllUserGroupRole()
        {
            return Query(); 
        }

        // GET tables/UserGroupRole/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserGroup> GetUserGroupRole(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/UserGroupRole/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserGroup> PatchUserGroupRole(string id, Delta<UserGroup> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/UserGroupRole
        public async Task<IHttpActionResult> PostUserGroupRole(UserGroup item)
        {
            UserGroup current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/UserGroupRole/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserGroupRole(string id)
        {
             return DeleteAsync(id);
        }

    }
}