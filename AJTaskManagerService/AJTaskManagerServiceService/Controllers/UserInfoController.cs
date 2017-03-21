﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json.Linq;

namespace AJTaskManagerServiceService.Controllers
{
    public class UserInfoController : ApiController
    {
        public ApiServices Services { get; set; }

        [AuthorizeLevel(AuthorizationLevel.User)]
        public async Task<JObject> GetUserInfo()
        {
            ServiceUser user = this.User as ServiceUser;
            if (user == null)
            {
                throw new InvalidOperationException("This can only be called by authenticated clients");
            }

            var identities = await user.GetIdentitiesAsync();
            var result = new JObject();
            var fb = identities.OfType<FacebookCredentials>().FirstOrDefault();
            if (fb != null)
            {
                var accessToken = fb.AccessToken;
                result.Add("facebook", await GetProviderInfo("https://graph.facebook.com/me?access_token=" + accessToken));
            }

            var ms = identities.OfType<MicrosoftAccountCredentials>().FirstOrDefault();
            if (ms != null)
            {
                var accessToken = ms.AccessToken;
                result.Add("microsoft", await GetProviderInfo("https://apis.live.net/v5.0/me/?method=GET&access_token=" + accessToken));
            }

            return result;
        }

        private async Task<JToken> GetProviderInfo(string url)
        {
            var c = new HttpClient();
            var resp = await c.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return JToken.Parse(await resp.Content.ReadAsStringAsync());
        }

    }
}
