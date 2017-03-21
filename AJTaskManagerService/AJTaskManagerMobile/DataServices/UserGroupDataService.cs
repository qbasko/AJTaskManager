using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.DataServices
{
    public class UserGroupDataService : DataServiceBase, IUserGroupDataService
    {
        public async Task<bool> DeleteUserGroup(Model.DTO.UserGroup userGroup)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<UserGroup>().DeleteAsync(userGroup);
                return true;
            });
        }
        public async Task<bool> DeleteUserGroup(string userGroupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var userGroup = await MobileService.GetTable<UserGroup>().Where(ug => ug.Id == userGroupId).ToCollectionAsync();
                if (userGroup.SingleOrDefault() != null)
                    await MobileService.GetTable<UserGroup>().DeleteAsync(userGroup.Single());
                return true;
            });
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<UserGroup>> GetUserGroupTableForGroup(string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var userGroupTable = MobileService.GetTable<UserGroup>();
                var userGroups = await userGroupTable.ToCollectionAsync();
                var userGroupsForCurrentGroup = userGroups.Where(ug => ug.GroupId == groupId);
                return userGroupsForCurrentGroup.ToObservableCollection();
            });
        }

        public async Task<bool> DeleteUserGroupsForGroup(string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var userGroupTable = MobileService.GetTable<UserGroup>();
                var userGroups = await GetUserGroupTableForGroup(groupId);
                userGroups.ForEach(async ug => await userGroupTable.DeleteAsync(ug));
                return true;
            });
        }

        public async Task<bool> Insert(UserGroup userGroup)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<UserGroup>().InsertAsync(userGroup);
                return true;
            });
        }

        public async Task<UserGroup> GetUserGroup(string userId, string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var userGroupsForGroup = await GetUserGroupTableForGroup(groupId);
                var targetUserGroup = userGroupsForGroup.Where(ug => ug.UserId == userId);
                return targetUserGroup.SingleOrDefault();
            });
        }


        public async Task<bool> Update(UserGroup userGroup)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<UserGroup>().UpdateAsync(userGroup);
                return true;
            });
        }
    }
}
