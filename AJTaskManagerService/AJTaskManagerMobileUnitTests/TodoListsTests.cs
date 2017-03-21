using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;
using AJTaskManagerMobile.ViewModel;
using GalaSoft.MvvmLight.Views;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace AJTaskManagerMobileUnitTests
{
    [TestClass]
    public class TodoListsTests
    {
        [TestMethod]
        public void GetAllTodoListsTest()
        {
            var mockTodoListDataService = new MockTodoListDataService();
            var mockUserDataService = new MockUserDataService();
            var mockGroupDataService = new MockGroupDataService();
            var mockRoleTypeDataService = new MockRoleTypeDataService();
            var mockNavigationService = new MockNavigationService();
            var todoListViewModel = new ToDoListsViewModel(mockTodoListDataService, mockUserDataService,
                mockGroupDataService, mockNavigationService, mockRoleTypeDataService);
            Assert.AreEqual(3, todoListViewModel.TodoLists.Count);
        }

        [TestMethod]
        public async Task UserCantAddListTest()
        {
            var mockTodoListDataService = new MockTodoListDataService();
            var mockUserDataService = new MockUserDataService();
            var mockGroupDataService = new MockGroupDataService();
            var mockRoleTypeDataService = new MockRoleTypeDataServiceNegative();
            var mockNavigationService = new MockNavigationService();
            var todoListViewModel = new ToDoListsViewModel(mockTodoListDataService, mockUserDataService,
                mockGroupDataService, mockNavigationService, mockRoleTypeDataService);
            todoListViewModel.ListTitle = "test list4";
            Task.Factory.StartNew(() => todoListViewModel.AddNewItemCommand.Execute(null)).Wait();
            Assert.AreEqual(3, todoListViewModel.TodoLists.Count);
        }

        [TestMethod]
        public async Task UserCanAddListTest()
        {
            var mockTodoListDataService = new MockTodoListDataService();
            var mockUserDataService = new MockUserDataService();
            var mockGroupDataService = new MockGroupDataService();
            var mockRoleTypeDataService = new MockRoleTypeDataService();
            var mockNavigationService = new MockNavigationService();
            var todoListViewModel = new ToDoListsViewModel(mockTodoListDataService, mockUserDataService,
                mockGroupDataService, mockNavigationService, mockRoleTypeDataService);
            todoListViewModel.ListTitle = "test list4";
            await Task.Factory.StartNew(() => todoListViewModel.AddNewItemCommand.Execute(null));
            await Task.Delay(TimeSpan.FromSeconds(15));
            Assert.AreEqual(4, todoListViewModel.TodoLists.Count);
        }




        private ObservableCollection<TodoList> GetTodoLists()
        {
            var result = new ObservableCollection<TodoList>();
            result.Add(new TodoList() { Id = "0", GroupId = "test group1", IsCompleted = false, IsDeleted = false, ListName = "test list1" });
            result.Add(new TodoList() { Id = "1", GroupId = "test group2", IsCompleted = false, IsDeleted = false, ListName = "test list2" });
            result.Add(new TodoList() { Id = "2", GroupId = "test group3", IsCompleted = false, IsDeleted = false, ListName = "test list3" });
            return result;
        }

    }
}
