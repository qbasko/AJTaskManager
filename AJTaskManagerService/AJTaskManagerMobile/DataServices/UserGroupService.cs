using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;
using Microsoft.WindowsAzure.MobileServices;

namespace AJTaskManagerMobile.DataServices
{
    public class UserGroupService : DataServiceBase
    {
        public async Task<MobileServiceCollection<User, User>> GetUsersTable()
        {
            return await ExecuteAuthenticated(() =>
            {
                var table = MobileService.GetTable<User>();
                var items = table.ToCollectionAsync();
                return items;
            }
                );
        }

        public async Task<MobileServiceCollection<Group, Group>> GetGroupsTable()
        {
            return await ExecuteAuthenticated(() =>
            {
                var table = MobileService.GetTable<Group>();
                var items = table.ToCollectionAsync();
                return items;
            }
                );
        }
    }
}
