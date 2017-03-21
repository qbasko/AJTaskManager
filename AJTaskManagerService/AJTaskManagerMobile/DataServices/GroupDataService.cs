using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.DataServices
{
    public class GroupDataService : DataServiceBase, IGroupDataService
    {
        public async Task<ObservableCollection<Group>> GetGroupsAvailableForUser(string externalUserId)
        {
            return await this.ExecuteAuthenticated(async () =>
            {
                var extUsers = await MobileService.GetTable<ExternalUser>().ToListAsync();
                var currentExtUser = extUsers.Single(u => u.ExternalUserId == externalUserId);
                var allUserGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
                var userGroups = allUserGroups.Where(ug => ug.UserId == currentExtUser.UserId);
                List<string> groupIds = userGroups.Select(ug => ug.GroupId).ToList();
                var allGroups = await MobileService.GetTable<Group>().ToListAsync();
                var availableGroups = allGroups.Where(g => groupIds.Contains(g.Id));
                return availableGroups.ToObservableCollection();
            });
        }

        public async Task<bool> DeleteGroups(IEnumerable<Group> groups)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var table = MobileService.GetTable<Group>();
                foreach (var group in groups)
                {
                    var userGroupDataService = SimpleIoc.Default.GetInstance<IUserGroupDataService>();
                    await userGroupDataService.DeleteUserGroupsForGroup(group.Id);

                    var todoListDataService = SimpleIoc.Default.GetInstance<ITodoListDataService>();
                    await todoListDataService.DeleteTodoListsForGroup(group.Id);

                    var taskItemDataService = SimpleIoc.Default.GetInstance<ITaskItemDataService>();
                    await taskItemDataService.DeleteTaskItemsForGroup(group.Id);

                    await table.DeleteAsync(group);
                }
                return true;
            });
        }

        public async Task<bool> AddUserGroup(Group group, User user)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<Group>().InsertAsync(group);
                var availableRoles = await MobileService.GetTable<RoleType>().ToListAsync();
                var adminRole = availableRoles.Where(r => r.RoleKey == (int)UserRoleEnum.Admin);
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
            });
        }
    }
}
