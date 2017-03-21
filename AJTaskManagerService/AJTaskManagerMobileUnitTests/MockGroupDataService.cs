using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using AJTaskManagerMobile.DataServices;
using AJTaskManagerMobile.Model.DTO;

namespace AJTaskManagerMobileUnitTests
{
    class MockGroupDataService : IGroupDataService
    {
        public async Task<System.Collections.ObjectModel.ObservableCollection<Group>> GetGroupsAvailableForUser(string externalUserId)
        {
            var result = new ObservableCollection<Group>();
            result.Add(new Group() { Id = "1", GroupName = "test group1" });
            result.Add(new Group() { Id = "2", GroupName = "test group2" });
            return await Task.Factory.StartNew(() => result);
        }

        public Task<bool> DeleteGroups(IEnumerable<Group> groups)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddUserGroup(Group group, User user)
        {
            throw new NotImplementedException();
        }
    }
}
