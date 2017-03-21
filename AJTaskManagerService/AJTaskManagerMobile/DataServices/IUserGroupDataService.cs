using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.DataServices
{
    public interface IUserGroupDataService
    {
        Task<bool> Insert(UserGroup userGroup);
        Task<bool> Update(UserGroup userGroup);
        Task<bool> DeleteUserGroup(UserGroup userGroup);
        Task<bool> DeleteUserGroup(string userGroupId);
        Task<bool> DeleteUserGroupsForGroup(string groupId);
        Task<ObservableCollection<UserGroup>> GetUserGroupTableForGroup(string groupId);
        Task<UserGroup> GetUserGroup(string userId, string groupId);
    }
}
