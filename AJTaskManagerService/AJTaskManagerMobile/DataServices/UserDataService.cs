using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Common;
using AJTaskManagerMobile.Model;
using AJTaskManagerMobile.Model.DTO;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.WindowsAzure.MobileServices;
using Syncfusion.Data.Extensions;

namespace AJTaskManagerMobile.DataServices
{
    public class UserDataService : DataServiceBase, IUserDataService
    {
        public async Task<bool> Insert(User user, UserDomainsEnum userDomainEnum, string externalUserId)
        {
            return await this.ExecuteAuthenticated(async () =>
            {
                var usersTable = await MobileService.GetTable<User>().ToCollectionAsync();

                var existingUser = usersTable.Where(u => u.Email == user.Email);
                if (existingUser.SingleOrDefault() == null)
                {
                    await MobileService.GetTable<User>().InsertAsync(user);
                }
                else
                {
                    user = existingUser.Single();
                }
                await AddExtUser(user, userDomainEnum, externalUserId);
                await AddUserGroup(user);
                return true;
            });
        }

        public async Task<string> GetUserInternalId(string externalUserId, UserDomainsEnum currentUserDomain)
        {
            return await this.ExecuteAuthenticated(async () =>
            {
                try
                {
                    var domainsTable = MobileService.GetTable<UserDomain>();
                    var domains = await domainsTable.ToCollectionAsync();
                    var currentDomain = domains.Single(d => d.DomainKey == (int)currentUserDomain);
                    var extUsers = await MobileService.GetTable<ExternalUser>().ToListAsync();
                    var currentExtUser =
                        extUsers.Single(u => u.ExternalUserId == externalUserId && u.UserDomainId == currentDomain.Id);

                    return currentExtUser.UserId;
                }
                catch (Exception ex)
                {

                }
                return "";
            });
        }

        public async Task<User> GetUser(string externalUserId, UserDomainsEnum currentUserDomain)
        {
            return await ExecuteAuthenticated(async () =>
            {
                try
                {
                    var domainsTable = MobileService.GetTable<UserDomain>();
                    var domains = await domainsTable.ToCollectionAsync();
                    var currentDomain = domains.Single(d => d.DomainKey == (int)currentUserDomain);
                    var extUsers = await MobileService.GetTable<ExternalUser>().ToListAsync();
                    var currentExtUser =
                        extUsers.Single(u => u.ExternalUserId == externalUserId && u.UserDomainId == currentDomain.Id);
                    var users = await MobileService.GetTable<User>().ToListAsync();
                    var user = users.Single(u => u.Id == currentExtUser.UserId);
                    return user;
                }
                catch (Exception ex)
                {

                }
                return null;
            });
        }

        private async Task AddExtUser(User user, UserDomainsEnum userDomainEnum, string externalUserId)
        {
            var userDomainsTable = MobileService.GetTable<UserDomain>();
            var userDomains = await userDomainsTable.ToCollectionAsync();
            var domain = userDomains.SingleOrDefault(d => d.DomainKey == (int)userDomainEnum);
            if (domain != null)
            {
                var externalUserstable = MobileService.GetTable<ExternalUser>();
                var externalUsers = await externalUserstable.ToCollectionAsync();
                var extUsers = externalUsers.Where(eu => eu.UserId == user.Id && eu.UserDomainId == domain.Id);
                if (extUsers.SingleOrDefault() == null)
                {
                    ExternalUser extUser = new ExternalUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        ExternalUserId = externalUserId,
                        UserDomainId = domain.Id
                    };
                    await externalUserstable.InsertAsync(extUser);
                }
            }
        }

        private async Task AddUserGroup(User user)
        {
            var userGroups = await MobileService.GetTable<UserGroup>().ToListAsync();
            var userGroup = userGroups.Where(ug => ug.UserId == user.Id);
            if (userGroup.FirstOrDefault() == null)
            {
                Group group = new Group()
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupName = "Default group for user: " + user.Id
                };
                await MobileService.GetTable<Group>().InsertAsync(@group);

                var availableRoles = await MobileService.GetTable<RoleType>().ToListAsync();
                var adminRole = availableRoles.Where(r => r.RoleKey == (int)UserRoleEnum.Admin);

                UserGroup ug = new UserGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupId = @group.Id,
                    UserId = user.Id,
                    RoleTypeId = adminRole.Single().Id,
                    IsUserDefaultGroup = true
                };

                await MobileService.GetTable<UserGroup>().InsertAsync(ug);
            }
        }

        public async void Test()
        {
            try
            {
                var domainTable = MobileService.GetTable<UserDomain>();
                var domains = await domainTable.ToCollectionAsync();
                var domain = domains.SingleOrDefault(d => d.UserDomainName == "Microsoft");
                if (domain != null)
                {
                    User user = new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "aa",
                        LastName = "ss",
                        Email = "email"
                    };
                    var userTable = MobileService.GetTable<User>();
                    await userTable.InsertAsync(user);
                    var users = await userTable.Where(u => u.Id == user.Id).ToCollectionAsync();
                    user = users.Single();
                    var externalUserstable = MobileService.GetTable<ExternalUser>();
                    ExternalUser extUser = new ExternalUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = user.Id,
                        ExternalUserId = "11111",
                        UserDomainId = domain.Id
                    };
                    await externalUserstable.InsertAsync(extUser);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<User>> GetUsersFromGroup(string groupId)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var userGroupService = SimpleIoc.Default.GetInstance<IUserGroupDataService>();
                var userGroups = await userGroupService.GetUserGroupTableForGroup(groupId);
                var allUsers = await MobileService.GetTable<User>().ToCollectionAsync();
                ObservableCollection<User> groupUsers = new ObservableCollection<User>();
                foreach (var userGroup in userGroups)
                {
                    var user = allUsers.SingleOrDefault(u => u.Id == userGroup.UserId);
                    if (user != null)
                        groupUsers.Add(user);
                }
                return groupUsers;
            });
        }


        public async Task<User> GetUsersByEmail(string email)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var users = await MobileService.GetTable<User>().ToCollectionAsync();
                return users.SingleOrDefault(u => u.Email == email);
            });
        }

        public async Task<User> GetUserById(string id)
        {
            return await ExecuteAuthenticated(async () =>
            {
                var users = await MobileService.GetTable<User>().Where(u => u.Id == id).ToCollectionAsync();
                return users.SingleOrDefault();
            });
        }


        public async Task<bool> Update(User user)
        {
            return await ExecuteAuthenticated(async () =>
            {
                await MobileService.GetTable<User>().UpdateAsync(user);
                return true;
            });
        }
    }
}
