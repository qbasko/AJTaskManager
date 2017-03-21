using Microsoft.Owin.Security.MicrosoftAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1
{
    class LinqToMicrosoftAuthenticationProvider : MicrosoftAccountAuthenticationProvider
    {
        public const string AccessToken = "MicrosoftAccessToken";
        public const string Email = "MicrosoftEmail";
        public const string UserId = "UserId";
        public const string FirstName = "FirstName";
        public const string LastName = "LastName";


        public override Task Authenticated(MicrosoftAccountAuthenticatedContext context)
        {
            context.Identity.AddClaims(
                new List<Claim>
           {
               new Claim(AccessToken, context.AccessToken),
               new Claim(UserId, context.Id),
               new Claim(Email, context.Email ??""),
               new Claim(FirstName, context.FirstName),
               new Claim(LastName, context.LastName)
           });

            return base.Authenticated(context);
        }
    }
}