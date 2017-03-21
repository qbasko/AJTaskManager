using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.DataServices
{
    public interface ITaskSubitemDataService
    {
        Task<ObservableCollection<TaskSubitem>> GetTaskSubitems(string taskItemId);

        Task<TaskSubitem> GetTaskSubitemById(string taskSubitemId);
        Task<ObservableCollection<TaskSubitem>> GetUserTaskSubitemsAlreadyStarted(string userId);
        Task<ObservableCollection<TaskSubitem>> GetUserTaskSubitemsNearDeadlines(string userId);
        
        Task<bool> InsertTaskSubitem(TaskSubitem taskSubitem);
        Task<bool> UpdateTaskSubitem(TaskSubitem taskSubItem);
        Task<bool> DeleteTaskSubitem(TaskSubitem taskSubItem);
    }
}
