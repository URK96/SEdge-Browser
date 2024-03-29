﻿using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using System.Collections.Generic;
using SEdgeBrowser.Services;
using SEdgeBrowser.Views;
using Windows.UI.Core;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

namespace SEdgeBrowser
{
    public sealed partial class MainPage : Page
    {
        public string URL { get; set; }
        
        private List<(string Name, string Url)> urlHistory = new List<(string Name, string Url)>();

        private bool isRefresh = false;

        public MainPage()
        {
            InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;

            WebViewService.webView = MainWebView;

            string homeUrl = SettingProvider.Load<string>(SettingKeys.HomepageURL);

            if (string.IsNullOrWhiteSpace(homeUrl))
            {
                homeUrl = "https://www.google.com";

                SettingProvider.Save(SettingKeys.HomepageURL, homeUrl);
            }

            URL = homeUrl;

            MainWebView.Source = new Uri(URL);
        }

        private void UpdateToolbarStatus(string url)
        {
            BackButton.IsEnabled = MainWebView.CanGoBack;
            ForwardButton.IsEnabled = MainWebView.CanGoForward;

            URL = url;

            Bindings.Update();
        }

        private void UpdateTitle()
        {
            ApplicationView.GetForCurrentView().Title = $"{MainWebView.CoreWebView2.DocumentTitle}";
        }


        #region Events


        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;

            if (MainWebView.CanGoBack)
            {
                MainWebView.GoBack();
            }
            else
            {
                Application.Current.Exit();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.GoBack();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            MainWebView.GoForward();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(10);

            MainWebView.Reload();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            WebViewService.NavigateURL(SettingProvider.Load<string>(SettingKeys.HomepageURL));
        }

        private async void MainWebView_NavigationStarting(WebView2 sender, CoreWebView2NavigationStartingEventArgs args)
        {
            WebLoadingProgressBar.Visibility = Visibility.Visible;
            RefreshButton.Visibility = Visibility.Collapsed;
            StopButton.Visibility = Visibility.Visible;

            MainSplitView.IsPaneOpen = false;

            await Task.Delay(10);
        }

        private async void MainWebView_NavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
        {
            RefreshButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;

            UpdateToolbarStatus(sender.Source.AbsoluteUri);
            UpdateTitle();

            if (!isRefresh)
            {
                HistoryDataService.AddItem(MainWebView.CoreWebView2.DocumentTitle, URL);
            }

            await Task.Delay(1000);

            WebLoadingProgressBar.Visibility = Visibility.Collapsed;
            isRefresh = false;
        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Delay(10);

            MainWebView.CoreWebView2.Stop();

            RefreshButton.Visibility = Visibility.Visible;
            StopButton.Visibility = Visibility.Collapsed;
        }

        private void URLTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                URL = (sender as TextBox).Text;

                Focus(FocusState.Programmatic);
                WebViewService.NavigateURL(URL);
            }
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuFlyoutItem);

            switch (item.Tag as string)
            {
                case "Setting":
                    MainSplitView.Pane = new SettingPage();
                    MainSplitView.IsPaneOpen = true;
                    break;
                case "Exit":
                    Application.Current.Exit();
                    break;
            }
        }

        /*private void BackButton_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var historyFlyout = new MenuFlyout();
            
            for (int i = 1; (urlHistory.Count >= i) && (i <= 10); ++i)
            {
                var history = urlHistory[urlHistory.Count - i];

                var item = new MenuFlyoutItem()
                {
                    Text = history.Name,
                    Tag = i,
                };
                item.Click += delegate
                {
                    int count = (int)item.Tag;
                    int index = urlHistory.Count - count;
                    string url = urlHistory[index].Url;

                    isHistoryClick = true;

                    BackButton.ContextFlyout = null;
                    NavigateURL(url);
                };

                historyFlyout.Items.Add(item);
            }

            //BackButton.Flyout = historyFlyout;
            BackButton.ContextFlyout = historyFlyout;
        }*/
        

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            MainSplitView.Pane = new HistoryPage();
            MainSplitView.IsPaneOpen = true;
        }

        private void MainSplitView_PaneClosed(SplitView sender, object args)
        {
            MainSplitView.Pane = null;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        #endregion
    }
}
