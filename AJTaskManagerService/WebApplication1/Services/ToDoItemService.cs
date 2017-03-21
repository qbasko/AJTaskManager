using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class ToDoItemService : BaseService
    {
        public ToDoItemService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task InsertTodoItem(ToDoItem toDoItem)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<ToDoItem>().InsertAsync(toDoItem);
            }
        
        }

        public async Task<ToDoItem> GetItemInfoByItemId(string itemId)
        {
            if (await EnsureLogin())
            {
                var items = await MobileService.GetTable<ToDoItem>().Where(i => i.Id == itemId).ToCollectionAsync();
                var item = items.Single();

                return item;
            }
            return null;
        }

        public async Task<ObservableCollection<ToDoItem>> GetTodoListsItems(string todoListId)
        {
            if (await EnsureLogin())
            {
                var todoItems =
                    await
                        MobileService.GetTable<ToDoItem>()
                            .Where(i => i.TodoListId == todoListId && !i.IsDeleted)
                            .ToCollectionAsync();
                return todoItems;
            }
            return null;
        }

        public async Task<bool> DeleteTodoItems(IEnumerable<ToDoItem> todoItems)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<ToDoItem>();
                foreach (var todoItem in todoItems)
                {
                    await table.DeleteAsync(todoItem);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteTodoItem(ToDoItem todoItem)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<ToDoItem>();
                await table.DeleteAsync(todoItem);
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateTodoItem(ToDoItem todoItem)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<ToDoItem>();
                await table.UpdateAsync(todoItem);
                return true;
            }
            return false;
        }
    }
}
