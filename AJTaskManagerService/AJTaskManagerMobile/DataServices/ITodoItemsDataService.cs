using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.DataServices
{
    public interface ITodoItemsDataService
    {
        Task<ObservableCollection<TodoItem>> GetTodoListsItems(string todoListId);
        Task<bool> DeleteTodoItems(IEnumerable<TodoItem> todoItems);
        Task<bool> UpdateTodoItem(TodoItem todoItem);
        Task<bool> DeleteTodoItem(TodoItem todoItem);
    }
}
