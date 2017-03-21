using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class TaskSubitemWorkService : BaseService
    {
        public TaskSubitemWorkService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorks(string taskSubitemId)
        {
            if (await EnsureLogin())
            {
                var workItems =
                    await MobileService.GetTable<TaskSubitemWork>()
                        .Where(w => w.TaskSubitemId == taskSubitemId)
                        .ToCollectionAsync();
                return workItems;
            }
            return null;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorksWithUsers(string taskSubitemId)
        {
            if (await EnsureLogin())
            {
                var workItems =
                    await MobileService.GetTable<TaskSubitemWork>()
                        .Where(w => w.TaskSubitemId == taskSubitemId)
                        .ToCollectionAsync();
                var userDataService = new UserService(base.AccessToken);
                var userLoginDict = new Dictionary<string, User>();
                foreach (var item in workItems)
                {
                    if (!userLoginDict.ContainsKey(item.UserId))
                        userLoginDict.Add(item.UserId, await userDataService.GetUserById(item.UserId));
                    item.User = userLoginDict[item.UserId];
                }
                return workItems;
            }
            return null;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorksInProgress(List<string> groupTaskSubitemIds)
        {
            if (await EnsureLogin())
            {
                DateTime dt = DateTime.Now.AddDays(-2);
                var works =
                    await MobileService.GetTable<TaskSubitemWork>()
                        .Where(
                            w =>
                                groupTaskSubitemIds.Contains(w.TaskSubitemId) && w.EndDateTime == null &&
                                w.StartDateTime > dt).ToCollectionAsync();
                return works;
            }
            return null;
        }

        public async Task<bool> Insert(TaskSubitemWork taskSubitemWork)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<TaskSubitemWork>().InsertAsync(taskSubitemWork);
                return true;
            }
            return false;
        }

        public async Task<bool> Update(TaskSubitemWork taskSubitemWork)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<TaskSubitemWork>().UpdateAsync(taskSubitemWork);
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(TaskSubitemWork taskSubitemWork)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<TaskSubitemWork>().DeleteAsync(taskSubitemWork);
                return true;
            }
            return false;
        }

        public async Task<ObservableCollection<TaskSubitemWork>> GetUserTaskSubitemWorks(string userId,
            DateTime fromDate, DateTime toDate)
        {
            if (await EnsureLogin())
            {
                var taskService = new TaskItemService(base.AccessToken);
                var taskSubitemService = new TaskSubitemService(base.AccessToken);
                var taskSubitemWorksService = new TaskSubitemWorkService(base.AccessToken);

                var taskItems = await taskService.GetTaskItems(userId);
                var results = new List<TaskSubitemWork>();
                foreach (var taskItem in taskItems)
                {
                    var subItems = await taskSubitemService.GetTaskSubitems(taskItem.Id);
                    //TODO 
                    foreach (var subitem in subItems)
                    {
                        var works = await taskSubitemWorksService.GetTaskSubitemWorks(subitem.Id);
                        var worksBetweenDates =
                            works.Where(
                                w =>
                                    w.StartDateTime >= fromDate && w.EndDateTime.HasValue &&
                                    w.EndDateTime.Value < toDate);
                        if (worksBetweenDates.Any())
                            results.AddRange(worksBetweenDates);
                    }
                }

                return results.ToObservableCollection();
            }
            return null;
        }

        public async Task<ObservableCollection<TaskSubitemWork>> GetGroupTaskSubitemWorks(string groupId,
           DateTime fromDate, DateTime toDate)
        {
            if (await EnsureLogin())
            {
                var taskService = new TaskItemService(base.AccessToken);
                var taskSubitemService = new TaskSubitemService(base.AccessToken);
                var taskSubitemWorksService = new TaskSubitemWorkService(base.AccessToken);

                var taskItems = await taskService.GetTaskItemsTableForGroup(groupId);
                var results = new List<TaskSubitemWork>();
                foreach (var taskItem in taskItems)
                {
                    var subItems = await taskSubitemService.GetTaskSubitems(taskItem.Id);
                    //TODO 
                    foreach (var subitem in subItems)
                    {
                        var works = await taskSubitemWorksService.GetTaskSubitemWorks(subitem.Id);
                        var worksBetweenDates =
                            works.Where(
                                w =>
                                    w.StartDateTime >= fromDate && w.EndDateTime.HasValue &&
                                    w.EndDateTime.Value < toDate);
                        if (worksBetweenDates.Any())
                            results.AddRange(worksBetweenDates);
                    }
                }

                return results.ToObservableCollection();
            }
            return null;
        }


        public async Task<ObservableCollection<TaskSubitemWork>> GetGroupTaskSubitemWorksForUser(string userId, string groupId,
         DateTime fromDate, DateTime toDate)
        {
            if (await EnsureLogin())
            {
                var taskService = new TaskItemService(base.AccessToken);
                var taskSubitemService = new TaskSubitemService(base.AccessToken);
                var taskSubitemWorksService = new TaskSubitemWorkService(base.AccessToken);

                var taskItems = await taskService.GetTaskItemsTableForGroup(groupId);
                var results = new List<TaskSubitemWork>();
                foreach (var taskItem in taskItems)
                {
                    var subItems = await taskSubitemService.GetTaskSubitems(taskItem.Id);
                    //TODO 
                    var userTaskSubitems = subItems.Where(t => t.ExecutorId == userId);
                    foreach (var subitem in userTaskSubitems)
                    {
                        var works = await taskSubitemWorksService.GetTaskSubitemWorks(subitem.Id);
                        var worksBetweenDates =
                            works.Where(
                                w =>
                                    w.StartDateTime >= fromDate && w.EndDateTime.HasValue &&
                                    w.EndDateTime.Value < toDate);
                        if (worksBetweenDates.Any())
                            results.AddRange(worksBetweenDates);
                    }
                }

                return results.ToObservableCollection();
            }
            return null;
        }
    }
}
