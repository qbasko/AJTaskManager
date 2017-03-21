using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model.DTO;
using Microsoft.WindowsAzure.MobileServices;

namespace AJTaskManagerMobile.DataServices
{
    public interface IGroupDataService
    {
        Task<ObservableCollection<Group>> GetGroupsAvailableForUser(string externalUserId);
        Task<bool> DeleteGroups(IEnumerable<Group> groups);
        Task<bool> AddUserGroup(Group group, User user);
    }
}
