using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.DTO;

namespace WebApplication1.Services
{
    public class BaseService
    {
        protected static readonly MobileServiceClient MobileService = new MobileServiceClient("https://ajtaskmanagerservice.azure-mobile.net/", "FLdMQrDpZuOTXNyGOuKsNvtOOmgOuO96");

        protected string AccessToken { get; set; }

        protected Task<bool> EnsureLogin()
        {
            if (MobileService.CurrentUser == null)
            {
                try
                {
                    var token = new JObject();
                    token.Add("authenticationToken", AccessToken);
                    return MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount, token).ContinueWith<bool>(t =>
                    {
                        //get information about user
                        // var userInfo= await MobileService.InvokeApiAsync("userInfo", HttpMethod.Get, null);
                        //var msAccUserInfo = JsonConvert.DeserializeObject<MicrosoftAccountUserInfoJson>(userInfo.ToString());

                        // Store credentials

                        //msAccUserInfo.MicrosoftAccountUserInfo.FirstName
                        //msAccUserInfo.MicrosoftAccountUserInfo.LastName
                        //msAccUserInfo.MicrosoftAccountUserInfo.Emails.Account
                        if (t.Exception != null)
                        {
                            return true;
                        }
                        else
                        {
                            System.Diagnostics.Trace.WriteLine("Error logging in: " + t.Exception);
                            return false;
                        }
                    });
                }
                catch (Exception e)
                { }
                return null;
            }

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }
    }
}