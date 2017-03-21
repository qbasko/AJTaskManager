using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplication1.DTO;
using WebApplication1.Services;
using TaskStatus = WebApplication1.DTO.TaskStatus;
using Microsoft.Practices.Unity;
using WebApplication1.Common;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace AJTaskManagerWebUnitTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public async Task TaskSubitemIndexTest()
        {
            var mockServiceProvider = new MockServiceProvider();
            var mockUserService = new Mock<IUserService>();
            var mockTaskSubitemService = new Mock<ITaskSubitemService>();
            var mockTaskItemService = new Mock<ITaskItemService>();

            mockUserService.Setup(x => x.GetUsersFromGroup(It.IsAny<string>()))
                .ReturnsTask<IUserService, string, ObservableCollection<User>>(GetUsersFromGroup);

            mockTaskSubitemService.Setup(x => x.GetTaskSubitems(It.IsAny<string>()))
                .ReturnsTask<ITaskSubitemService, string, ObservableCollection<TaskSubitem>>(GetTaskSubitems);

            mockTaskSubitemService.Setup(x => x.GetAvailableTaskStatuses())
                .ReturnsTask<ITaskSubitemService, ObservableCollection<TaskStatus>>(GetAvailableTaskStatuses);

            mockTaskItemService.Setup(x => x.GetTaskItemById(It.IsAny<string>()))
                .ReturnsTask<ITaskItemService, string, TaskItem>(GetTaskItemById);

            mockUserService.Setup(x => x.GetUserId(It.IsAny<string>(), It.IsAny<UserDomainEnum>()))
              .ReturnsTask<IUserService, string, UserDomainEnum, string>((x, y) => "1234");

            mockServiceProvider.Container.RegisterInstance(typeof(IUserService), mockUserService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(ITaskSubitemService), mockTaskSubitemService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof (ITaskItemService), mockTaskItemService.Object);

            var controller = new TaskSubitemController(mockServiceProvider);
            var result = (await controller.Index("", "", "")) as ViewResult;
            Assert.AreEqual(5, ((List<TaskSubitemModel>)result.Model).Count);

        }

        [TestMethod]
        public async Task TaskSubitemDetailsTest()
        {
            var mockServiceProvider = new MockServiceProvider();
            var mockUserService = new Mock<IUserService>();
            var mockTaskSubitemService = new Mock<ITaskSubitemService>();
            var mockTaskItemService = new Mock<ITaskItemService>();

            mockUserService.Setup(x => x.GetUsersFromGroup(It.IsAny<string>()))
                .ReturnsTask<IUserService, string, ObservableCollection<User>>(GetUsersFromGroup);

            mockTaskSubitemService.Setup(x => x.GetTaskSubitems(It.IsAny<string>()))
                .ReturnsTask<ITaskSubitemService, string, ObservableCollection<TaskSubitem>>(GetTaskSubitems);

            mockTaskSubitemService.Setup(x => x.GetAvailableTaskStatuses())
                .ReturnsTask<ITaskSubitemService, ObservableCollection<TaskStatus>>(GetAvailableTaskStatuses);

            mockTaskItemService.Setup(x => x.GetTaskItemById(It.IsAny<string>()))
                .ReturnsTask<ITaskItemService, string, TaskItem>(GetTaskItemById);

            mockUserService.Setup(x => x.GetUserId(It.IsAny<string>(), It.IsAny<UserDomainEnum>()))
              .ReturnsTask<IUserService, string, UserDomainEnum, string>((x, y) => "1234");

            mockTaskSubitemService.Setup(x => x.GetTaskSubitemById(It.IsAny<string>()))
                .ReturnsTask<ITaskSubitemService, string, TaskSubitem>(GetTaskSubitemById);

            mockTaskSubitemService.Setup(x => x.GetTaskSubitemNameByTaskSubitemId(It.IsAny<string>()))
                .ReturnsTask<ITaskSubitemService, string, string>(GetTaskSubitemNameByTaskSubitemId);


            mockServiceProvider.Container.RegisterInstance(typeof(IUserService), mockUserService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(ITaskSubitemService), mockTaskSubitemService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(ITaskItemService), mockTaskItemService.Object);

            var controller = new TaskSubitemController(mockServiceProvider);
            var result = (await controller.Details("1", "1")) as ViewResult;
            Assert.AreEqual("1", ((TaskSubitemModel)result.Model).Id);

        }

        private ObservableCollection<TaskSubitem> GetTaskSubitems(string taskItemId)
        {
            var result = new ObservableCollection<TaskSubitem>(); 
            result.Add(new TaskSubitem() { Id = "1", IsCompleted = false, IsDeleted = false, Name = "Task Subitem 1", Description = "Desc 1", StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddMonths(2), TaskItemId = "1", TaskStatusId = "1"});
            result.Add(new TaskSubitem() { Id = "2", IsCompleted = false, IsDeleted = false, Name = "Task Subitem 2", Description = "Desc 2", StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddMonths(1), TaskItemId = "1", TaskStatusId = "1" });
            result.Add(new TaskSubitem() { Id = "3", IsCompleted = false, IsDeleted = false, Name = "Task Subitem 3", Description = "Desc 3", StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddMonths(3), TaskItemId = "1", TaskStatusId = "1" });
            result.Add(new TaskSubitem() { Id = "4", IsCompleted = false, IsDeleted = false, Name = "Task Subitem 4", Description = "Desc 4", StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddMonths(4), TaskItemId = "1", TaskStatusId = "1" });
            result.Add(new TaskSubitem() { Id = "5", IsCompleted = false, IsDeleted = false, Name = "Task Subitem 5", Description = "Desc 5", StartDateTime = DateTime.Now, EndDateTime = DateTime.Now.AddMonths(5), TaskItemId = "1", TaskStatusId = "1" });
            return result;
        }

        private ObservableCollection<TaskStatus> GetAvailableTaskStatuses()
        {
            var result = new ObservableCollection<TaskStatus>();
            result.Add(new TaskStatus() { Id = "1",Status = "TaskStatus"});
           
            return result;
        }

        private TaskItem GetTaskItemById(string taskItemId)
        {
            return new TaskItem(){ Id = "1", Name = "1", GroupId = "1"};
        }

        private ObservableCollection<User> GetUsersFromGroup(string groupId)
        {
            var result = new ObservableCollection<User>();
            return result;
        }

        private TaskSubitem GetTaskSubitemById(string taskSubitemId)
        {
            return GetTaskSubitems("").Single(t => t.Id == taskSubitemId);
        }

        private string GetTaskSubitemNameByTaskSubitemId(string taskSubitemId)
        {
            return GetTaskSubitems("").Single(t => t.Id == taskSubitemId).Name;
        }

    }
}
