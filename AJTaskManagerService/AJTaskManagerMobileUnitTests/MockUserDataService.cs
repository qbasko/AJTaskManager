using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobileUnitTests
{
    class MockUserDataService : IUserDataService
    {
        public Task<bool> Insert(AJTaskManagerMobile.Model.DTO.User user, AJTaskManagerMobile.Common.UserDomainsEnum userDomainEnum, string externalUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(AJTaskManagerMobile.Model.DTO.User user)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserInternalId(string externalUserId, AJTaskManagerMobile.Common.UserDomainsEnum userDomainEnum)
        {
            return await Task.Factory.StartNew(() => "");
        }

        public Task<AJTaskManagerMobile.Model.DTO.User> GetUser(string externalUserId, AJTaskManagerMobile.Common.UserDomainsEnum currentUserDomain)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<AJTaskManagerMobile.Model.DTO.User>> GetUsersFromGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public Task<AJTaskManagerMobile.Model.DTO.User> GetUsersByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<AJTaskManagerMobile.Model.DTO.User> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public void Test()
        {
            throw new NotImplementedException();
        }
    }
}
