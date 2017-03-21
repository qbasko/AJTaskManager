using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJTaskManagerMobile.Helpers
{
    class AccountHelper
    {
        public static string GetCurrentUserId()
        {
            var userId = StorageHelper.GetSetting<string>(Common.Constants.UserId, null);
            return userId.Replace(Common.Constants.UserIdMsPrefix, String.Empty);
        }

        public static string GetUserFirstName()
        {
            return StorageHelper.GetSetting<string>(Common.Constants.UserFirstName);
        }

        public static string GetUserLastName()
        {
            return StorageHelper.GetSetting<string>(Common.Constants.UserLastName);
        }

        public static string GetUserEmail()
        {
            return StorageHelper.GetSetting<string>(Common.Constants.UserEmail);
        }
    }
}
