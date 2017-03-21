using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.DataServices
{
    public interface IRoleTypeDataService
    {
        Task<ObservableCollection<RoleType>> GetAllRoleTypes();

        Task<RoleType> GetRoleForUserInGroup(string userId, string groupId);
        Task<bool> CanUserAddOrDeleteItem(string userId, string groupId);
        Task<bool> CanUserEditItem(string userId, string groupId);
    }
}
