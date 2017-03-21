using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AJTaskManagerMobileUnitTests
{
    [TestClass]
    public class TasksListTests
    {
        [TestMethod]
        public async Task GetAllTaskItemsTest()
        {
            var mockUserDataService = new MockUserDataService();
            var mockRoleTypeDataService = new MockRoleTypeDataService();
            var mockNavigationService = new MockNavigationService();
            var mockTaskItemDataService = new MockTaskItemDataService();
            var mockTaskSubitemDataServcice = new MockTaskSubitemDataService();

            var tasksListViewModel = new TasksListViewModel(mockNavigationService, mockTaskItemDataService,
                mockUserDataService, mockRoleTypeDataService, mockTaskSubitemDataServcice);
            await tasksListViewModel.Refresh();
            Assert.AreEqual(4, tasksListViewModel.TaskItems.Count);
        }

        [TestMethod]
        public async Task UserCantDeleteTaskTest()
        {
            var mockUserDataService = new MockUserDataService();
            var mockRoleTypeDataService = new MockRoleTypeDataServiceNegative();
            var mockNavigationService = new MockNavigationService();
            var mockTaskItemDataService = new MockTaskItemDataService();
            var mockTaskSubitemDataServcice = new MockTaskSubitemDataService();

            var tasksListViewModel = new TasksListViewModel(mockNavigationService, mockTaskItemDataService,
                mockUserDataService, mockRoleTypeDataService, mockTaskSubitemDataServcice);
            await tasksListViewModel.Refresh();
            int totalTaskItems = tasksListViewModel.TaskItems.Count;
            var taskItem = tasksListViewModel.TaskItems.First();
            taskItem.IsCompleted = true;
            await Task.Factory.StartNew(() => tasksListViewModel.DeleteCompletedCommand.Execute(null));
            await Task.Delay(TimeSpan.FromSeconds(15));
            Assert.AreEqual(totalTaskItems, tasksListViewModel.TaskItems.Count);
        }

        [TestMethod]
        public async Task UserCanDeleteTaskTest()
        {
            var mockUserDataService = new MockUserDataService();
            var mockRoleTypeDataService = new MockRoleTypeDataService();
            var mockNavigationService = new MockNavigationService();
            var mockTaskItemDataService = new MockTaskItemDataService();
            var mockTaskSubitemDataServcice = new MockTaskSubitemDataService();

            var tasksListViewModel = new TasksListViewModel(mockNavigationService, mockTaskItemDataService,
                mockUserDataService, mockRoleTypeDataService, mockTaskSubitemDataServcice);
            await tasksListViewModel.Refresh();
            int totalTaskItems = tasksListViewModel.TaskItems.Count;
            var taskItem = tasksListViewModel.TaskItems.First();
            taskItem.IsCompleted = true;
            await Task.Factory.StartNew(() => tasksListViewModel.DeleteCompletedCommand.Execute(null));
            await Task.Delay(TimeSpan.FromSeconds(15));
            Assert.AreEqual(totalTaskItems - 1, tasksListViewModel.TaskItems.Count);
        }
    }
}
