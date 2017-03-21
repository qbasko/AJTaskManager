using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AJTaskManagerMobile.Helpers;
using Microsoft.WindowsAzure.MobileServices;
using System.Net.Http;
using AJTaskManagerMobile.Model;
using Newtonsoft.Json;
using Syncfusion.CompoundFile.XlsIO.Native;
using AJTaskManagerMobile.Common;

namespace AJTaskManagerMobile.DataServices
{
    public abstract class DataServiceBase
    {
        
        // Our MobileServiceClient instance with Url and application key set
        public readonly static MobileServiceClient MobileService = new MobileServiceClient(
          "https://ajtaskmanagerservice.azure-mobile.net/",
            "FLdMQrDpZuOTXNyGOuKsNvtOOmgOuO96"
            );

        protected static MobileServiceAuthenticationProvider Provider = MobileServiceAuthenticationProvider.MicrosoftAccount;

        protected async Task<T> ExecuteAuthenticated<T>(Func<Task<T>> t, int retries = 1)
        {
            int retry = 0;
            T retVal = default(T);

            while (retry <= retries)
            {
                // If we have no current user, login
                if (MobileService.CurrentUser == null)
                {
                    // If login fails return default
                    if (!await Login())
                        return retVal;
                }

                // Try and execute task
                try
                {
                    retVal = await t();
                    break;
                }
                catch (InvalidOperationException ioex)
                {
                    // If task has failed because it was unauthorised try again
                    if (ioex.Message == "Error: Unauthorized" || ioex.Message == "Error: The authentication token has expired.")
                    {
                        Logout();
                    }

                    retry++;
                }
            }

            return retVal;
        }

        public static bool HasUserLoggedIn()
        {
            var userId = StorageHelper.GetSetting<string>(Constants.UserId, null);
            var userToken = StorageHelper.GetSetting<string>(Constants.UserToken, null);

            if (userId == null || userToken == null)
                return false;

            return true;
        }

        public static async Task<bool> Login()
        {
            // First have a look and see if we have the user's token
            var userId = StorageHelper.GetSetting<string>(Constants.UserId, null);
            var userToken = StorageHelper.GetSetting<string>(Constants.UserToken, null);

            bool success = true;

            if (userId != null && userToken != null)
            {
                // Create user and apply to client
                var user = new MobileServiceUser(userId);
                user.MobileServiceAuthenticationToken = userToken;
                MobileService.CurrentUser = user;
            }
            else
            {
                try
                {
                    // Login
                    var user =
                        await MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);

                    var info = await MobileService.InvokeApiAsync("userInfo", HttpMethod.Get, null);
                    var msAccUserInfo = JsonConvert.DeserializeObject<MicrosoftAccountUserInfoJson>(info.ToString());

                    // Store credentials
                    StorageHelper.StoreSetting(Constants.UserId, user.UserId, true);
                    StorageHelper.StoreSetting(Constants.UserToken, user.MobileServiceAuthenticationToken, true);
                    StorageHelper.StoreSetting(Constants.UserFirstName, msAccUserInfo.MicrosoftAccountUserInfo.FirstName, true);
                    StorageHelper.StoreSetting(Constants.UserLastName, msAccUserInfo.MicrosoftAccountUserInfo.LastName, true);
                    StorageHelper.StoreSetting(Constants.UserEmail, msAccUserInfo.MicrosoftAccountUserInfo.Emails.Account, true);
                }
                catch (Exception ex)
                {
                    success = false;
                }
            }

            return success;
        }


        public static void Logout()
        {
            MobileService.Logout();

            // Clear credentials
            StorageHelper.StoreSetting(Constants.UserId, null, true);
            StorageHelper.StoreSetting(Constants.UserToken, null, true);
        }
    }
}
