using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SEdgeBrowser
{
    public sealed partial class MainPage : Page
    {
        public string URL { get; set; }

        public MainPage()
        {
            InitializeComponent();

            //ApplicationView.GetForCurrentView().TitleBar.BackgroundColor = 

            string homeUrl = SettingProvider.Load<string>(SettingKeys.HomeURL);

            URL = string.IsNullOrWhiteSpace(homeUrl) ? "" : homeUrl;

            //MainWebView.Navigate(new Uri(URL));

            UpdateTitleStatus();
        }

        private string CheckURL(string url) => url.StartsWith("http") ? url : $"http://{url}";

        private void UpdateTitleStatus()
        {
            BackButton.IsEnabled = MainWebView.CanGoBack;
            ForwardButton.IsEnabled = MainWebView.CanGoForward;

            URL = MainWebView.Source.AbsoluteUri;

            ApplicationView.GetForCurrentView().Title = $"{MainWebView.DocumentTitle}";

            Bindings.Update();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.GoBack();
            UpdateTitleStatus();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.GoForward();
            UpdateTitleStatus();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(10);

            MainWebView.Refresh();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            //string homeUrl = SettingProvider.Load<string>(SettingKeys.HomeURL);

            //MainWebView.Navigate(new Uri(CheckURL(homeUrl)));
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void MainWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            WebLoadingProgressBar.Value = 0;
            WebLoadingProgressBar.Visibility = Visibility.Visible;

            await Task.Delay(10);
        }

        private async void MainWebView_ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            RefreshButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = Visibility.Visible;

            UpdateTitleStatus();

            WebLoadingProgressBar.Value = 30;

            await Task.Delay(10);
        }

        private async void MainWebView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            WebLoadingProgressBar.Value = 70;

            await Task.Delay(10);
        }

        private async void MainWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            RefreshButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;

            WebLoadingProgressBar.Value = 100;

            await Task.Delay(1000);

            WebLoadingProgressBar.Visibility = Visibility.Collapsed;
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(10);

            MainWebView.Stop();

            RefreshButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;
        }

        private void URLTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                URL = (sender as TextBox).Text;

                MainWebView.Navigate(new Uri(CheckURL(URL)));
            }
        }

        private void MainWebView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            args.Handled = true;
            MainWebView.Navigate(args.Uri);
        }
    }
}
