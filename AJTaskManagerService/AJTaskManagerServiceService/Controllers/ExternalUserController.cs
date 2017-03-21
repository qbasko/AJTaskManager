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
    public class ExternalUserController : TableController<ExternalUser>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<ExternalUser>(context, Request, Services);
        }

        // GET tables/ExternalUser
        public IQueryable<ExternalUser> GetAllExternalUser()
        {
            return Query(); 
        }

        // GET tables/ExternalUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<ExternalUser> GetExternalUser(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ExternalUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ExternalUser> PatchExternalUser(string id, Delta<ExternalUser> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/ExternalUser
        public async Task<IHttpActionResult> PostExternalUser(ExternalUser item)
        {
            ExternalUser current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ExternalUser/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteExternalUser(string id)
        {
             return DeleteAsync(id);
        }

    }
}