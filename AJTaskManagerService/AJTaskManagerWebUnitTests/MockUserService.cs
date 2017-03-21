using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace AJTaskManagerWebUnitTests
{
    class MockUserService : IUserService
    {
        public string AccessToken { get; set; }

        public Task<string> UserExist(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<WebApplication1.DTO.MicrosoftAccountUserInfo> GetMicrosoftAccountUserInfo()
        {
            throw new NotImplementedException();
        }

        public Task InsertUser(WebApplication1.DTO.User user, string extUserId)
        {
            throw new NotImplementedException();
        }

        public Task AddExternalUser(WebApplication1.DTO.ExternalUser externalUser)
        {
            throw new NotImplementedException();
        }

        public Task AddUserGroup(WebApplication1.DTO.User user)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetUserId(string externalUserId, WebApplication1.Common.UserDomainEnum currentUserDomain)
        {
            //await Task.Yield();
            return await Task.Factory.StartNew(()=> "1234"); // AsyncHelpers.RunSync(() => Task.Run(() => "12345")); 
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<WebApplication1.DTO.User>> GetUsersFromGroup(string groupId)
        {
            throw new NotImplementedException();
        }

        public Task<WebApplication1.DTO.User> GetUsersByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<WebApplication1.DTO.User> GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(WebApplication1.DTO.User user)
        {
            throw new NotImplementedException();
        }

        public Task<WebApplication1.DTO.User> GetUser(string externalUserId, WebApplication1.Common.UserDomainEnum currentUserDomain)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserInternalId(string externalUserId, WebApplication1.Common.UserDomainEnum currentUserDomain)
        {
            throw new NotImplementedException();
        }
    }
}
