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
    public class UserDomainController : TableController<UserDomain>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<UserDomain>(context, Request, Services);
        }

        // GET tables/UserDomain
        public IQueryable<UserDomain> GetAllUserDomain()
        {
            return Query(); 
        }

        // GET tables/UserDomain/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserDomain> GetUserDomain(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/UserDomain/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserDomain> PatchUserDomain(string id, Delta<UserDomain> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/UserDomain
        public async Task<IHttpActionResult> PostUserDomain(UserDomain item)
        {
            UserDomain current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/UserDomain/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserDomain(string id)
        {
             return DeleteAsync(id);
        }

    }
}