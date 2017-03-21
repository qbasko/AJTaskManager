using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.Data.Extensions;
using WebApplication1.Common;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface ITaskSubitemService :IService
    {

        Task<System.Collections.ObjectModel.ObservableCollection<DTO.TaskSubitem>> GetTaskSubitems(string taskItemId);

        Task<TaskSubitem> GetTaskSubitemById(string taskSubitemId);


        Task<bool> DeleteTaskSubitem(TaskSubitem taskSubItem);


        Task<bool> UpdateTaskSubitem(TaskSubitem taskSubItem);

        Task<bool> InsertTaskSubitem(TaskSubitem taskSubitem);

        Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitem>> GetUserTaskSubitemsAlreadyStarted(string userId);

        Task<System.Collections.ObjectModel.ObservableCollection<TaskSubitem>> GetUserTaskSubitemsNearDeadlines(
            string userId);

        Task<ObservableCollection<DTO.TaskStatus>> GetAvailableTaskStatuses();


        Task<DTO.TaskStatus> GetTaskStatusEnumById(string statusId);

        Task<string> GetTaskSubitemNameByTaskSubitemId(string taskSubitemId);


        Task<ObservableCollection<TaskSubitem>> GetNotCompletedTaskSubitems(string userId, DateTime fromDate,
            DateTime toDate);


        Task<ObservableCollection<TaskSubitem>> GetCompletedTaskSubitems(string userId, DateTime fromDate,
            DateTime toDate);


    }
}
