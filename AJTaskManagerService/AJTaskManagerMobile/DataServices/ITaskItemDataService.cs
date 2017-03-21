using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.DataServices
{
    public interface ITaskItemDataService
    {
        Task<ObservableCollection<TaskItem>> GetTaskItems(string userId);
        Task<TaskItem> GetTaskItemById(string taskId);

        Task<ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskStatus>> GetAvailableTaskStatuses();

        Task<bool> InsertTaskItem(TaskItem taskItem, string userId);

        Task<bool> UpdateTaskItem(TaskItem taskItem);
        Task<bool> DeleteTaskItem(TaskItem taskItem);

        Task<ObservableCollection<TaskItem>> GetTaskItemsTableForGroup(string groupId);
        Task<bool> DeleteTaskItemsForGroup(string groupId);

        Task<Model.DTO.TaskStatus> GetTaskStatusEnumById(string statusId);
    }
}
