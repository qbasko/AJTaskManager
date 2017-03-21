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
    public class UserNotificationController : TableController<UserNotification>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<UserNotification>(context, Request, Services);
        }

        // GET tables/UserNotification
        public IQueryable<UserNotification> GetAllUserNotification()
        {
            return Query(); 
        }

        // GET tables/UserNotification/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserNotification> GetUserNotification(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/UserNotification/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<UserNotification> PatchUserNotification(string id, Delta<UserNotification> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/UserNotification
        public async Task<IHttpActionResult> PostUserNotification(UserNotification item)
        {
            UserNotification current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/UserNotification/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteUserNotification(string id)
        {
             return DeleteAsync(id);
        }

    }
}