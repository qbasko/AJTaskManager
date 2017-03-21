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
    public class TodoListController : TableController<TodoList>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            AJTaskManagerServiceContext context = new AJTaskManagerServiceContext();
            DomainManager = new EntityDomainManager<TodoList>(context, Request, Services);
        }

        // GET tables/TodoList
        public IQueryable<TodoList> GetAllTodoList()
        {
            return Query(); 
        }

        // GET tables/TodoList/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoList> GetTodoList(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoList/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoList> PatchTodoList(string id, Delta<TodoList> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/TodoList
        public async Task<IHttpActionResult> PostTodoList(TodoList item)
        {
            TodoList current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoList/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoList(string id)
        {
             return DeleteAsync(id);
        }

    }
}