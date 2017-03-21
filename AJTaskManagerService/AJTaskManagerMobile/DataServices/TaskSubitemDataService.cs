using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;
using Syncfusion.UI.Xaml.Controls;

namespace AJTaskManagerMobile.DataServices
{
    public class TaskSubitemDataService : DataServiceBase, ITaskSubitemDataService
    {
        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.DTO.TaskSubitem>> GetTaskSubitems(string taskItemId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var taskSubitems =
                    await
                        MobileService.GetTable<TaskSubitem>().Where(t => t.TaskItemId == taskItemId && !t.IsDeleted).ToCollectionAsync();
                return taskSubitems;
            });
        }


        public async Task<TaskSubitem> GetTaskSubitemById(string taskSubitemId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var taskSubitem =
                    await MobileService.GetTable<TaskSubitem>().Where(t => t.Id == taskSubitemId).ToCollectionAsync();
                return taskSubitem.SingleOrDefault();
            });
        }

        public async Task<bool> DeleteTaskSubitem(Model.DTO.TaskSubitem taskSubItem)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<TaskSubitem>().DeleteAsync(taskSubItem);
                return true;
            });
        }

        public async Task<bool> UpdateTaskSubitem(Model.DTO.TaskSubitem taskSubItem)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<TaskSubitem>().UpdateAsync(taskSubItem);
                return true;
            });
        }

        public async Task<bool> InsertTaskSubitem(Model.DTO.TaskSubitem taskSubitem)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<TaskSubitem>().InsertAsync(taskSubitem);
                return true;
            });
        }


        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitem>> GetUserTaskSubitemsAlreadyStarted(string userId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var taskService = SimpleIoc.Default.GetInstance<ITaskItemDataService>();
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
            });
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitem>> GetUserTaskSubitemsNearDeadlines(string userId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var taskService = SimpleIoc.Default.GetInstance<ITaskItemDataService>();
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
            });
        }


    }
}
