using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class UserGroupService : BaseService
    {
        public UserGroupService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<bool> DeleteUserGroup(UserGroup userGroup)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<UserGroup>().DeleteAsync(userGroup);
                return true;
            }
            return false;
        }
        public async Task<bool> DeleteUserGroup(string userGroupId)
        {
            if (await EnsureLogin())
            {
                var userGroup = await MobileService.GetTable<UserGroup>().Where(ug => ug.Id == userGroupId).ToCollectionAsync();
                if (userGroup.SingleOrDefault() != null)
                    await MobileService.GetTable<UserGroup>().DeleteAsync(userGroup.Single());
                return true;
            }
            return false;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<UserGroup>> GetUserGroupTableForGroup(string groupId)
        {
            if (await EnsureLogin())
            {
                var userGroupTable = MobileService.GetTable<UserGroup>();
                var userGroups = await userGroupTable.ToCollectionAsync();
                var userGroupsForCurrentGroup = userGroups.Where(ug => ug.GroupId == groupId);
                return userGroupsForCurrentGroup.ToObservableCollection();
            }
            return null;
        }

        public async Task<bool> DeleteUserGroupsForGroup(string groupId)
        {
            if (await EnsureLogin())
            {
                var userGroupTable = MobileService.GetTable<UserGroup>();
                var userGroups = await GetUserGroupTableForGroup(groupId);
                userGroups.ForEach(async ug => await userGroupTable.DeleteAsync(ug));
                return true;
            }
            return false;
        }

        public async Task<bool> Insert(UserGroup userGroup)
        {
            if (await EnsureLogin())
            {
                var existingUserGroup =
                    await MobileService.GetTable<UserGroup>()
                        .Where(ug => ug.UserId == userGroup.Id && ug.GroupId == userGroup.Id).ToCollectionAsync();
                if (!existingUserGroup.Any())
                    await MobileService.GetTable<UserGroup>().InsertAsync(userGroup);
                return true;
            }
            return false;
        }

        public async Task<UserGroup> GetUserGroup(string userId, string groupId)
        {
            if (await EnsureLogin())
            {
                var userGroupsForGroup = await GetUserGroupTableForGroup(groupId);
                var targetUserGroup = userGroupsForGroup.Where(ug => ug.UserId == userId);
                return targetUserGroup.SingleOrDefault();
            }
            return null;
        }

        public async Task<UserGroup> GetUserGroupById(string userGroupId)
        {
            if (await EnsureLogin())
            {
                var userGroup =
                    await MobileService.GetTable<UserGroup>().Where(ug => ug.Id == userGroupId).ToCollectionAsync();
                return userGroup.SingleOrDefault();
            }
            return null;
        }


        public async Task<bool> Update(UserGroup userGroup)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<UserGroup>().UpdateAsync(userGroup);
                return true;
            }
            return false;
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
                    return availableGroups.ToObservableCollection();


                }
                catch (Exception ex)
                {

                }
            }


            return null;
        }
    }
}
