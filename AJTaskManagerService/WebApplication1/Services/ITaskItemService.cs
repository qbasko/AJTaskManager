using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface ITaskItemService :IService
    {

        Task<ObservableCollection<TaskItem>> GetTaskItems(string userId);

        Task<TaskItem> GetTaskItemById(string taskId);

        Task<bool> InsertTaskItem(TaskItem taskItem, string userId);

        Task<bool> UpdateTaskItem(TaskItem taskItem);


        Task<bool> DeleteTaskItem(TaskItem taskItem);

        Task<ObservableCollection<TaskItem>> GetTaskItemsTableForGroup(string groupId);


        Task<bool> DeleteTaskItemsForGroup(string groupId);

        Task<ObservableCollection<DTO.TaskStatus>> GetAvailableTaskStatuses();

        Task<DTO.TaskStatus> GetTaskStatusEnumById(string statusId);

    }
}
