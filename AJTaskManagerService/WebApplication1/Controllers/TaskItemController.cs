using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Twitter.Messages;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class TaskItemController : Controller
    {
        // GET: TaskItem
        public async Task<ActionResult> Index(string sortOrder, string searchString)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            UserService userService = new UserService(accessToken);
            var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            
            if (String.IsNullOrWhiteSpace(userId))
                return RedirectToAction("Login", "Account");     

            TaskItemService taskItemService = new TaskItemService(accessToken);
            var taskItems = await taskItemService.GetTaskItems(userId);

            var groupIds = taskItems.Select(t => t.GroupId).Distinct();
            Dictionary<string, string> groups = new Dictionary<string, string>();
            foreach (var groupId in groupIds)
            {
                GroupService groupService = new GroupService(accessToken);
                string groupName = await groupService.GroupNameByGroupId(groupId);
                groups.Add(groupId, groupName);
            }
            var taskStatuses = await taskItemService.GetAvailableTaskStatuses();
            var tasks = new List<TaskItemModel>();
            foreach (var taskItem in taskItems)
            {
                TaskItemModel taskItemModel = GetTaskItemModel(taskItem);
                taskItemModel.GroupName = groups[taskItemModel.GroupId];
                taskItemModel.Status = taskStatuses.Single(s => s.Id == taskItemModel.StatusId).Status;
                tasks.Add(taskItemModel);
            }
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                tasks = tasks.Where(t => t.Name.Contains(searchString) || t.GroupName.Contains(searchString)).ToList();
            }

            ViewBag.TaskNameParam = String.IsNullOrEmpty(sortOrder) ? "taskName_desc" : "";
            ViewBag.GroupNameParam = sortOrder == "groupName" ? "groupName_desc" : "groupName";
            ViewBag.IsCompletedParam = sortOrder == "isCompleted" ? "isCompleted_desc" : "isCompleted";
            ViewBag.StatusParam = sortOrder == "status" ? "status_desc" : "status";
            ViewBag.StartDateParam = sortOrder == "startDate" ? "startDate_desc" : "startDate";
            ViewBag.EndDateParam = sortOrder == "endDate" ? "endDate_desc" : "endDate";

            tasks = OrderTasks(sortOrder, tasks);

            return View(tasks);
        }

        //Get
        public async Task<ActionResult> Subitems(string taskItemId)
        {
            return RedirectToAction("Index", "TaskSubitem", new {taskItemId});
        }
        // GET: TaskItem/Details/5
        public async Task<ActionResult> Details(string taskItemId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var taskItemService = new TaskItemService(accessToken);
            var taskItem = await taskItemService.GetTaskItemById(taskItemId);
            var taskItemModel = GetTaskItemModel(taskItem);

            var statuses = await taskItemService.GetAvailableTaskStatuses();
            taskItemModel.Status = statuses.Single(s => s.Id == taskItem.TaskStatusId).Status;

            var groupService = new GroupService(accessToken);
            taskItemModel.GroupName = await groupService.GroupNameByGroupId(taskItem.GroupId);
            return View(taskItemModel);
        }

        // GET: TaskItem/Create
        public async Task<ActionResult> Create()
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var groupService = new GroupService(accessToken);
            var groups = await groupService.GetGroupsAvailableForUserWhereUserIsAdmin(Session["UserId"] as string);
            var defaultGroup = groups.SingleOrDefault(g => g.GroupName.Contains("Default group for user:"));

            var taskItemService = new TaskItemService(accessToken);
            var statuses = await taskItemService.GetAvailableTaskStatuses();
            var defaultStatus = statuses.Single(s => s.Id == ((int)TaskStatusEnum.NotStarted).ToString());

            ViewBag.GroupID = new SelectList(groups, "Id", "GroupNameTruncated", defaultGroup.Id);
            ViewBag.StatusID = new SelectList(statuses, "Id", "Status", defaultStatus.Id);
            return View();

        }

        // POST: TaskItem/Create
        [HttpPost]
        public async Task<ActionResult> Create(TaskItemModel taskItemModel)
        {
            try
            {
                string accessToken = Session["MicrosoftAccessToken"] as string;
                var taskItem = GetTaskItemFromModel(taskItemModel);
                var taskItemService = new TaskItemService(accessToken);
                var userService = new UserService(accessToken);
                var usrId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);
                taskItem.Id = Guid.NewGuid().ToString();
                await taskItemService.InsertTaskItem(taskItem, usrId);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskItem/Edit/5
        public async Task<ActionResult> Edit(string taskItemId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            //UserService userService = new UserService(accessToken);
            //var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            var taskItemService = new TaskItemService(accessToken);
            var taskItem = await taskItemService.GetTaskItemById(taskItemId);

            var groupService = new GroupService(accessToken);
            var groups = await groupService.GetGroupsAvailableForUserWhereUserIsAdmin(Session["UserId"] as string);
            //var groupName = await groupService.GroupNameByGroupId(taskItem.GroupId);

            var statuses = await taskItemService.GetAvailableTaskStatuses();
            var status = statuses.Single(s => s.Id == taskItem.TaskStatusId);

            ViewBag.GroupId = new SelectList(groups, "Id", "GroupNameTruncated", taskItem.GroupId);
            ViewBag.StatusId = new SelectList(statuses, "Id", "Status", status.Id);

            var taskItemModel = GetTaskItemModel(taskItem);

            //
            
            if (await AccessAdmin(taskItem.GroupId))
            {
                return View(taskItemModel);
        
            }
            else if (await AccessEditor(taskItem.GroupId))
            {
                return View(taskItemModel);
            }
            else
            {
                return View("AccessDenied");
            }

            
        }

        // POST: TaskItem/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(string taskItemId, TaskItemModel taskItemModel)
        {
            try
            {
                var taskItem = GetTaskItemFromModel(taskItemModel);
                var taskItemService = new TaskItemService(Session["MicrosoftAccessToken"] as string);

                if (taskItem.IsCompleted ||
                  taskItem.TaskStatusId.Equals(((int)TaskStatusEnum.Completed).ToString()))
                {
                    taskItem.TaskStatusId = ((int)TaskStatusEnum.Completed).ToString();
                    taskItem.IsCompleted = true;
                }
                await taskItemService.UpdateTaskItem(taskItem);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskItem/Delete/5
        public async Task<ActionResult> Delete(string taskItemId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var taskItemService = new TaskItemService(accessToken);
            var taskItem = await taskItemService.GetTaskItemById(taskItemId);
            var taskItemModel = GetTaskItemModel(taskItem);

            var statuses = await taskItemService.GetAvailableTaskStatuses();
            taskItemModel.Status = statuses.Single(s => s.Id == taskItem.TaskStatusId).Status;

            var groupService = new GroupService(accessToken);
            taskItemModel.GroupName = await groupService.GroupNameByGroupId(taskItem.GroupId);

            if (await AccessAdmin(taskItem.GroupId))
            {
                return View(taskItemModel);
            }
            else
            {
                return View("AccessDenied");
            }

        }

        // POST: TaskItem/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(string taskItemId, TaskItemModel taskItemModel)
        {
            try
            {
                var taskItemService = new TaskItemService(Session["MicrosoftAccessToken"] as string);
                var taskItem = await taskItemService.GetTaskItemById(taskItemModel.TaskItemId);
                taskItem.IsDeleted = true;
               
                await taskItemService.UpdateTaskItem(taskItem);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        public async Task<bool> AccessEditor(string groupId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

           
            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserEditItem(userId, groupId);

        }

        public async Task<bool> AccessAdmin(string groupId)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;

            ToDoListService toDoListService = new ToDoListService(accessToken);


            UserService userService = new UserService(accessToken);
            string userId = await userService.GetUserInternalId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            RoleTypeService roleTypeService = new RoleTypeService(accessToken);

            return await roleTypeService.CanUserAddOrDeleteItem(userId, groupId);

        }

        private TaskItem GetTaskItemFromModel(TaskItemModel taskItemModel)
        {
            TaskItem taskItem = new TaskItem();
            //taskItem.Id = Guid.NewGuid().ToString();
            taskItem.Name = taskItemModel.Name;
            taskItem.Description = taskItemModel.Description;
            taskItem.StartDateTime = taskItemModel.StartDate;
            taskItem.EndDateTime = taskItemModel.EndDate;
            taskItem.GroupId = taskItemModel.GroupId;
            taskItem.Id = taskItemModel.TaskItemId;
            taskItem.IsCompleted = taskItemModel.IsCompleted;
            taskItem.TaskStatusId = taskItemModel.StatusId;
            return taskItem;
        }

        private TaskItemModel GetTaskItemModel(TaskItem taskItem)
        {
            TaskItemModel taskItemModel = new TaskItemModel();
            taskItemModel.TaskItemId = taskItem.Id;
            taskItemModel.Name = taskItem.Name;
            taskItemModel.Description = taskItem.Description;
            taskItemModel.StartDate = taskItem.StartDateTime.Value;
            taskItemModel.EndDate = taskItem.EndDateTime.Value;
            taskItemModel.GroupId = taskItem.GroupId;
            taskItemModel.IsCompleted = taskItem.IsCompleted;
            taskItemModel.StatusId = taskItem.TaskStatusId;
            return taskItemModel;

        }

        private List<TaskItemModel> OrderTasks(string sortOrder, List<TaskItemModel> tasks)
        {
            switch (sortOrder)
            {
                case "taskName_desc":
                    tasks = tasks.OrderByDescending(t => t.Name).ToList();
                    break;

                case "groupName":
                    tasks = tasks.OrderBy(t => t.GroupName).ToList();
                    break;

                case "groupName_desc":
                    tasks = tasks.OrderByDescending(t => t.GroupName).ToList();
                    break;

                case "isCompleted":
                    tasks = tasks.OrderBy(t => t.IsCompleted).ToList();
                    break;

                case "isCompleted_desc":
                    tasks = tasks.OrderByDescending(t => t.IsCompleted).ToList();
                    break;

                case "status":
                    tasks = tasks.OrderBy(t => t.Status).ToList();
                    break;

                case "status_desc":
                    tasks = tasks.OrderByDescending(t => t.Status).ToList();
                    break;

                case "startDate":
                    tasks = tasks.OrderBy(t => t.StartDate).ToList();
                    break;

                case "startDate_desc":
                    tasks = tasks.OrderByDescending(t => t.StartDate).ToList();
                    break;

                case "endDate":
                    tasks = tasks.OrderBy(t => t.EndDate).ToList();
                    break;

                case "endDate_desc":
                    tasks = tasks.OrderByDescending(t => t.EndDate).ToList();
                    break;

                default:
                    tasks = tasks.OrderBy(t => t.Name).ToList();
                    break;
            }
            return tasks;

        }
    }
}
