using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;
using Microsoft.WindowsAzure.MobileServices;

namespace AJTaskManagerMobile.DataServices
{
    public interface ITodoListDataService
    {
        Task<ObservableCollection<TodoList>> GetTodoListsTable(string userId);
        Task<ObservableCollection<TodoList>> GetTodoListsTableForGroup(string groupId);

        Task<bool> DeleteTodoListsForGroup(string groupId);

        Task<bool> InsertTodoList(TodoList todoList, string userId);
        Task<bool> UpdateTodoList(TodoList list);

        Task<bool> DeleteTodoList(TodoList list);
        
    }
}
