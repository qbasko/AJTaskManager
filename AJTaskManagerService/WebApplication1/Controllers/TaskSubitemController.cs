using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Syncfusion.Data.Extensions;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;


namespace WebApplication1.Controllers
{
    public class TaskSubitemController : BaseController
    {
        private ITaskSubitemService _taskSubitemService;
        private IUserService _userService;
        private ITaskItemService _taskItemService;

        public TaskSubitemController()
        {
            _taskSubitemService = (ITaskSubitemService)ServiceProvider.GetService(typeof(ITaskSubitemService));
            _userService = (IUserService) ServiceProvider.GetService(typeof (IUserService));
            _taskItemService = (ITaskItemService) ServiceProvider.GetService(typeof (ITaskItemService));
        }

        public TaskSubitemController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _taskSubitemService = (ITaskSubitemService)ServiceProvider.GetService(typeof(ITaskSubitemService));
            _userService = (IUserService) ServiceProvider.GetService(typeof (IUserService));
            _taskItemService = (ITaskItemService) ServiceProvider.GetService(typeof (ITaskItemService));
        }

        // GET: TaskSubitem
        public async Task<ActionResult> Index(string taskItemId, string sortOrder, string searchString)
        {
            ViewBag.TaskItemId = taskItemId;

           // string accessToken = Session["MicrosoftAccessToken"] as string;
            string accessToken = Session != null ? Session["MicrosoftAccessToken"] as string : "";

            _taskItemService.AccessToken = accessToken;
            _userService.AccessToken = accessToken;
            _taskSubitemService.AccessToken = accessToken;

            if (String.IsNullOrWhiteSpace(await UserIsLogin(accessToken)))
            {
                return RedirectToAction("Login", "Account");
            }

            //TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
            var taskSubitems = await _taskSubitemService.GetTaskSubitems(taskItemId);

            var statuses = await _taskSubitemService.GetAvailableTaskStatuses();

            //UserService userService = new UserService(accessToken);
            //TaskItemService taskItemService = new TaskItemService(accessToken);

            var taskInfo = await _taskItemService.GetTaskItemById(taskItemId);

            var users = await _userService.GetUsersFromGroup(taskInfo.GroupId);

            Dictionary<string, string> fullname = new Dictionary<string, string>();
            foreach (var user in users)
            {
                fullname.Add(user.Id, user.FullName);
            }

            var otherTasksSubitem = await _taskSubitemService.GetTaskSubitems(taskItemId);

            Dictionary<string, string> taskSubItemDictionary = new Dictionary<string, string>();
            foreach (var oTS in otherTasksSubitem)
            {
                taskSubItemDictionary.Add(oTS.Id, oTS.Name);
            }


            var subItems = new List<TaskSubitemModel>();

            foreach (var taskSubitem in taskSubitems)
            {
                TaskSubitemModel taskSubitemModel = GetTaskSubitemModel(taskSubitem);
                taskSubitemModel.Status = statuses.Single(s => s.Id == taskSubitemModel.StatusId).Status;
                taskSubitemModel.Executor = !String.IsNullOrWhiteSpace(taskSubitem.ExecutorId) ? fullname[taskSubitemModel.ExecutorId] : null;
                taskSubitemModel.Predecessor = !String.IsNullOrWhiteSpace(taskSubitem.PredecessorId) ? taskSubItemDictionary[taskSubitemModel.PredecessorId] : null;
                taskSubitemModel.Successor = !String.IsNullOrWhiteSpace(taskSubitem.SuccessorId) ? taskSubItemDictionary[taskSubitemModel.SuccessorId] : null;
                subItems.Add(taskSubitemModel);
            }

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                subItems = subItems.Where(t => t.Name.Contains(searchString)).ToList();
            }
            ViewBag.TaskSubitemNameParam = String.IsNullOrEmpty(sortOrder) ? "taskName_desc" : "";
            ViewBag.StatusParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.StartDateParam = sortOrder == "startDate" ? "startDate_desc" : "startDate";
            ViewBag.EndDateParam = sortOrder == "endDate" ? "endDate_desc" : "endDate";
            ViewBag.EstimationParam = sortOrder == "estimation" ? "estimation_desc" : "estimation";
            ViewBag.PredecessorParam = sortOrder == "predecessor" ? "predecessor_desc" : "predecessor";
            ViewBag.SuccessorParam = sortOrder == "successor" ? "successor_desc" : "successor";
            ViewBag.IsCompletedParam = sortOrder == "isCompleted" ? "isCompleted_desc" : "isCompleted";
            ViewBag.ExecutorParam = sortOrder == "executor" ? "executor_desc" : "executor";

            subItems = OrderTaskSubitems(sortOrder, subItems);

            return View(subItems);
        }

        private static List<TaskSubitemModel> OrderTaskSubitems(string sortOrder, List<TaskSubitemModel> subItems)
        {
            switch (sortOrder)
            {
                case "taskName_desc":
                    subItems = subItems.OrderByDescending(t => t.Name).ToList();
                    break;

                case "status":
                    subItems = subItems.OrderBy(t => t.Status).ToList();
                    break;

                case "status_desc":
                    subItems = subItems.OrderByDescending(t => t.Status).ToList();
                    break;

                case "startDate":
                    subItems = subItems.OrderBy(t => t.StartDateTime).ToList();
                    break;

                case "startDate_desc":
                    subItems = subItems.OrderByDescending(t => t.StartDateTime).ToList();
                    break;

                case "endDate":
                    subItems = subItems.OrderBy(t => t.EndDateTime).ToList();
                    break;

                case "endDate_desc":
                    subItems = subItems.OrderByDescending(t => t.EndDateTime).ToList();
                    break;

                case "estimation":
                    subItems = subItems.OrderBy(t => t.EstimationInHours).ToList();
                    break;

                case "estimation_desc":
                    subItems = subItems.OrderByDescending(t => t.EstimationInHours).ToList();
                    break;

                case "predecessor":
                    subItems = subItems.OrderBy(t => t.Predecessor).ToList();
                    break;

                case "predecessor_desc":
                    subItems = subItems.OrderByDescending(t => t.Predecessor).ToList();
                    break;

                case "successor":
                    subItems = subItems.OrderBy(t => t.Successor).ToList();
                    break;

                case "successor_desc":
                    subItems = subItems.OrderByDescending(t => t.Successor).ToList();
                    break;

                case "isCompleted":
                    subItems = subItems.OrderBy(t => t.IsCompleted).ToList();
                    break;

                case "isCompleted_desc":
                    subItems = subItems.OrderByDescending(t => t.IsCompleted).ToList();
                    break;

                case "executor":
                    subItems = subItems.OrderBy(t => t.Executor).ToList();
                    break;

                case "executor_desc":
                    subItems = subItems.OrderByDescending(t => t.Executor).ToList();
                    break;

                default:
                    subItems = subItems.OrderBy(t => t.Name).ToList();
                    break;
            }

            return subItems;
        }

        // GET: TaskSubitem/Details/5
        public async Task<ActionResult> Details(string taskItemId, string taskSubitemId)
        {
            ViewBag.TaskItemId = taskItemId;
            ViewBag.TaskSubitemId = taskSubitemId;

            //var accessToken = Session["MicrosoftAccessToken"] as string;
            string accessToken = Session != null ? Session["MicrosoftAccessToken"] as string : "";

            _taskItemService.AccessToken = accessToken;
            _userService.AccessToken = accessToken;
            _taskSubitemService.AccessToken = accessToken;
            //TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
            TaskSubitem taskSubitem = await _taskSubitemService.GetTaskSubitemById(taskSubitemId);
            TaskSubitemModel taskSubitemModel = GetTaskSubitemModel(taskSubitem);

            var statuses = await _taskSubitemService.GetAvailableTaskStatuses();
            taskSubitemModel.Status = statuses.Single(s => s.Id == taskSubitem.TaskStatusId).Status;

            //ViewBag.StatusID = new SelectList(statuses, "Id", "Status", status.Id);

            var tasks = new ObservableCollection<TaskSubitem>() { new TaskSubitem() };
            var items = await _taskSubitemService.GetTaskSubitems(taskItemId);

            if (!String.IsNullOrWhiteSpace(taskSubitem.PredecessorId))
            {
                taskSubitemModel.Predecessor = await _taskSubitemService.GetTaskSubitemNameByTaskSubitemId(taskSubitem.PredecessorId);
            }
            else
            {
                taskSubitemModel.Predecessor = "";
            }
            if (!String.IsNullOrWhiteSpace(taskSubitem.SuccessorId))
            {
                taskSubitemModel.Successor =
                    await _taskSubitemService.GetTaskSubitemNameByTaskSubitemId(taskSubitem.SuccessorId);

            }
            else
            {
                taskSubitemModel.Successor = "";
            }



            //UserService userService = new UserService(accessToken);

            if (!String.IsNullOrWhiteSpace(taskSubitem.ExecutorId))
            {
                taskSubitemModel.Executor = (await _userService.GetUserById(taskSubitem.ExecutorId)).FullName;
            }
            else
            {
                taskSubitemModel.Executor = "";
            }




            return View(taskSubitemModel);
        }

        // GET: TaskSubitem/Create
        public async Task<ActionResult> Create(string taskItemId)
        {
            ViewBag.TaskItemId = taskItemId;

            string accessToken = Session["MicrosoftAccessToken"] as string;
            TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
            var statuses = await taskSubitemService.GetAvailableTaskStatuses();
            var defaultStatus = statuses.Single(s => s.Id == ((int)TaskStatusEnum.NotStarted).ToString());

            ViewBag.StatusID = new SelectList(statuses, "Id", "Status", defaultStatus.Id);

            var tasks = new ObservableCollection<TaskSubitem>() { new TaskSubitem() };
            var items = await taskSubitemService.GetTaskSubitems(taskItemId);
            items.ForEach(i => tasks.Add(i));

            ViewBag.PredecessorID = new SelectList(tasks, "Id", "Name", new TaskSubitem());
       

            ViewBag.SuccessorID = new SelectList(tasks, "Id", "Name", new TaskSubitem());


            UserService userService = new UserService(accessToken);
            TaskItemService taskItemService = new TaskItemService(accessToken);
            UserGroupService userGroup = new UserGroupService(accessToken);

            var itemInfo = await taskItemService.GetTaskItemById(taskItemId);
            var groupInfo = await userGroup.GetUserGroupTableForGroup(itemInfo.GroupId);
            var userInGroup = groupInfo.Select(u => u.UserId);
            List<User> userProp = new List<User>();
            //Dictionary<string,string> userProp = new Dictionary<string, string>();
            foreach (var usrId in userInGroup)
            {
                var user = await userService.GetUserById(usrId);
                //var userFullName = user.FullName;
                userProp.Add(user);
            }

            var userInternalId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            var defaultExecutor = userProp.Single(i => i.Id == userInternalId);
            ViewBag.ExecutorID = new SelectList(userProp, "Id", "FullName", defaultExecutor.Id);

            if (await AccessAdmin(taskItemId))
            {
                return View();
            }
            else
            {
                return View("AccessDenied");
            }



        }

        // POST: TaskSubitem/Create
        [HttpPost]
        public async Task<ActionResult> Create(string taskItemId, TaskSubitemModel taskSubitemModel)
        {
            ViewBag.TaskItemId = taskItemId;
            string accessToken = Session["MicrosoftAccessToken"] as string;

            try
            {
                var taskSubitem = GetTaskSubitemFromModel(taskItemId, taskSubitemModel);
                taskSubitem.Id = Guid.NewGuid().ToString();
                TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
                await taskSubitemService.InsertTaskSubitem(taskSubitem);
                // TODO: Add insert logic here

                return RedirectToAction("Index", new { taskItemId });
            }

            catch
            {
                return View();
            }
        }

        // GET: TaskSubitem/Edit/5
        public async Task<ActionResult> Edit(string taskItemId, string taskSubitemId)
        {
            ViewBag.TaskItemId = taskItemId;
            string accessToken = Session["MicrosoftAccessToken"] as string;
            TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
            TaskSubitem taskSubitemInfo = await taskSubitemService.GetTaskSubitemById(taskSubitemId);

            TaskSubitemModel taskSubitemModel = GetTaskSubitemModel(taskSubitemInfo);

            var statuses = await taskSubitemService.GetAvailableTaskStatuses();
            var status = statuses.Single(s => s.Id == taskSubitemInfo.TaskStatusId);

            ViewBag.StatusID = new SelectList(statuses, "Id", "Status", status.Id);

            var tasks = new ObservableCollection<TaskSubitem>() { new TaskSubitem() };
            var items = await taskSubitemService.GetTaskSubitems(taskItemId);
            items.ForEach(i => tasks.Add(i));

            if (String.IsNullOrWhiteSpace(taskSubitemInfo.PredecessorId))
            {
                ViewBag.PredecessorID = new SelectList(tasks, "Id", "Name", new TaskSubitem());
            }
            else
            {
                ViewBag.PredecessorID = new SelectList(tasks, "Id", "Name", taskSubitemInfo.PredecessorId);
            }

            if (String.IsNullOrWhiteSpace(taskSubitemInfo.SuccessorId))
            {
                ViewBag.SuccessorID = new SelectList(tasks, "Id", "Name", new TaskSubitem());
            }
            else
            {
                ViewBag.SuccessorID = new SelectList(tasks, "Id", "Name", taskSubitemInfo.SuccessorId);

            }

            UserService userService = new UserService(accessToken);
            TaskItemService taskItemService = new TaskItemService(accessToken);
            UserGroupService userGroup = new UserGroupService(accessToken);

            var itemInfo = await taskItemService.GetTaskItemById(taskItemId);
            var groupInfo = await userGroup.GetUserGroupTableForGroup(itemInfo.GroupId);
            var userInGroup = groupInfo.Select(u => u.UserId);
            List<User> userProp = new List<User>();

            userProp.Add(new User());
            foreach (var usrId in userInGroup)
            {
                var user = await userService.GetUserById(usrId);
                userProp.Add(user);
            }

            var userInternalId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            //var defaultExecutor = userProp.Single(i => i.Id == userInternalId);

            if (String.IsNullOrWhiteSpace(taskSubitemInfo.ExecutorId))
            {

                ViewBag.ExecutorID = new SelectList(userProp, "Id", "FullName", new TaskSubitem());
            }
            else
            {
                ViewBag.ExecutorID = new SelectList(userProp, "Id", "FullName", taskSubitemInfo.ExecutorId);
            }

            if (await AccessAdmin(taskItemId))
            {
                return View(taskSubitemModel);
            }
            else if (await AccessEditor(taskItemId))
            {
                return View(taskSubitemModel);
            }
            else
            {
                return View("AccessDenied");
            }



        }

        // POST: TaskSubitem/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string taskItemId, string taskSubitemId, TaskSubitemModel taskSubitemModel)
        {
            ViewBag.TaskItemId = taskItemId;
            var accessToken = Session["MicrosoftAccessToken"] as string;
            TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
            
            try
            {
                // TODO: Add update logic here
                TaskSubitem taskSubitem = GetTaskSubitemFromModel(taskItemId, taskSubitemModel);
                taskSubitem.Id = taskSubitemId;

                if (taskSubitem.IsCompleted ||
                    taskSubitem.TaskStatusId.Equals(((int) TaskStatusEnum.Completed).ToString()))
                {
                    taskSubitem.TaskStatusId = ((int)TaskStatusEnum.Completed).ToString();
                    taskSubitem.IsCompleted = true;
                }

                await taskSubitemService.UpdateTaskSubitem(taskSubitem);

                var allTaskSubitems = await taskSubitemService.GetTaskSubitems(taskItemId);

                if (allTaskSubitems.All(t => t.IsCompleted))
                {
                    var taskItemService = new TaskItemService(accessToken);
                    var taskItem = await taskItemService.GetTaskItemById(taskItemId);
                    taskItem.IsCompleted = true;
                    taskItem.TaskStatusId = ((int)TaskStatusEnum.Completed).ToString();
                    await taskItemService.UpdateTaskItem(taskItem);
                }

                return RedirectToAction("Index", new { taskItemId });
            }

            catch
            {
                return View(taskSubitemModel);
            }
        }

        // GET: TaskSubitem/Delete/5
        public async Task<ActionResult> Delete(string taskItemId, string taskSubitemId)
        {
            ViewBag.TaskItemId = taskItemId;

            var accessToken = Session["MicrosoftAccessToken"] as string;
            TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
            TaskSubitem taskSubitem = await taskSubitemService.GetTaskSubitemById(taskSubitemId);
            TaskSubitemModel taskSubitemModel = GetTaskSubitemModel(taskSubitem);

            var statuses = await taskSubitemService.GetAvailableTaskStatuses();
            taskSubitemModel.Status = statuses.Single(s => s.Id == taskSubitem.TaskStatusId).Status;

            //ViewBag.StatusID = new SelectList(statuses, "Id", "Status", status.Id);

            var tasks = new ObservableCollection<TaskSubitem>() { new TaskSubitem() };
            var items = await taskSubitemService.GetTaskSubitems(taskItemId);

            if (!String.IsNullOrWhiteSpace(taskSubitem.PredecessorId))
            {
                taskSubitemModel.Predecessor = await taskSubitemService.GetTaskSubitemNameByTaskSubitemId(taskSubitem.PredecessorId);
            }
            else
            {
                taskSubitemModel.Predecessor = "";
            }
            if (!String.IsNullOrWhiteSpace(taskSubitem.SuccessorId))
            {
                taskSubitemModel.Successor =
                    await taskSubitemService.GetTaskSubitemNameByTaskSubitemId(taskSubitem.SuccessorId);

            }
            else
            {
                taskSubitemModel.Successor = "";
            }



            UserService userService = new UserService(accessToken);

            if (!String.IsNullOrWhiteSpace(taskSubitem.ExecutorId))
            {
                taskSubitemModel.Executor = (await userService.GetUserById(taskSubitem.ExecutorId)).FullName;
            }
            else
            {
                taskSubitemModel.Executor = "";
            }

            if (await AccessAdmin(taskItemId))
            {
                return View(taskSubitemModel);
            }
            else
            {
                return View("AccessDenied");
            }




        }

        // POST: TaskSubitem/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string taskItemId, string taskSubitemId, TaskSubitemModel taskSubitemModel)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

            try
            {
                // TODO: Add delete logic here
                TaskSubitemService taskSubitemService = new TaskSubitemService(accessToken);
                TaskSubitem taskSubitem = await taskSubitemService.GetTaskSubitemById(taskSubitemId);
                taskSubitem.IsDeleted = true;

                await taskSubitemService.UpdateTaskSubitem(taskSubitem);

                return RedirectToAction("Index", new { taskItemId });
            }

            catch
            {
                return View();
            }
        }


        public async Task<bool> AccessEditor(string taskItemId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;
            TaskItemService taskItemService =new TaskItemService(accessToken);
           
            string groupId = (await taskItemService.GetTaskItemById(taskItemId)).GroupId;

            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserEditItem(userId, groupId);

        }

        public async Task<bool> AccessAdmin(string taskItemId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

            TaskItemService taskItemService = new TaskItemService(accessToken);

            string groupId = (await taskItemService.GetTaskItemById(taskItemId)).GroupId;


            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserAddOrDeleteItem(userId, groupId);

        }

        public async Task<string> UserIsLogin(string accessToken)
        {
            try
            {
                _userService.AccessToken = accessToken;
                var userId = Session != null ? Session["UserId"] as string : "";
                var res = await _userService.GetUserId(userId, UserDomainEnum.Microsoft);

                return res;
            }

            catch
            {
                return null;
            }
        }

        private TaskSubitem GetTaskSubitemFromModel(string taskItemId, TaskSubitemModel taskSubitemModel)
        {
            TaskSubitem taskSubitem = new TaskSubitem();

            taskSubitem.Name = taskSubitemModel.Name;
            taskSubitem.Description = taskSubitemModel.Description;
            taskSubitem.TaskItemId = taskItemId;
            taskSubitem.TaskStatusId = taskSubitemModel.StatusId;
            taskSubitem.StartDateTime = taskSubitemModel.StartDateTime;
            taskSubitem.EndDateTime = taskSubitemModel.EndDateTime;
            taskSubitem.EstimationInHours = taskSubitemModel.EstimationInHours;
            taskSubitem.IsDeleted = false;
            taskSubitem.PredecessorId = taskSubitemModel.PredecessorId;
            taskSubitem.SuccessorId = taskSubitemModel.SuccessorId;
            taskSubitem.IsCompleted = taskSubitemModel.IsCompleted;
            taskSubitem.ExecutorId = taskSubitemModel.ExecutorId;

            return taskSubitem;

        }

        private TaskSubitemModel GetTaskSubitemModel(TaskSubitem taskSubitem)
        {
            TaskSubitemModel taskSubitemModel = new TaskSubitemModel();
            taskSubitemModel.Id = taskSubitem.Id;
            taskSubitemModel.Name = taskSubitem.Name;
            taskSubitemModel.StatusId = taskSubitem.TaskStatusId;
            taskSubitemModel.Description = taskSubitem.Description;
            taskSubitemModel.TaskItemId = taskSubitem.TaskItemId;
            taskSubitemModel.StartDateTime = taskSubitem.StartDateTime.Value;
            taskSubitemModel.EndDateTime = taskSubitem.EndDateTime.Value;
            taskSubitemModel.EstimationInHours = taskSubitem.EstimationInHours;
            taskSubitemModel.PredecessorId = taskSubitem.PredecessorId;
            taskSubitemModel.SuccessorId = taskSubitem.SuccessorId;
            taskSubitemModel.IsCompleted = taskSubitem.IsCompleted;
            taskSubitemModel.ExecutorId = taskSubitem.ExecutorId;

            return taskSubitemModel;


        }

    }
}
