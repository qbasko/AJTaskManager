using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;
using WebApplication1.Common;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class GroupService : BaseService, IGroupService
    {

        public new string AccessToken
        {
            get { return base.AccessToken; }
            set { base.AccessToken = value; }
        }

        public GroupService()
        {
        }

        public GroupService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<string> GroupNameByGroupId(string id)
        {
            if (await EnsureLogin())
            {
                var groupName = await MobileService.GetTable<Group>().Where(g => g.Id == id).ToCollectionAsync();
                return groupName.Select(g => g.GroupNameTruncated).Single();
            }
            return "";
        }

        public async Task<Group> GetGroupById(string id)
        {
            if (await EnsureLogin())
            {
                var group = await MobileService.GetTable<Group>().Where(g => g.Id == id).ToCollectionAsync();
                return group.Single();
            }
            return null;
        }

        public async Task UpdateGroup(Group group)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<Group>().UpdateAsync(group);
            }
        }

        public async Task<ObservableCollection<Group>> GetGroupsAvailableForUser(string externalUserId)
        {
            if (await EnsureLogin())
            {
                var extUsers = await MobileService.GetTable<ExternalUser>().ToListAsync();
                var currentExtUser = extUsers.Single(u => u.ExternalUserId == externalUserId);
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
                var userGroups = allUserGroups.Where(ug => ug.UserId == currentExtUser.UserId);
                List<string> groupIds = userGroups.Select(ug => ug.GroupId).ToList();
                var allGroups = await MobileService.GetTable<Group>().ToListAsync();
                var availableGroups = allGroups.Where(g => groupIds.Contains(g.Id));
                return availableGroups.ToObservableCollection();
            }
            return null;
        }

        public async Task<ObservableCollection<Group>> GetGroupsAvailableForUserWhereUserIsAdmin(string externalUserId)
        {
            if (await EnsureLogin())
            {
                var extUsers = await MobileService.GetTable<ExternalUser>().ToListAsync();
                var currentExtUser = extUsers.Single(u => u.ExternalUserId == externalUserId);
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
                var userGroups = allUserGroups.Where(ug => ug.UserId == currentExtUser.UserId);
                List<string> groupIds = userGroups.Select(ug => ug.GroupId).ToList();
                var allGroups = await MobileService.GetTable<Group>().ToListAsync();
                var availableGroups = allGroups.Where(g => groupIds.Contains(g.Id));
                var roleTypeService = new RoleTypeService(base.AccessToken);
                var groupsWhereUserIsAdmin = new ObservableCollection<Group>();
                foreach (var group in availableGroups)
                {
                    if (await roleTypeService.CanUserAddOrDeleteItem(currentExtUser.UserId, group.Id))
                        groupsWhereUserIsAdmin.Add(group);
                }
                return groupsWhereUserIsAdmin.ToObservableCollection();
            }
            return null;
        }

        public async Task<bool> DeleteGroups(IEnumerable<Group> groups)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<Group>();
                foreach (var group in groups)
                {
                    var userGroupDataService = new UserGroupService(base.AccessToken);
                    await userGroupDataService.DeleteUserGroupsForGroup(group.Id);
                    var todoListDataService = new ToDoListService(base.AccessToken);
                    await todoListDataService.DeleteTodoListsForGroup(group.Id);
                    await table.DeleteAsync(group);
                }
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteGroup(Group group)
        {
            if (await EnsureLogin())
            {
                var table = MobileService.GetTable<Group>();
                var userGroupDataService = new UserGroupService(base.AccessToken);
                await userGroupDataService.DeleteUserGroupsForGroup(group.Id);
                var todoListDataService = new ToDoListService(base.AccessToken);
                await todoListDataService.DeleteTodoListsForGroup(group.Id);
                var taskItemService = new TaskItemService(base.AccessToken);
                await taskItemService.DeleteTaskItemsForGroup(group.Id);
                await table.DeleteAsync(group);
                return true;
            }
            return false;
        }

        public async Task<bool> AddUserGroup(Group group, User user)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<Group>().InsertAsync(group);
                var availableRoles = await MobileService.GetTable<RoleType>().ToListAsync();
                var adminRole = availableRoles.Where(r => r.RoleKey == (int)RoleTypeEnum.Admin);
                UserGroup userGroup = new UserGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupId = @group.Id,
                    UserId = user.Id,
                    RoleTypeId = adminRole.Single().Id,
                    IsUserDefaultGroup = false
                };
                await MobileService.GetTable<UserGroup>().InsertAsync(userGroup);
                return true;
            }
            return false;
        }

    }
}