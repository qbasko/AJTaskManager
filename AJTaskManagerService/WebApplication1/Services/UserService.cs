using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.Common;
using WebApplication1.Controllers;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserService : BaseService, IUserService
    {
        public new string AccessToken
        {
            get { return base.AccessToken; }
            set { base.AccessToken = value; }
        }

        public UserService() { }

        public UserService(string accessToken)
        {
            base.AccessToken = accessToken;
        }

        public async Task<string> UserExist(string userId)
        {

            if (await EnsureLogin())
            {
                //var usrDB = MobileService.GetTable<ExternalUser>().Where(u => u.ExternalUserId == userId).ToListAsync();
                // var usrDB = MobileService.GetTable<ExternalUser>().Where(u => u.ExternalUserId == userId).Select(u =>u.Id);
                //string usrDB = MobileService.GetTable<ExternalUser>().Where(u => u.ExternalUserId == userId).ToString();
                try
                {
                    var table = await MobileService.GetTable<ExternalUser>()
                                .Where(u => u.ExternalUserId == userId)
                                .ToCollectionAsync();

                    var usrDB = table.SingleOrDefault();
                    if (usrDB == null)
                    {
                        return null;
                    }
                    else
                    {
                        return usrDB.UserId;
                    }
                }
                catch (Exception ex)
                {

                }
            }


            return null;
        }

        public async Task<MicrosoftAccountUserInfo> GetMicrosoftAccountUserInfo()
        {
            var userInfo = await MobileService.InvokeApiAsync("userInfo", HttpMethod.Get, new Dictionary<string, string>());
            var msAccUserInfo = JsonConvert.DeserializeObject<MicrosoftAccountUserInfoJson>(userInfo.ToString());

            return msAccUserInfo.MicrosoftAccountUserInfo;
        }

        public async Task InsertUser(User user, string extUserId)
        {
            if (await EnsureLogin() && !string.IsNullOrWhiteSpace(extUserId))
            {
                var userInUser =
                    await MobileService.GetTable<User>().Where(u => u.Email == user.Email).ToCollectionAsync();
                if (userInUser.SingleOrDefault() == null)
                {
                    await MobileService.GetTable<User>().InsertAsync(user);

                }
                else
                {
                    user = userInUser.Single();
                }
                //await table.InsertAsync(user);
                ExternalUser externalUser = new ExternalUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    ExternalUserId = extUserId,
                    UserDomainId =
                        (await
                            MobileService.GetTable<UserDomain>()
                                .Where(u => u.DomainKey == (int)UserDomainEnum.Microsoft)
                                .ToCollectionAsync()).Single().Id
                };
                await AddExternalUser(externalUser);
                await AddUserGroup(user);


            }

        }

        public async Task AddExternalUser(ExternalUser externalUser)
        {
            if (await EnsureLogin())
            {
                var userInExtUser =
                    await
                        MobileService.GetTable<ExternalUser>()
                            .Where(u => u.ExternalUserId == externalUser.ExternalUserId)
                            .ToCollectionAsync();
                if (userInExtUser.SingleOrDefault() == null)
                    await MobileService.GetTable<ExternalUser>().InsertAsync(externalUser);

                //await tableExternalUser.InsertAsync();
            }

        }

        public async Task AddUserGroup(User user)
        {
            var userGroups = await MobileService.GetTable<UserGroup>().Where(t => t.UserId == user.Id).ToCollectionAsync();
            var userGroupExist = userGroups.Where(i => i.IsUserDefaultGroup);

            var userHasGroup = userGroupExist.FirstOrDefault();

            if (userHasGroup == null)
            {
                Group group = new Group()
                {
                    Id = Guid.NewGuid().ToString(),
                    GroupName = "Default group for user: " + user.Id
                };

                await MobileService.GetTable<Group>().InsertAsync(group);

                var roles =
                    await MobileService.GetTable<RoleType>().Where(r => r.RoleKey == (int)RoleTypeEnum.Admin).ToCollectionAsync();

                var userRoleId = roles.Single().Id;

                UserGroup userGroup = new UserGroup()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    GroupId = group.Id,
                    RoleTypeId = userRoleId,
                    IsUserDefaultGroup = true

                };
                await MobileService.GetTable<UserGroup>().InsertAsync(userGroup);



            }
        }

        public async Task<string> GetUserId(string externalUserId, UserDomainEnum currentUserDomain)
        {
            if (await EnsureLogin())
            {
                try
                {
                    var domains = await MobileService.GetTable<UserDomain>().ToListAsync();
                    var currentDomain = domains.Single(d => d.DomainKey == (int)currentUserDomain);
                    var externalUsr = await MobileService.GetTable<ExternalUser>().ToListAsync();
                    var currentExtUsr =
                        externalUsr.Single(u => u.ExternalUserId == externalUserId && u.UserDomainId == currentDomain.Id);

                    return currentExtUsr.UserId;
                }
                catch (Exception ex)
                {


                }
            }
            return "";
        }

        public async Task<System.Collections.ObjectModel.ObservableCollection<User>> GetUsersFromGroup(string groupId)
        {
            if (await EnsureLogin())
            {
                var userGroupService = new UserGroupService(base.AccessToken);
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
            }
            return null;
        }


        public async Task<User> GetUsersByEmail(string email)
        {
            if (await EnsureLogin())
            {
                var users = await MobileService.GetTable<User>().ToCollectionAsync();
                return users.SingleOrDefault(u => u.Email == email);
            }
            return null;
        }

        public async Task<User> GetUserById(string id)
        {
            if (await EnsureLogin())
            {
                var users = await MobileService.GetTable<User>().Where(u => u.Id == id).ToCollectionAsync();
                return users.SingleOrDefault();
            }
            return null;
        }


        public async Task<bool> Update(User user)
        {
            if (await EnsureLogin())
            {
                await MobileService.GetTable<User>().UpdateAsync(user);
                return true;
            }
            return false;
        }

        public async Task<User> GetUser(string externalUserId, UserDomainEnum currentUserDomain)
        {
            if (await EnsureLogin())
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
            }
            return null;
        }

        public async Task<string> GetUserInternalId(string externalUserId, UserDomainEnum currentUserDomain)
        {
            if (await EnsureLogin())
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
            }
            return null;
        }

 
    }
}