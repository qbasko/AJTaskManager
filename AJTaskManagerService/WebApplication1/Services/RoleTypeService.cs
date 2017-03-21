using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;
using Microsoft.WindowsAzure.MobileServices;
using WebApplication1.Common;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class RoleTypeService : BaseService
    {
        public RoleTypeService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<RoleType>> GetAllRoleTypes()
        {
            if (await EnsureLogin())
                return await MobileService.GetTable<RoleType>().ToCollectionAsync();
            else
            {
                return null;
            }
            
        }

        public async Task<RoleType> GetRoleForUserInGroup(string userId, string groupId)
        {
             if (await EnsureLogin())
            {
                var userGroup = await MobileService.GetTable<UserGroup>()
                    .Where(ug => ug.UserId == userId && ug.GroupId == groupId)
                    .ToCollectionAsync();
                if (userGroup.SingleOrDefault() == null)
                    return null;
                string roleTypeId = userGroup.Single().RoleTypeId;
                var role = await MobileService.GetTable<RoleType>().Where(r => r.Id == roleTypeId).ToCollectionAsync();
                return role.Single();
            }
             else
             {
                 return null;
             }
            
        }

        public async Task<bool> CanUserAddOrDeleteItem(string userId, string groupId)
        {
             if (await EnsureLogin())
            {
                RoleType roleType = await GetRoleForUserInGroup(userId, groupId);
                return roleType.RoleKey == (int)RoleTypeEnum.Admin;
            }
             else
             {
                 return false;
             }
            
        }

        public async Task<bool> CanUserEditItem(string userId, string groupId)
        {
             if (await EnsureLogin())
            {
                RoleType roleType = await GetRoleForUserInGroup(userId, groupId);
                return roleType.RoleKey == (int)RoleTypeEnum.Editor || roleType.RoleKey == (int)RoleTypeEnum.Admin;
            }
             else
             {
                 return false;
             }
            
        }
    }
}
