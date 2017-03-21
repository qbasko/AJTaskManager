using System.Threading.Tasks;
using WebApplication1.Common;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface IUserService : IService
    {
        Task<string> UserExist(string userId);

        Task<MicrosoftAccountUserInfo> GetMicrosoftAccountUserInfo();

        Task InsertUser(User user, string extUserId);

        Task AddExternalUser(ExternalUser externalUser);

        Task AddUserGroup(User user);

        Task<string> GetUserId(string externalUserId, UserDomainEnum currentUserDomain);

        Task<System.Collections.ObjectModel.ObservableCollection<User>> GetUsersFromGroup(string groupId);

        Task<User> GetUsersByEmail(string email);

        Task<User> GetUserById(string id);

        Task<bool> Update(User user);

        Task<User> GetUser(string externalUserId, UserDomainEnum currentUserDomain);

        Task<string> GetUserInternalId(string externalUserId, UserDomainEnum currentUserDomain);  
    }
}
