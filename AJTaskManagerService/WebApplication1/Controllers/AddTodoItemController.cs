using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class AddTodoItemController : Controller
    {
        // GET: AddTodoItem
        public async Task<ActionResult> Index(string listId, string sortOrder, string searchString)
        {

            ToDoItemService toDoItemService = new ToDoItemService(Session["MicrosoftAccessToken"] as string);
            var listItems = await toDoItemService.GetTodoListsItems(listId);
            var items = new List<AddTodoItemModel>();

            ToDoListService toDoListService = new ToDoListService(Session["MicrosoftAccessToken"] as string);
            var listName = await toDoListService.GetListById(listId);

            ViewBag.ListId = listId;
            //AddTodoItemModel addTodoItemModel = new AddTodoItemModel();
            //TO do 
            foreach (ToDoItem toDoItem in listItems)
            {
                items.Add(new AddTodoItemModel(){Id=toDoItem.Id, Text = toDoItem.Text, IsCompleted = toDoItem.IsCompleted, TodoListId = toDoItem.TodoListId, 
                    //TodoListName = listName.ListName
                });
                
            }

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                items =
                    items.Where(l => l.Text.Contains(searchString)).ToList();
            }

            ViewBag.ItemNameSortParam = String.IsNullOrEmpty(sortOrder) ? "listName_desc" : "";
            ViewBag.IsCompletedParam = sortOrder == "isCompleted" ? "isCompleted_desc" : "isCompleted";
            ViewBag.ListId = listId;

            items = SortItems(sortOrder, items);

            return View(items);
        }

        // GET: AddTodoItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AddTodoItem/Create
        public async Task<ActionResult> Create(string  listId)
        {
            if ((await AccessAdmin(listId)))
            {
                ViewBag.ListId = listId;
                return View();
            }
            else
            {
                ViewBag.ListId = listId;
                return View("AccessDenied");
            }
        

            
        }

        // POST: AddTodoItem/Create
        [HttpPost]
        public async Task<ActionResult> Create(string listId, AddTodoItemModel addTodoItemModel)
        {

            DTO.ToDoItem toDoItem = new ToDoItem();
            toDoItem.Id = Guid.NewGuid().ToString();
            toDoItem.Text = addTodoItemModel.Text;
            toDoItem.IsCompleted = false;
            toDoItem.IsDeleted = false;
            toDoItem.CreatedDateTime = DateTime.Now;
            toDoItem.Deadline = DateTime.Now.AddYears(100);
            toDoItem.TodoListId = listId;

            ToDoItemService toDoItemService = new ToDoItemService(Session["MicrosoftAccessToken"]as string);
            

            try
            {
                // TODO: Add insert logic here
                await toDoItemService.InsertTodoItem(toDoItem);
                return RedirectToAction("Index", new {listId});
            }
            catch
            {
                return View();
            }
        }

        // GET: AddTodoItem/Edit/5
        public async Task<ActionResult> Edit(string listId, string itemId)
        {
            ToDoItemService toDoItemService = new ToDoItemService(Session["MicrosoftAccessToken"] as string);
            var item = await toDoItemService.GetItemInfoByItemId(itemId);

            AddTodoItemModel addTodoItemModel = new AddTodoItemModel();
            addTodoItemModel.Id = item.Id;
            addTodoItemModel.IsCompleted = item.IsCompleted;
            addTodoItemModel.Text = item.Text;
            addTodoItemModel.TodoListId = item.TodoListId;

            ViewBag.ListId = listId;

            if (await AccessAdmin(listId))
            {
                return View(addTodoItemModel);
            }
            else if (await AccessEditor(listId))
            {
                return View(addTodoItemModel);
            }
            else
            {
                return View("AccessDenied");
            }
        }

        // POST: AddTodoItem/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string listId, string itemId, AddTodoItemModel addTodoItemModel)
        {

            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                ToDoItemService toDoItemService = new ToDoItemService(accessToken);

                // TODO: Add update logic here
                DTO.ToDoItem tdItem = await toDoItemService.GetItemInfoByItemId(itemId);
                
                //tdItem.Id = itemId;
                tdItem.Text = addTodoItemModel.Text;
                tdItem.IsCompleted = addTodoItemModel.IsCompleted;
                //tdItem.IsDeleted = false;
                //tdItem.TodoListId = listId;
               

                //ToDoItemService toDoItemService = new ToDoItemService(Session["MicrosoftAccessToken"] as string);
                await toDoItemService.UpdateTodoItem(tdItem);

                var allListItems = await toDoItemService.GetTodoListsItems(listId);
                if (allListItems.All(i => i.IsCompleted))
                {
                    var todoListService = new ToDoListService(accessToken);
                    var list = await todoListService.GetListById(listId);
                    list.IsCompleted = true;
                    await todoListService.UpdateTodoList(list);
                }

                ViewBag.ListId = listId;
                //ViewBag.ItemId = itemId;

                return RedirectToAction("Index", new {listId});
            }
            catch
            {
                return View();
            }
        }

        // GET: AddTodoItem/Delete/5
        public async  Task<ActionResult> Delete(string listId, string itemId)
        {
            ToDoItemService toDoItemService = new ToDoItemService(Session["MicrosoftAccessToken"]as string);
            var items = await toDoItemService.GetItemInfoByItemId(itemId);

            AddTodoItemModel addTodoItemModel = new AddTodoItemModel();
            addTodoItemModel.Id = items.Id;
            addTodoItemModel.Text = items.Text;
            addTodoItemModel.IsCompleted = items.IsCompleted;
            addTodoItemModel.TodoListId = items.TodoListId;

            ViewBag.ListId = listId;
            ViewBag.ItemId = itemId;
            if ((await AccessAdmin(listId)))
            {
                
                return View(addTodoItemModel);
            }
            else
            {
                
                return View("AccessDenied");
            }

           
        }

        // POST: AddTodoItem/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string listId, string itemId, AddTodoItemModel addTodoItemModel)
        {
            try
            {
                // TODO: Add delete logic here
                ToDoItemService toDoItemService = new ToDoItemService(Session["MicrosoftAccessToken"] as string);
                DTO.ToDoItem tdItem = await toDoItemService.GetItemInfoByItemId(itemId);

                tdItem.IsDeleted = true;

                await toDoItemService.UpdateTodoItem(tdItem);


                return RedirectToAction("Index", new {listId});
            }
            catch
            {
                return View();
            }
        }

       

     

        public async Task<bool> AccessEditor(string listId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

            ToDoListService toDoListService = new ToDoListService(accessToken);

            string groupId = (await toDoListService.GetListById(listId)).GroupId;


            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserEditItem(userId, groupId);

        }

        public async Task<bool> AccessAdmin(string listId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

            ToDoListService toDoListService = new ToDoListService(accessToken);

            string groupId = (await toDoListService.GetListById(listId)).GroupId;


            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserAddOrDeleteItem(userId, groupId);

        }
        private List<AddTodoItemModel> SortItems(string sortOrder, List<AddTodoItemModel> items)
        {
            switch (sortOrder)
            {
                case "listName_desc":
                    items = items.OrderByDescending(l => l.Text).ToList();
                    break;

                case "isCompleted":
                    items = items.OrderBy(l => l.IsCompleted).ToList();
                    break;

                case "isCompleted_desc":
                    items = items.OrderByDescending(l => l.IsCompleted).ToList();
                    break;

                default:
                    items = items.OrderBy(l => l.Text).ToList();
                    break;
            }
            return items;
        }
    }
}
