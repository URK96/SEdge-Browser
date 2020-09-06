using SEdgeBrowser.Helper;
using SEdgeBrowser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEdgeBrowser.Services
{
    public static class HistoryDataService
    {
        public static List<History> urlHistory = new List<History>();

        public static void AddItem(string title, string url)
        {
            urlHistory.Add(new History()
            {
                Id = urlHistory.Count + 1,
                Title = title,
                Url = url,
                Favicon = FaviconHelper.GetFavicon(FaviconHelper.GetAuthority(url)),
                VisitDateTime = DateTime.Now
            });
        }

        public static void RemoveItems(int position, int count)
        {
            if (urlHistory.Count <= 0)
            {
                return;
            }

            urlHistory.RemoveRange(position, count);
        }
    }
}
