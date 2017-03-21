using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;
using WebApplication1.Common;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class TaskSubitemService : BaseService, ITaskSubitemService
    {
        public TaskSubitemService()
        {
        }

        public TaskSubitemService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<DTO.TaskSubitem>> GetTaskSubitems(string taskItemId)
        {
            if (await EnsureLogin())
            {
                var taskSubitems =
                    await
                        MobileService.GetTable<TaskSubitem>().Where(t => t.TaskItemId == taskItemId && !t.IsDeleted).ToCollectionAsync();
                return taskSubitems;
            }
            return null;
        }


        public async Task<TaskSubitem> GetTaskSubitemById(string taskSubitemId)
        {
            if (await EnsureLogin())
            {
                var taskSubitem =
                    await MobileService.GetTable<TaskSubitem>().Where(t => t.Id == taskSubitemId).ToCollectionAsync();
                return taskSubitem.SingleOrDefault();
            }
            return null;
        }

        public async Task<bool> DeleteTaskSubitem(TaskSubitem taskSubItem)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<TaskSubitem>().DeleteAsync(taskSubItem);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateTaskSubitem(TaskSubitem taskSubItem)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<TaskSubitem>();
                await table.UpdateAsync(taskSubItem);
                return true;
            }
            return false;
        }

        public async Task<bool> InsertTaskSubitem(TaskSubitem taskSubitem)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<TaskSubitem>().InsertAsync(taskSubitem);
                return true;
            }
            return false;
        }


        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitem>> GetUserTaskSubitemsAlreadyStarted(string userId)
        {
            if (await EnsureLogin())
            {
                var taskService = new TaskItemService(base.AccessToken);
                var taskItems = await taskService.GetTaskItems(userId);
                var result = new List<TaskSubitem>();
                foreach (var taskItem in taskItems)
                {
                    var taskSubitems =
                        await
                            MobileService.GetTable<TaskSubitem>()
                                .Where(t => t.TaskItemId == taskItem.Id && t.TaskStatusId != ((int)TaskStatusEnum.Completed).ToString() && t.TaskStatusId != ((int)TaskStatusEnum.Rejected).ToString())
                                .ToCollectionAsync();

                    Func<TaskSubitem, bool> func = t => t.ExecutorId == userId && t.StartDateTime.HasValue && t.StartDateTime.Value.Date <= DateTime.Today &&
                                                     t.StartDateTime.Value.Date > DateTime.Today.AddDays(-2);

                    if (taskSubitems.Any(func))
                        result.AddRange(taskSubitems.Where(func));
                }
                return result.ToObservableCollection();
            }
            return null;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitem>> GetUserTaskSubitemsNearDeadlines(string userId)
        {
            if (await EnsureLogin())
            {
                var taskService = new TaskItemService(base.AccessToken);
                var taskItems = await taskService.GetTaskItems(userId);
                var result = new List<TaskSubitem>();
                foreach (var taskItem in taskItems)
                {
                    var taskSubitems =
                        await
                            MobileService.GetTable<TaskSubitem>()
                                .Where(t => t.TaskItemId == taskItem.Id && t.TaskStatusId != ((int)TaskStatusEnum.Completed).ToString() && t.TaskStatusId != ((int)TaskStatusEnum.Rejected).ToString())
                                .ToCollectionAsync();

                    Func<TaskSubitem, bool> func = t => t.ExecutorId == userId && t.EndDateTime.HasValue && t.EndDateTime.Value.Date >= DateTime.Today &&
                                                     t.EndDateTime.Value.Date < DateTime.Today.AddDays(3);

                    if (taskSubitems.Any(func))
                        result.AddRange(taskSubitems.Where(func));
                }
                return result.ToObservableCollection();
            }
            return null;
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

        public async Task<string> GetTaskSubitemNameByTaskSubitemId(string taskSubitemId)
        {

            if (await EnsureLogin())
            {
                var obj =
                    await MobileService.GetTable<TaskSubitem>().Where(i => i.Id == taskSubitemId).ToCollectionAsync();
                
                return obj.Select(n => n.Name).Single(); 
            }
            return "";
        }

        public async Task<ObservableCollection<TaskSubitem>> GetNotCompletedTaskSubitems(string userId, DateTime fromDate, DateTime toDate)
        {
            if (await EnsureLogin())
            {
                var taskService = new TaskItemService(base.AccessToken);
                var taskItems = await taskService.GetTaskItems(userId);
                var result = new List<TaskSubitem>();
                foreach (var taskItem in taskItems)
                {
                    var taskSubitems =
                        await
                            MobileService.GetTable<TaskSubitem>()
                                .Where(t => t.TaskItemId == taskItem.Id && t.TaskStatusId != ((int)TaskStatusEnum.Completed).ToString() && t.TaskStatusId != ((int)TaskStatusEnum.Rejected).ToString())
                                .ToCollectionAsync();

                    Func<TaskSubitem, bool> func = t => t.ExecutorId == userId && t.StartDateTime.HasValue && t.StartDateTime.Value.Date >= fromDate.Date &&
                                                     t.EndDateTime.HasValue && t.EndDateTime.Value.Date < toDate.Date;

                    if (taskSubitems.Any(func))
                        result.AddRange(taskSubitems.Where(func));
                }
                return result.ToObservableCollection();
            }
            return null;
        }

        public async Task<ObservableCollection<TaskSubitem>> GetCompletedTaskSubitems(string userId, DateTime fromDate, DateTime toDate)
        {
            if (await EnsureLogin())
            {
                var taskService = new TaskItemService(base.AccessToken);
                var taskItems = await taskService.GetTaskItems(userId);
                var result = new List<TaskSubitem>();
                foreach (var taskItem in taskItems)
                {
                    var taskSubitems =
                        await
                            MobileService.GetTable<TaskSubitem>()
                                .Where(t => t.TaskItemId == taskItem.Id && t.TaskStatusId == ((int)TaskStatusEnum.Completed).ToString())
                                .ToCollectionAsync();

                    Func<TaskSubitem, bool> func = t => t.ExecutorId == userId && t.StartDateTime.HasValue && t.StartDateTime.Value.Date >= fromDate.Date &&
                                                                 t.EndDateTime.HasValue && t.EndDateTime.Value.Date < toDate.Date;

                    if (taskSubitems.Any(func))
                        result.AddRange(taskSubitems.Where(func));
                }
                return result.ToObservableCollection();
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
