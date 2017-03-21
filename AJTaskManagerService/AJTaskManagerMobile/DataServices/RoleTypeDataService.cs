using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;

namespace AJTaskManagerMobile.DataServices
{
    public class RoleTypeDataService : DataServiceBase, IRoleTypeDataService
    {
        public async Task<System.Collections.ObjectModel.ObservableCollection<Model.DTO.RoleType>> GetAllRoleTypes()
        {
            return await ExecuteAuthenticated(async () => await MobileService.GetTable<RoleType>().ToCollectionAsync());
        }

        public async Task<RoleType> GetRoleForUserInGroup(string userId, string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var userGroup = await MobileService.GetTable<UserGroup>()
                    .Where(ug => ug.UserId == userId && ug.GroupId == groupId)
                    .ToCollectionAsync();
                if (userGroup.SingleOrDefault() == null)
                    return null;
                string roleTypeId = userGroup.Single().RoleTypeId;
                var role = await MobileService.GetTable<RoleType>().Where(r => r.Id == roleTypeId).ToCollectionAsync();
                return role.Single();
            });
        }

        public async Task<bool> CanUserAddOrDeleteItem(string userId, string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var roleTypeService = SimpleIoc.Default.GetInstance<IRoleTypeDataService>();
                RoleType roleType = await roleTypeService.GetRoleForUserInGroup(userId, groupId);
                return roleType.RoleKey == (int)UserRoleEnum.Admin;
            });
        }


        public async Task<bool> CanUserEditItem(string userId, string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var roleTypeService = SimpleIoc.Default.GetInstance<IRoleTypeDataService>();
                RoleType roleType = await roleTypeService.GetRoleForUserInGroup(userId, groupId);
                return roleType.RoleKey == (int)UserRoleEnum.Editor || roleType.RoleKey == (int)UserRoleEnum.Admin;
            });
        }
    }
}
