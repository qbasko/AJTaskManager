using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTO;
using WebApplication1.Services;

namespace AJTaskManagerWebUnitTests
{
    class MockToDoListService : IToDoListService
    {
        public string AccessToken { get; set; }
        public Task<System.Collections.ObjectModel.ObservableCollection<WebApplication1.DTO.Group>> GetGroupForUser(string externalUserId)
        {
            throw new NotImplementedException();
        }

        public Task<WebApplication1.DTO.Group> GetGroupForUserByGroupId(string externalUserId, string groupId)
        {
            throw new NotImplementedException();
        }
        
        public Task InsertTodoList(WebApplication1.DTO.ToDoList todoList, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<WebApplication1.DTO.ToDoList>> GetUserLists(string userId)
        {
            var result = new ObservableCollection<ToDoList>();
            result.Add(new ToDoList() { Id = "1", GroupId = "G1", IsCompleted = false, IsDeleted = false, ListName = "List1" });
            result.Add(new ToDoList() { Id = "2", GroupId = "G1", IsCompleted = false, IsDeleted = false, ListName = "List2" });
            result.Add(new ToDoList() { Id = "3", GroupId = "G2", IsCompleted = false, IsDeleted = false, ListName = "List3" });
            result.Add(new ToDoList() { Id = "4", GroupId = "G2", IsCompleted = false, IsDeleted = false, ListName = "List4" });
            result.Add(new ToDoList() { Id = "5", GroupId = "G3", IsCompleted = false, IsDeleted = false, ListName = "List5" });

            return new Task<ObservableCollection<ToDoList>>(() => result);
        }

        public Task<WebApplication1.DTO.ToDoList> GetUserListById(string listId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<WebApplication1.DTO.ToDoList> GetListById(string listId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateTodoList(WebApplication1.DTO.ToDoList toDoList)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTodoList(WebApplication1.DTO.ToDoList list)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<WebApplication1.DTO.ToDoList>> GetTodoListsTableForGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTodoListsForGroup(string groupId)
        {
            throw new NotImplementedException();
        }
    }
}
