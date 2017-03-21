using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.DataServices;

namespace AJTaskManagerMobileUnitTests
{
    class MockRoleTypeDataService : IRoleTypeDataService
    {
        public Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.RoleType>> GetAllRoleTypes()
        {
            throw new NotImplementedException();
        }

        public Task<AJTaskManagerMobile.Model.DTO.RoleType> GetRoleForUserInGroup(string userId, string groupId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CanUserAddOrDeleteItem(string userId, string groupId)
        {
            return await Task.Factory.StartNew(() => true);
        }

        public Task<bool> CanUserEditItem(string userId, string groupId)
        {
            throw new NotImplementedException();
        }
    }
}
