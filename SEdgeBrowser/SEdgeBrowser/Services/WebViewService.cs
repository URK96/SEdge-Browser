using Microsoft.UI.Xaml.Controls;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace SEdgeBrowser.Services
{
    public static class WebViewService
    {
        public static WebView2 webView;

        private static string CheckURL(string url) => url.StartsWith("http") ? url : $"http://{url}";

        public static void NavigateURL(string url)
        {
            webView.CoreWebView2.Navigate(CheckURL(url));
        }
    }
}
