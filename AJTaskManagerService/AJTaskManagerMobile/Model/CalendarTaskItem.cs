using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Helpers;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.Common;
using GalaSoft.MvvmLight.Ioc;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Controls;

namespace AJTaskManagerMobile.Model
{
    public class CalendarTaskItem : Dictionary<DateTime, TaskSubitem>
    {
        private readonly ITaskItemDataService _taskItemDataService;
        private readonly ITaskSubitemDataService _taskSubitemDataService;
        private readonly IUserDataService _userDataService;

        public CalendarTaskItem()
        {
            _userDataService = SimpleIoc.Default.GetInstance<IUserDataService>();
            _taskItemDataService = SimpleIoc.Default.GetInstance<ITaskItemDataService>();
            _taskSubitemDataService = SimpleIoc.Default.GetInstance<ITaskSubitemDataService>();
            Clear();
            AsyncHelpers.RunSync(Initialize);

            //Task.Run(async () => await Initialize());
            //Add(DateTime.Now.Date, new TaskItem() { Name = "Test1", Description = "TestDesc1" });
            //Add(DateTime.Now.AddDays(5).Date, new TaskItem() { Name = "Test1", Description = "TestDesc1" });
            //Add(DateTime.Now.AddDays(6).Date, new TaskItem() { Name = "Test2", Description = "TestDesc2" });
            //Add(DateTime.Now.AddDays(7).Date, new TaskItem() { Name = "Test3", Description = "TestDesc3" });
        }

        private async Task Initialize()
        {
            var userId = AccountHelper.GetCurrentUserId();
            
            string userInternalId = await _userDataService.GetUserInternalId(userId, Constants.MainAuthenticationDomain);
            var taskItems = await _taskItemDataService.GetTaskItems(userInternalId);

            var tasksNotCompleted = taskItems.Where(t => t.TaskStatusId != ((int) TaskStatusEnum.Completed).ToString());

            List<TaskSubitem> userTaskSubitems = new List<TaskSubitem>();
            foreach (var taskItem in tasksNotCompleted)
            {
                var taskSubitems = await _taskSubitemDataService.GetTaskSubitems(taskItem.Id);
                var items =  taskSubitems.Where(
                    t =>
                        t.ExecutorId == userInternalId &&
                        t.EndDateTime.HasValue &&
                        t.TaskStatusId != ((int) TaskStatusEnum.Completed).ToString()).ToList();
                if(items.Any())
                    userTaskSubitems.AddRange(items);
            }

            var tasksGroupedByEndDate = userTaskSubitems.GroupBy(t => t.EndDateTime.Value.Date);
            foreach (var dateTask in tasksGroupedByEndDate)
            {
                Add(dateTask.Key.Date, new TaskSubitem()
                {
                    Name = "Deadline: " + String.Join(" || ", dateTask.Select(t => t.Name).ToList()),
                    Description = String.Join(" || ", dateTask.Select(t => t.Description).ToList())
                });
            }
        }
    }
}
