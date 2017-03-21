using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJTaskManagerMobile.Helpers
{
    public class StorageHelper
    {
        /// <summary>
        /// Stored a setting, returns true if success
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public static bool StoreSetting(string key, object value, bool overwrite)
        {
            var appSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (overwrite || appSettings.Values.ContainsKey(key))
            {
                appSettings.Values[key] = value;
                
               // IsolatedStorageSettings.ApplicationSettings.Save();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get a setting from storage, returns default value if it does not exist
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSetting<T>(string key)
        {
            var appSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (appSettings.Values.ContainsKey(key))
            {
                return (T)appSettings.Values[key];
            }

            return default(T);
        }

        public static T GetSetting<T>(string key, T defaultVal)
        {
            var appSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (appSettings.Values.ContainsKey(key))
            {
                return (T)appSettings.Values[key];
            }

            return defaultVal;
        }

        public static void RemoveSetting(string key)
        {
            var appSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            appSettings.Values.Remove(key);
        }
    }
}
