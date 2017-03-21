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
    class MockTodoListDataService : ITodoListDataService
    {
        private ObservableCollection<TodoList> _todoLists;

        public MockTodoListDataService()
        {
            _todoLists = new ObservableCollection<TodoList>();
            _todoLists.Add(new TodoList() { Id = "0", GroupId = "test group1", IsCompleted = false, IsDeleted = false, ListName = "test list1" });
            _todoLists.Add(new TodoList() { Id = "1", GroupId = "test group2", IsCompleted = false, IsDeleted = false, ListName = "test list2" });
            _todoLists.Add(new TodoList() { Id = "2", GroupId = "test group3", IsCompleted = false, IsDeleted = false, ListName = "test list3" }); 
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TodoList>> GetTodoListsTable(string userId)
        {
            return await Task.Factory.StartNew(() => _todoLists);
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.TodoList>> GetTodoListsTableForGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTodoListsForGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> InsertTodoList(AJTaskManagerMobile.Model.DTO.TodoList todoList, string userId)
        {
           _todoLists.Add(todoList);
           return await Task.Factory.StartNew(() => true);
        }

        public Task<bool> UpdateTodoList(AJTaskManagerMobile.Model.DTO.TodoList list)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTodoList(AJTaskManagerMobile.Model.DTO.TodoList list)
        {
            throw new NotImplementedException();
        }
    }
}
