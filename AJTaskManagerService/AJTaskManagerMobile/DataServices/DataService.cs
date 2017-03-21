using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.DataServices
{
    public class DataService : DataServiceBase
    {
        //public async Task<bool> Insert(LeaderBoardItem item)
        //{
        //    // Make sure we're authenticated by passing the task into ExecuteAuthenticated
        //    return await this.ExecuteAuthenticated(async () =>
        //    {
        //        var table = _mobileService.GetTable<LeaderBoardItem>();
        //        await table.InsertAsync(item);

        //        return true;
        //    });
        //}

        //public async Task<IEnumerable<LeaderBoardItem>> GetAll()
        //{
        //    // Make sure we're authenticated by passing the task into ExecuteAuthenticated
        //    return await this.ExecuteAuthenticated(async () =>
        //    {
        //        var table = _mobileService.GetTable<LeaderBoardItem>();
        //        return await table.ToEnumerableAsync();
        //    });
        //}

        public async Task<bool> Insert(TodoItem item)
        {
            // Make sure we're authenticated by passing the task into ExecuteAuthenticated
            return await this.ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<TodoItem>();
                await table.InsertAsync(item);
                return true;
            });
        }

        public async Task<bool> Insert(User user)
        {
            // Make sure we're authenticated by passing the task into ExecuteAuthenticated
            return await this.ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<User>();
                await table.InsertAsync(user);
                return true;
            });
        }

        public async Task<bool> Test()
        {
            // Make sure we're authenticated by passing the task into ExecuteAuthenticated
            return await this.ExecuteAuthenticated(async () =>
            {


                return true;
            });
        }

        public async Task<MobileServiceCollection<TodoItem, TodoItem>> GetTodoItemsTable()
        {
            return await ExecuteAuthenticated(() =>
            {
                var table = MobileService.GetTable<TodoItem>();
                var items = table
                     .Where(todoItem => todoItem.IsCompleted == false)
                     .ToCollectionAsync();
                return items;
            });
        }

        public async Task<MobileServiceCollection<TodoItem, TodoItem>> GetUsersTodoItems(string listId)
        {
            return await ExecuteAuthenticated(() =>
            {
                var table = MobileService.GetTable<TodoItem>();
                var items = table
                     .Where(todoItem => todoItem.IsCompleted == false && todoItem.TodoListId == listId)
                     .ToCollectionAsync();
                return items;
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

        public async Task<MobileServiceCollection<User, User>> GetUsersTable()
        {
            return await ExecuteAuthenticated(() =>
            {
                var table = MobileService.GetTable<User>();
                var items = table.ToCollectionAsync();
                return items;
            }
                );
        }
    }
}
