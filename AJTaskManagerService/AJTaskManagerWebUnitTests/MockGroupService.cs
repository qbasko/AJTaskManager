using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace AJTaskManagerWebUnitTests
{
    class MockGroupService : IGroupService
    {
        public Task<string> GroupNameByGroupId(string id)
        {
            string groupName = "";
            switch (id)
            {
                case "G1": groupName = "Group1";
                    break;
                case "G2":
                    groupName = "Group2";
                    break;
                case "G3":
                    groupName = "Group3";
                    break;
            }
            return new Task<string>(()=>groupName);
        }

        public Task<WebApplication1.DTO.Group> GetGroupById(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateGroup(WebApplication1.DTO.Group group)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<WebApplication1.DTO.Group>> GetGroupsAvailableForUser(string externalUserId)
        {
            throw new NotImplementedException();
        }

        public Task<System.Collections.ObjectModel.ObservableCollection<WebApplication1.DTO.Group>> GetGroupsAvailableForUserWhereUserIsAdmin(string externalUserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGroups(IEnumerable<WebApplication1.DTO.Group> groups)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGroup(WebApplication1.DTO.Group group)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddUserGroup(WebApplication1.DTO.Group group, WebApplication1.DTO.User user)
        {
            throw new NotImplementedException();
        }

        public string AccessToken
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
