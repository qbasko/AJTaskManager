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
    public class RoleTypeController : TableController<RoleType>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<RoleType>(context, Request, Services);
        }

        // GET tables/Role
        public IQueryable<RoleType> GetAllRole()
        {
            return Query(); 
        }

        // GET tables/Role/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<RoleType> GetRole(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Role/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<RoleType> PatchRole(string id, Delta<RoleType> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Role
        public async Task<IHttpActionResult> PostRole(RoleType item)
        {
            RoleType current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Role/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteRole(string id)
        {
             return DeleteAsync(id);
        }

    }
}