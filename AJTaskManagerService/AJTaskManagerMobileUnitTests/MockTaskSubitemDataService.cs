using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobileUnitTests
{
    class MockTaskSubitemDataService : ITaskSubitemDataService
    {
        private ObservableCollection<TaskSubitem> _taskItems;

        public MockTaskSubitemDataService()
        {
            _taskItems = new ObservableCollection<TaskSubitem>();
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskSubitem>> GetTaskSubitems(string taskItemId)
        {
            return await Task.Factory.StartNew(() => _taskItems);
        }

        public Task<AJTaskManagerMobile.Model.DTO.TaskSubitem> GetTaskSubitemById(string taskSubitemId)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskSubitem>> GetUserTaskSubitemsAlreadyStarted(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskSubitem>> GetUserTaskSubitemsNearDeadlines(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertTaskSubitem(AJTaskManagerMobile.Model.DTO.TaskSubitem taskSubitem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTaskSubitem(AJTaskManagerMobile.Model.DTO.TaskSubitem taskSubItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTaskSubitem(AJTaskManagerMobile.Model.DTO.TaskSubitem taskSubItem)
        {
            throw new NotImplementedException();
        }
    }
}
