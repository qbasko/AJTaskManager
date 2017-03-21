using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobile.DataServices
{
    public interface IUserDataService
    {
        Task<bool> Insert(User user, UserDomainsEnum userDomainEnum, string externalUserId);
        Task<bool> Update(User user);
        Task<string> GetUserInternalId(string externalUserId, UserDomainsEnum userDomainEnum);
        Task<User> GetUser(string externalUserId, UserDomainsEnum currentUserDomain);
        Task<ObservableCollection<User>> GetUsersFromGroup(string groupId);
        Task<User> GetUsersByEmail(string email);
        Task<User> GetUserById(string id);
        void Test();

    }
}
