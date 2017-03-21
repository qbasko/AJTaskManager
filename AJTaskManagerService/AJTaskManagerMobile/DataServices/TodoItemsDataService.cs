using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.DataServices
{
    public class TodoItemsDataService : DataServiceBase, ITodoItemsDataService
    {
        public async Task<ObservableCollection<TodoItem>> GetTodoListsItems(string todoListId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var todoItems =
                    await
                        MobileService.GetTable<TodoItem>()
                            .Where(i => i.TodoListId == todoListId && !i.IsDeleted)
                            .ToCollectionAsync();
                return todoItems;
            });
        }

        public async Task<bool> DeleteTodoItems(IEnumerable<TodoItem> todoItems)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<TodoItem>();
                foreach (var todoItem in todoItems)
                {
                    await table.DeleteAsync(todoItem);
                }
                return true;
            });
        }

        public async Task<bool> DeleteTodoItem(TodoItem todoItem)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<TodoItem>();
                await table.DeleteAsync(todoItem);
                return true;
            });
        }

        public async Task<bool> UpdateTodoItem(TodoItem todoItem)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<TodoItem>();
                await table.UpdateAsync(todoItem);
                return true;
            });
        }
    }
}
