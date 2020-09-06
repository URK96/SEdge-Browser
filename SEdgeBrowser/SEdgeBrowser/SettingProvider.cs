using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SEdgeBrowser
{
    internal static class SettingProvider
    {
        private static ApplicationDataContainer localSettings => ApplicationData.Current.LocalSettings;

        internal static void Save<T>(string key, T value) => localSettings.Values[key] = value;
        internal static T Load<T>(string key)
        {
            try
            {
                return (T)localSettings.Values[key];
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
