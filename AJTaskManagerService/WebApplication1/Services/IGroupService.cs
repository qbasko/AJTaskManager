using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public interface IGroupService : IService
    {
        Task<string> GroupNameByGroupId(string id);
        Task<Group> GetGroupById(string id);
        Task UpdateGroup(Group group);
        Task<ObservableCollection<Group>> GetGroupsAvailableForUser(string externalUserId);
        Task<ObservableCollection<Group>> GetGroupsAvailableForUserWhereUserIsAdmin(string externalUserId);
        Task<bool> DeleteGroups(IEnumerable<Group> groups);
        Task<bool> DeleteGroup(Group group);
        Task<bool> AddUserGroup(Group group, User user);
    }
}
