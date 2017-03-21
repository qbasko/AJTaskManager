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
    public class TaskItemService : BaseService, ITaskItemService
    {
        public TaskItemService()
        {
        }

        public TaskItemService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<ObservableCollection<TaskItem>> GetTaskItems(string userId)
        {
            if (await EnsureLogin())
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
            }
            return null;
        }
        public async Task<TaskItem> GetTaskItemById(string taskId)
        {
            if (await EnsureLogin())
            {
                var taskItem = await MobileService.GetTable<TaskItem>().Where(t => t.Id == taskId).ToCollectionAsync();
                return taskItem.SingleOrDefault();
            }
            return null;
        }

        public async Task<bool> InsertTaskItem(TaskItem taskItem, string userId)
        {
            if (await EnsureLogin())
            {
                if (String.IsNullOrWhiteSpace(taskItem.GroupId))
                {
                    var userGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
                    var defaultUserGroup = userGroups.Single(ug => ug.UserId == userId && ug.IsUserDefaultGroup);
                    taskItem.GroupId = defaultUserGroup.Id;
                }
                await MobileService.GetTable<TaskItem>().InsertAsync(taskItem);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateTaskItem(TaskItem taskItem)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<TaskItem>();
                await table.UpdateAsync(taskItem);
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteTaskItem(TaskItem taskItem)
        {
            if (await EnsureLogin())
            {
                //TODO delete all TaskSubItems
                var taskSubItemDataService = new TaskSubitemService(base.AccessToken);
                var taskSubItems = await taskSubItemDataService.GetTaskSubitems(taskItem.Id);
                taskSubItems.ForEach(async t => await taskSubItemDataService.DeleteTaskSubitem(t));
                await MobileService.GetTable<TaskItem>().DeleteAsync(taskItem);
                return true;
            }
            return false;
        }
        public async Task<ObservableCollection<TaskItem>> GetTaskItemsTableForGroup(string groupId)
        {
            if (await EnsureLogin())
            {
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToCollectionAsync();
                var userGroups = allUserGroups.Where(ug => ug.GroupId == groupId);
                List<string> groupsIds = userGroups.Select(ug => ug.GroupId).ToList();

                var taskItems = await MobileService.GetTable<TaskItem>().ToCollectionAsync();
                var items = taskItems.Where(t => groupsIds.Contains(t.GroupId) && !t.IsDeleted);
                return items.ToObservableCollection();
            }
            return null;
        }

        public async Task<bool> DeleteTaskItemsForGroup(string groupId)
        {
            if (await EnsureLogin())
            {
                var taskItemsForGroup = await GetTaskItemsTableForGroup(groupId);
                var taskItemsTable = MobileService.GetTable<TaskItem>();
                taskItemsForGroup.ForEach(async t => taskItemsTable.DeleteAsync(t));
                return true;
            }
            return false;
        }

        public async Task<ObservableCollection<DTO.TaskStatus>> GetAvailableTaskStatuses()
        {
            if (await EnsureLogin())
                return await MobileService.GetTable<DTO.TaskStatus>().ToCollectionAsync();
            return null;
        }

        public async Task<DTO.TaskStatus> GetTaskStatusEnumById(string statusId)
        {
            if (await EnsureLogin())
            {
                var
                    status =
                        await
                            MobileService.GetTable<DTO.TaskStatus>()
                                .Where(t => t.Id == statusId)
                                .ToCollectionAsync();
                return status.Single();
            }
            return null;
        }

        public new string AccessToken
        {
            get { return base.AccessToken; }
            set { base.AccessToken = value; }
        }
    }
}
