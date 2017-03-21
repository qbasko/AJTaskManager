using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplication1.Controllers;
using System.Threading.Tasks;
using Moq;
using WebApplication1.Common;
using WebApplication1.Services;
using Microsoft.Practices.Unity;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace AJTaskManagerWebUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TodoListIndexTest()
        {
            var mockServiceProvider = new MockServiceProvider();
            var mockUserService = new Mock<IUserService>();
            var mockGroupService = new Mock<IGroupService>();
            var mockTodoListService = new Mock<IToDoListService>();

            mockUserService.Setup(x => x.GetUserId(It.IsAny<string>(), It.IsAny<UserDomainEnum>()))
                .ReturnsTask<IUserService, string, UserDomainEnum, string>((x, y) => "1234");

            mockTodoListService.Setup(x => x.GetUserLists(It.IsAny<string>()))
                .ReturnsTask<IToDoListService, string, ObservableCollection<ToDoList>>(x => GetTodoLists());

            mockGroupService.Setup(x => x.GroupNameByGroupId(It.IsAny<string>()))
                .ReturnsTask<IGroupService, string, string>(GroupNameByGroupId);

            mockServiceProvider.Container.RegisterInstance(typeof(IUserService), mockUserService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(IGroupService), mockGroupService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(IToDoListService), mockTodoListService.Object);

            var controller = new AddTodoListController(mockServiceProvider);
            var result = (await controller.Index("", "")) as ViewResult;
            Assert.AreEqual(5, ((List<AddTodoListModel>)result.Model).Count);

        }
        [TestMethod]
        public async Task TodoListIndexSortByNameTest()
        {//
            var mockServiceProvider = new MockServiceProvider();
            var mockUserService = new Mock<IUserService>();
            var mockGroupService = new Mock<IGroupService>();
            var mockTodoListService = new Mock<IToDoListService>();

            mockUserService.Setup(x => x.GetUserId(It.IsAny<string>(), It.IsAny<UserDomainEnum>()))
                .ReturnsTask<IUserService, string, UserDomainEnum, string>((x, y) => "1234");

            mockTodoListService.Setup(x => x.GetUserLists(It.IsAny<string>()))
                .ReturnsTask<IToDoListService, string, ObservableCollection<ToDoList>>(x => GetTodoLists());

            mockGroupService.Setup(x => x.GroupNameByGroupId(It.IsAny<string>()))
                .ReturnsTask<IGroupService, string, string>(GroupNameByGroupId);

            mockServiceProvider.Container.RegisterInstance(typeof(IUserService), mockUserService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(IGroupService), mockGroupService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(IToDoListService), mockTodoListService.Object);

            var controller = new AddTodoListController(mockServiceProvider);
            var result = (await controller.Index("listName_desc", "")) as ViewResult;
            Assert.AreEqual(GetTodoLists().Last().ListName, ((List<AddTodoListModel>)result.Model).First().ListName);

        }
        [TestMethod]
        public async Task TodoListIndexFilterTest()
        {
            var mockServiceProvider = new MockServiceProvider();
            var mockUserService = new Mock<IUserService>();
            var mockGroupService = new Mock<IGroupService>();
            var mockTodoListService = new Mock<IToDoListService>();

            mockUserService.Setup(x => x.GetUserId(It.IsAny<string>(), It.IsAny<UserDomainEnum>()))
                .ReturnsTask<IUserService, string, UserDomainEnum, string>((x, y) => "1234");

            mockTodoListService.Setup(x => x.GetUserLists(It.IsAny<string>()))
                .ReturnsTask<IToDoListService, string, ObservableCollection<ToDoList>>(x => GetTodoLists());

            mockGroupService.Setup(x => x.GroupNameByGroupId(It.IsAny<string>()))
                .ReturnsTask<IGroupService, string, string>(GroupNameByGroupId);

            mockServiceProvider.Container.RegisterInstance(typeof(IUserService), mockUserService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(IGroupService), mockGroupService.Object);
            mockServiceProvider.Container.RegisterInstance(typeof(IToDoListService), mockTodoListService.Object);


            string filter = "List3";
            var controller = new AddTodoListController(mockServiceProvider);
            var result = (await controller.Index("", filter)) as ViewResult;
            Assert.AreEqual(filter, ((List<AddTodoListModel>)result.Model).Single().ListName);
        }


        private ObservableCollection<ToDoList> GetTodoLists()
        {
            var result = new ObservableCollection<ToDoList>();

            result.Add(new ToDoList() { Id = "1", GroupId = "G1", IsCompleted = false, IsDeleted = false, ListName = "List1" });
            result.Add(new ToDoList() { Id = "2", GroupId = "G1", IsCompleted = false, IsDeleted = false, ListName = "List2" });
            result.Add(new ToDoList() { Id = "3", GroupId = "G2", IsCompleted = false, IsDeleted = false, ListName = "List3" });
            result.Add(new ToDoList() { Id = "4", GroupId = "G2", IsCompleted = false, IsDeleted = false, ListName = "List4" });
            result.Add(new ToDoList() { Id = "5", GroupId = "G3", IsCompleted = false, IsDeleted = false, ListName = "List5" });
            return result;
        }

        public string GroupNameByGroupId(string id)
        {
            switch (id)
            {
                case "G1": return "Group1";
                case "G2":
                    return "Group2";
                case "G3":
                    return "Group3";
                default:
                    return "";
            }

        }
    }
}
