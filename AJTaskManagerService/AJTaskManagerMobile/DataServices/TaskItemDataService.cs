using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.DataServices
{
    public class TaskItemDataService : DataServiceBase, ITaskItemDataService
    {
        public async Task<ObservableCollection<TaskItem>> GetTaskItems(string userId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var userGroups =
                    await MobileService.GetTable<UserGroup>().Where(ug => ug.UserId == userId).ToCollectionAsync();
                List<string> groupsId = userGroups.Select(ug => ug.GroupId).ToList();

                var items =
                    await
                        MobileService.GetTable<TaskItem>()
                            .Where(i => groupsId.Contains(i.GroupId) && !i.IsDeleted)
                            .ToCollectionAsync();
                return items.ToObservableCollection();
            });
        }
        public async Task<TaskItem> GetTaskItemById(string taskId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var taskItem = await MobileService.GetTable<TaskItem>().Where(t => t.Id == taskId).ToCollectionAsync();
                return taskItem.SingleOrDefault();
            });
        }

        public async Task<bool> InsertTaskItem(TaskItem taskItem, string userId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                if (String.IsNullOrWhiteSpace(taskItem.GroupId))
                {
                    var userGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
                    var defaultUserGroup = userGroups.Single(ug => ug.UserId == userId && ug.IsUserDefaultGroup);
                    taskItem.GroupId = defaultUserGroup.Id;
                }
                await MobileService.GetTable<TaskItem>().InsertAsync(taskItem);
                return true;
            });
        }

        public async Task<bool> UpdateTaskItem(TaskItem taskItem)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<TaskItem>();
                await table.UpdateAsync(taskItem);
                return true;
            });
        }

        public async Task<bool> DeleteTaskItem(TaskItem taskItem)
        {
            return await ExecuteAuthenticated(async () =>
            {
                //TODO delete all TaskSubItems
                var taskSubItemDataService = SimpleIoc.Default.GetInstance<ITaskSubitemDataService>();
                var taskSubItems = await taskSubItemDataService.GetTaskSubitems(taskItem.Id);
                taskSubItems.ForEach(async t => await taskSubItemDataService.DeleteTaskSubitem(t));
                await MobileService.GetTable<TaskItem>().DeleteAsync(taskItem);
                return true;
            });
        }
        public async Task<ObservableCollection<TaskItem>> GetTaskItemsTableForGroup(string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToCollectionAsync();
                var userGroups = allUserGroups.Where(ug => ug.GroupId == groupId);
                List<string> groupsIds = userGroups.Select(ug => ug.GroupId).ToList();

                var taskItems = await MobileService.GetTable<TaskItem>().ToCollectionAsync();
                var items = taskItems.Where(t => groupsIds.Contains(t.GroupId) && !t.IsDeleted);
                return items.ToObservableCollection();
            });
        }

        public async Task<bool> DeleteTaskItemsForGroup(string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var taskItemsForGroup = await GetTaskItemsTableForGroup(groupId);
                var taskItemsTable = MobileService.GetTable<TaskItem>();
                taskItemsForGroup.ForEach(async t => taskItemsTable.DeleteAsync(t));
                return true;
            });
        }

        public async Task<ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskStatus>> GetAvailableTaskStatuses()
        {
            return
                await ExecuteAuthenticated(async () => await MobileService.GetTable<AJTaskManagerMobile.Model.DTO.TaskStatus>().ToCollectionAsync());
        }

        public async Task<Model.DTO.TaskStatus> GetTaskStatusEnumById(string statusId)
        {
            return
                await
                    ExecuteAuthenticated(
                        async () =>
                        {
                            var
                                status =
                                    await
                                        MobileService.GetTable<Model.DTO.TaskStatus>()
                                            .Where(t => t.Id == statusId)
                                            .ToCollectionAsync();
                            return status.Single();
                        });
        }
    }
}
