using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEdgeBrowser.Helper
{
    public static class FaviconHelper
    {
        public static string GetAuthority(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var dom) ? dom.Authority : url;
        }

        public static string GetFavicon(string domain)
        {
            return $"https://api.faviconkit.com/{domain}/144";
        }
    }
}
