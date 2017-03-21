using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.DataServices
{
    public class TodoListDataService : DataServiceBase, ITodoListDataService
    {
        public async Task<ObservableCollection<TodoList>> GetTodoListsTable(string userId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToCollectionAsync();
                var userGroups = allUserGroups.Where(ug => ug.UserId == userId);
                List<string> groupsId = userGroups.Select(ug => ug.GroupId).ToList();

                var table = await MobileService.GetTable<TodoList>().ToCollectionAsync();
                var items = table.Where(l => groupsId.Contains(l.GroupId) && !l.IsDeleted);
                return items.ToObservableCollection();
            });
        }

        public async Task<bool> InsertTodoList(TodoList todoList, string userId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                if (String.IsNullOrWhiteSpace(todoList.GroupId))
                {
                    var userGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
                    var defaultUserGroup = userGroups.Single(ug => ug.UserId == userId && ug.IsUserDefaultGroup);
                    todoList.GroupId = defaultUserGroup.GroupId;
                }
                await MobileService.GetTable<TodoList>().InsertAsync(todoList);
                return true;
            });
        }

        public async Task<bool> UpdateTodoList(TodoList list)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<TodoList>();
                await table.UpdateAsync(list);
                return true;
            });
        }


        public async Task<bool> DeleteTodoList(TodoList list)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var todoItemsDataService = SimpleIoc.Default.GetInstance<ITodoItemsDataService>();
                var todoListItems = await todoItemsDataService.GetTodoListsItems(list.Id);
                todoListItems.ForEach(async i => await todoItemsDataService.DeleteTodoItem(i));
                await MobileService.GetTable<TodoList>().DeleteAsync(list);
                return true;
            });
        }

        public async Task<ObservableCollection<TodoList>> GetTodoListsTableForGroup(string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToCollectionAsync();
                var userGroups = allUserGroups.Where(ug => ug.GroupId == groupId);
                List<string> groupsId = userGroups.Select(ug => ug.GroupId).ToList();

                var table = await MobileService.GetTable<TodoList>().ToCollectionAsync();
                var items = table.Where(l => groupsId.Contains(l.GroupId) && !l.IsDeleted);
                return items.ToObservableCollection();
            });
        }


        public async Task<bool> DeleteTodoListsForGroup(string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var todoListsForGroup = await GetTodoListsTableForGroup(groupId);
                var todoListTable = MobileService.GetTable<TodoList>();
                todoListsForGroup.ForEach(async tdl => await todoListTable.DeleteAsync(tdl));
                return true;
            });
        }
    }
}
