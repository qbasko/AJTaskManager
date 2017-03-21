using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobileUnitTests
{
    class MockTaskItemDataService : ITaskItemDataService
    {
        private ObservableCollection<TaskItem> _taskItems;

        public MockTaskItemDataService()
        {
            _taskItems = new ObservableCollection<TaskItem>();
            _taskItems.Add(new TaskItem()
            {
                Id = "1",
                Name = "TaskItem1",
                GroupId = "Group1",
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddDays(2),
                TaskStatusId = "1"
            });
            _taskItems.Add(new TaskItem()
            {
                Id = "2",
                Name = "TaskItem2",
                GroupId = "Group1",
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddDays(3),
                TaskStatusId = "2"
            });
            _taskItems.Add(new TaskItem()
            {
                Id = "3",
                Name = "TaskItem3",
                GroupId = "Group2",
                StartDateTime = DateTime.Today,
                EndDateTime = DateTime.Today.AddDays(1),
                TaskStatusId = "1"
            });
            _taskItems.Add(new TaskItem()
            {
                Id = "4",
                Name = "TaskItem4",
                GroupId = "Group2",
                StartDateTime = DateTime.Today.AddDays(-5),
                EndDateTime = DateTime.Today.AddDays(2),
                TaskStatusId = "2"
            });

        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskItem>> GetTaskItems(string userId)
        {
            var items = new ObservableCollection<TaskItem>();
            var notDeleted = _taskItems.Where(i => !i.IsDeleted);
            foreach (var taskItem in notDeleted)
            {
                items.Add(taskItem);
            }
            return await Task.Factory.StartNew(() => items);
        }

        public Task<AJTaskManagerMobile.Model.DTO.TaskItem> GetTaskItemById(string taskId)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskStatus>> GetAvailableTaskStatuses()
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertTaskItem(AJTaskManagerMobile.Model.DTO.TaskItem taskItem, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateTaskItem(AJTaskManagerMobile.Model.DTO.TaskItem taskItem)
        {
            var task = _taskItems.Single(t => t.Id == taskItem.Id);
            task.IsDeleted = true;
            return await Task.Factory.StartNew(() => true);
        }

        public Task<bool> DeleteTaskItem(AJTaskManagerMobile.Model.DTO.TaskItem taskItem)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TaskItem>> GetTaskItemsTableForGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTaskItemsForGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public Task<AJTaskManagerMobile.Model.DTO.TaskStatus> GetTaskStatusEnumById(string statusId)
        {
            throw new NotImplementedException();
        }
    }
}
