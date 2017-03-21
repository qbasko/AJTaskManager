using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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
    public class AddTodoListController : BaseController
    {
        private IUserService _userService;
        private IGroupService _groupService;
        private IToDoListService _todoListService;

        public AddTodoListController()
        {
            _userService = (IUserService)ServiceProvider.GetService(typeof(IUserService));
            _groupService = (IGroupService)ServiceProvider.GetService(typeof(IGroupService));
            _todoListService = (IToDoListService)ServiceProvider.GetService(typeof(IToDoListService));

        }

        public AddTodoListController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userService = (IUserService)ServiceProvider.GetService(typeof(IUserService));
            _groupService = (IGroupService)ServiceProvider.GetService(typeof(IGroupService));
            _todoListService = (IToDoListService)ServiceProvider.GetService(typeof(IToDoListService));

        }


        // GET: AddTodoList
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            string accessToken = Session != null ? Session["MicrosoftAccessToken"] as string : "";
            _userService.AccessToken = accessToken;
            string extUserId = Session != null ? Session["UserId"] as string : "test";

            //UserService userService = new UserService(Session["MicrosoftAccessToken"] as string);
            //var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            var userId = await _userService.GetUserId(extUserId, UserDomainEnum.Microsoft);

            if (String.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");

            _todoListService.AccessToken = accessToken;
            //ToDoListService todoListService = new ToDoListService(Session["MicrosoftAccessToken"] as string);
            // var usrList = await todoListService.GetUserLists(userId);
            var usrList = await _todoListService.GetUserLists(userId);
            var lists = new List<AddTodoListModel>();
            var groupIds = usrList.Select(g => g.GroupId).Distinct();
            Dictionary<string, string> groups = new Dictionary<string, string>();
            foreach (var groupId in groupIds)
            {
                _groupService.AccessToken = accessToken;
                //GroupService groupService = new GroupService(Session["MicrosoftAccessToken"] as string);

                //string groupName = await groupService.GroupNameByGroupId(groupId);
                string groupName = await _groupService.GroupNameByGroupId(groupId);
                groups.Add(groupId, groupName);
            }
            foreach (ToDoList todoList in usrList)
            {

                lists.Add(new AddTodoListModel() { GroupId = todoList.GroupId, GroupName = groups[todoList.GroupId], ListName = todoList.ListName, ListId = todoList.Id, IsCompleted = todoList.IsCompleted });
            }

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                lists =
                    lists.Where(l => l.ListName.Contains(searchString) || l.GroupName.Contains(searchString)).ToList();
            }

            ViewBag.ListNameSortParam = String.IsNullOrEmpty(sortOrder) ? "listName_desc" : "";
            ViewBag.GroupNameSortParam = sortOrder == "groupName" ? "groupName_desc" : "groupName";
            ViewBag.IsCompletedParam = sortOrder == "isCompleted" ? "isCompleted_desc" : "isCompleted";

            lists = OrderLists(sortOrder, lists);

            return View(lists);
        }

        // GET: AddTodoList/Details/5
        public ActionResult Details(string listId)
        {
            //return View();
            return RedirectToAction("Index", "AddTodoItem", new { listId });

        }

        // GET: AddTodoList/Create
        public async Task<ActionResult> Create()
        {
            ToDoListService toDoListService = new ToDoListService(Session["MicrosoftAccessToken"] as string);

            var usrGroups = await toDoListService.GetGroupForUser(Session["UserId"] as string);
            var defaultGroup = usrGroups.SingleOrDefault(g => g.GroupName.Contains("Default group for user:"));

            ViewBag.GroupID = new SelectList(usrGroups, "Id", "GroupNameTruncated", defaultGroup.Id);
            return View();
        }

        // POST: AddTodoList/Create
        [HttpPost]
        public async Task<ActionResult> Create(AddTodoListModel addTodoListModel)
        {
            DTO.ToDoList toDoList = new ToDoList();

            toDoList.Id = Guid.NewGuid().ToString();
            toDoList.ListName = addTodoListModel.ListName;
            toDoList.GroupId = addTodoListModel.GroupId;
            toDoList.IsCompleted = false;


            ToDoListService todoListService = new ToDoListService(Session["MicrosoftAccessToken"] as string);
            UserService userService = new UserService(Session["MicrosoftAccessToken"] as string);

            var usrId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);

            try
            {
                // TODO: Add insert logic here
                await todoListService.InsertTodoList(toDoList, usrId);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        // GET: AddTodoList/Edit/5
        public async Task<ActionResult> Edit(string listId)
        {
            UserService userService = new UserService(Session["MicrosoftAccessToken"] as string);
            var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);

            ToDoListService toDoListService = new ToDoListService(Session["MicrosoftAccessToken"] as string);
            var listByListId = await toDoListService.GetUserListById(listId, userId);

            var usrGroups = await toDoListService.GetGroupForUser(Session["UserId"] as string);
            //var defaultGroup = usrGroups.SingleOrDefault(g => g.GroupName.Contains("Default group for user:"));
            var usrGroupName = toDoListService.GetGroupForUserByGroupId(Session["MicrosoftAccessToken"] as string,
                listByListId.GroupId);

            ViewBag.GroupID = new SelectList(usrGroups, "Id", "GroupNameTruncated", listByListId.GroupId);

            string groupId = listByListId.GroupId;

            AddTodoListModel addTodoListModel = new AddTodoListModel();
            addTodoListModel.ListName = listByListId.ListName;
            addTodoListModel.IsCompleted = listByListId.IsCompleted;


            if (await AccessAdmin(userId, groupId))
            {
                return View(addTodoListModel);
            }
            else if (await AccessEditor(userId, groupId))
            {
                return View(addTodoListModel);
            }
            else
            {
                return View("AccessDenied");
            }
        }

        // POST: AddTodoList/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string listId, AddTodoListModel addTodoListModel)
        {
            try
            {
                // TODO: Add update logic here
                DTO.ToDoList tdList = new ToDoList();
                tdList.Id = listId;
                tdList.GroupId = addTodoListModel.GroupId;
                tdList.ListName = addTodoListModel.ListName;
                tdList.IsCompleted = addTodoListModel.IsCompleted;

                ToDoListService toDoList = new ToDoListService(Session["MicrosoftAccessToken"] as string);
                await toDoList.UpdateTodoList(tdList);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        // GET: AddTodoList/Delete/5
        public async Task<ActionResult> Delete(string listId)
        {
            UserService userService = new UserService(Session["MicrosoftAccessToken"] as string);
            var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);

            ToDoListService toDoListService = new ToDoListService(Session["MicrosoftAccessToken"] as string);
            var listByListId = await toDoListService.GetUserListById(listId, userId);

            //var usrGroups = await toDoListService.GetGroupForUser(Session["UserId"] as string);
            //var defaultGroup = usrGroups.SingleOrDefault(g => g.GroupName.Contains("Default group for user:"));
            //var usrGroupName = toDoListService.GetGroupForUserByGroupId(Session["MicrosoftAccessToken"] as string,
            //listByListId.GroupId);

            GroupService groupService = new GroupService(Session["MicrosoftAccessToken"] as string);

            var groupTName = await groupService.GroupNameByGroupId(listByListId.GroupId);

            //ViewBag.GroupID = new SelectList(usrGroups, "Id", "GroupNameTruncated", usrGroupName);

            AddTodoListModel addTodoListModel = new AddTodoListModel();
            addTodoListModel.ListName = listByListId.ListName;
            addTodoListModel.GroupName = groupTName;
            addTodoListModel.IsCompleted = listByListId.IsCompleted;

            string groupId = listByListId.GroupId;

            if (await AccessAdmin(userId, groupId))
            {
                return View(addTodoListModel);
            }
            else
            {
                return View("AccessDenied");
            }
        }

        // POST: AddTodoList/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string listId, AddTodoListModel addTodoListModel)
        {
            try
            {
                ToDoListService toDoList = new ToDoListService(Session["MicrosoftAccessToken"] as string);
                UserService userService = new UserService(Session["MicrosoftAccessToken"] as string);
                var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);
                DTO.ToDoList tdList = await toDoList.GetUserListById(addTodoListModel.ListId, userId);
                tdList.IsDeleted = true;
                await toDoList.UpdateTodoList(tdList);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }


        public async Task<bool> AccessEditor(string userId, string groupId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserEditItem(userId, groupId);
        }

        public async Task<bool> AccessAdmin(string userId, string groupId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserAddOrDeleteItem(userId, groupId);

        }

        private List<AddTodoListModel> OrderLists(string sortOrder, List<AddTodoListModel> lists)
        {
            switch (sortOrder)
            {
                case "listName_desc":
                    lists = lists.OrderByDescending(l => l.ListName).ToList();
                    break;

                case "groupName":
                    lists = lists.OrderBy(l => l.GroupName).ToList();
                    break;

                case "groupName_desc":
                    lists = lists.OrderByDescending(l => l.GroupName).ToList();
                    break;

                case "isCompleted":
                    lists = lists.OrderBy(l => l.IsCompleted).ToList();
                    break;

                case "isCompleted_desc":
                    lists = lists.OrderByDescending(l => l.IsCompleted).ToList();
                    break;

                default:
                    lists = lists.OrderBy(l => l.ListName).ToList();
                    break;
            }
            return lists;

        }
    }
}
