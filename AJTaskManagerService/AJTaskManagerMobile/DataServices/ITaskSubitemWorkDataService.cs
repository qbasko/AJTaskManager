using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.DataServices
{
    public interface ITaskSubitemWorkDataService
    {
        Task<ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorks(string taskSubitemId);
        Task<ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorksWithUsers(string taskSubitemId);

        Task<ObservableCollection<TaskSubitemWork>> GetTaskSubitemWorksInProgress(List<string> groupTaskSubitemIds);

        Task<bool> Insert(TaskSubitemWork taskSubitemWork);
        Task<bool> Update(TaskSubitemWork taskSubitemWork);
        Task<bool> Delete(TaskSubitemWork taskSubitemWork);
    }
}
