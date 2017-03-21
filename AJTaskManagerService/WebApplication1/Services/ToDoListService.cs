using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class ToDoListService : BaseService, IToDoListService
    {
        public new string AccessToken
        {
            get { return base.AccessToken; }
            set { base.AccessToken = value; }
        }

        public ToDoListService() { }
        public ToDoListService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<ObservableCollection<Group>> GetGroupForUser(string externalUserId)
        {
            if (await EnsureLogin())
            {
                try
                {
                    var extUsers = await MobileService.GetTable<ExternalUser>().ToListAsync();
                    var currentExtUser = extUsers.Single(u => u.ExternalUserId == externalUserId);
                    var allUserGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
                    var userGroups = allUserGroups.Where(ug => ug.UserId == currentExtUser.UserId);
                    List<string> groupIds = userGroups.Select(ug => ug.GroupId).ToList();
                    var allGroups = await MobileService.GetTable<Group>().ToListAsync();
                    var availableGroups = allGroups.Where(g => groupIds.Contains(g.Id));
                    //Get only if user is admin
                    var groupsWhereUserIsAdmin = new ObservableCollection<Group>();
                    var roleTypeService = new RoleTypeService(base.AccessToken);
                    foreach (var group in availableGroups)
                    {
                        if(await roleTypeService.CanUserAddOrDeleteItem(currentExtUser.UserId, group.Id))
                            groupsWhereUserIsAdmin.Add(group);
                    }
                    return groupsWhereUserIsAdmin;
                }
                catch (Exception ex)
                {

                }
            }


            return null;
        }

        public async Task<Group> GetGroupForUserByGroupId(string externalUserId, string groupId)
        {
            if (await EnsureLogin())
            {
                var userGroups = await GetGroupForUser(externalUserId);
                var userGroupById = userGroups.Where(i => i.Id == groupId).ToObservableCollection();
                var usrGroupById = userGroupById.Single();

                return usrGroupById;
            }
            return null;
        }

        public async Task InsertTodoList(ToDoList todoList, string userId)
        {
            if (await EnsureLogin())
            {
                if (string.IsNullOrWhiteSpace(todoList.GroupId))
                {
                    var usrGroup = await MobileService.GetTable<UserGroup>().ToListAsync();
                    var choosengroup = usrGroup.Single(g => g.UserId == userId && g.IsUserDefaultGroup);
                    todoList.GroupId = choosengroup.GroupId;
                }
                await MobileService.GetTable<ToDoList>().InsertAsync(todoList);
            }
        }

        public async Task<ObservableCollection<ToDoList>> GetUserLists(string userId)
        {

            if (await EnsureLogin())
            {
                var userGroups = await MobileService.GetTable<UserGroup>().ToCollectionAsync();
                var usrGroups = userGroups.Where(ug => ug.UserId == userId);
                List<string> groups = usrGroups.Select(ug => ug.GroupId).ToList();

                var lists = await MobileService.GetTable<ToDoList>().ToCollectionAsync();
                var usrLists = lists.Where(l => groups.Contains(l.GroupId) && !l.IsDeleted);
                return usrLists.ToObservableCollection();
            }
            return null;
        }

        public async Task<ToDoList> GetUserListById(string listId, string userId)
        {
            if (await EnsureLogin())
            {

                var userLists = await GetUserLists(userId);
                var userListById =
                    await MobileService.GetTable<ToDoList>().Where(l => l.Id == listId).ToCollectionAsync();
                var usrListById = userListById.Single();

                return usrListById;
            }
            return null;
        }
        public async Task<ToDoList> GetListById(string listId)
        {
            if (await EnsureLogin())
            {
            
                var listById =
                    await MobileService.GetTable<ToDoList>().Where(l => l.Id == listId).ToCollectionAsync();
                var list = listById.Single();

                return list;
            }
            return null;
        }

        public async Task<bool> UpdateTodoList(ToDoList toDoList)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<ToDoList>();
                await table.UpdateAsync(toDoList);
                return true;
            }

            return false;
        }

        //NExt
        //public async Task<bool> DeleteTodoList(ToDoList todoList)
        //{
        //    if (await EnsureLogin())
        //    {
        //        var todoItemsDataService = SimpleIoc.Default.GetInstance<ITodoItemsDataService>();
        //        var todoListItems = await todoItemsDataService.GetTodoListsItems(list.Id);
        //        todoListItems.ForEach(async i => await todoItemsDataService.DeleteTodoItem(i));
        //        await MobileService.GetTable<TodoList>().DeleteAsync(list);
        //        return true;
        //    }
        //}

        public async Task<bool> DeleteTodoList(ToDoList list)
        {
            if (await EnsureLogin())
            {
                var todoItemsDataService = new ToDoItemService(base.AccessToken);
                var todoListItems = await todoItemsDataService.GetTodoListsItems(list.Id);
                todoListItems.ForEach(async i => await todoItemsDataService.DeleteTodoItem(i));
                await MobileService.GetTable<ToDoList>().DeleteAsync(list);
                return true;
            }
            return false;
        }

        public async Task<ObservableCollection<ToDoList>> GetTodoListsTableForGroup(string groupId)
        {
            if (await EnsureLogin())
            {
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToCollectionAsync();
                var userGroups = allUserGroups.Where(ug => ug.GroupId == groupId);
                List<string> groupsId = userGroups.Select(ug => ug.GroupId).ToList();

                var table = await MobileService.GetTable<ToDoList>().ToCollectionAsync();
                var items = table.Where(l => groupsId.Contains(l.GroupId) && !l.IsDeleted);
                return items.ToObservableCollection();
            }
            return null;
        }


        public async Task<bool> DeleteTodoListsForGroup(string groupId)
        {
            if (await EnsureLogin())
            {
                var todoListsForGroup = await GetTodoListsTableForGroup(groupId);
                var todoListTable = MobileService.GetTable<ToDoList>();
                todoListsForGroup.ForEach(async tdl => await todoListTable.DeleteAsync(tdl));
                return true;
            }
            return false;
        }

    }
}
