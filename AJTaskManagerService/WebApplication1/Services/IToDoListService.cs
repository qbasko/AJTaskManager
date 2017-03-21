using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface IToDoListService : IService
    {
        Task<ObservableCollection<Group>> GetGroupForUser(string externalUserId);

        Task<Group> GetGroupForUserByGroupId(string externalUserId, string groupId);
        Task InsertTodoList(ToDoList todoList, string userId);
        Task<ObservableCollection<ToDoList>> GetUserLists(string userId);
        Task<ToDoList> GetUserListById(string listId, string userId);
        Task<ToDoList> GetListById(string listId);
        Task<bool> UpdateTodoList(ToDoList toDoList);
        Task<bool> DeleteTodoList(ToDoList list);
        Task<ObservableCollection<ToDoList>> GetTodoListsTableForGroup(string groupId);
        Task<bool> DeleteTodoListsForGroup(string groupId);
    }
}
