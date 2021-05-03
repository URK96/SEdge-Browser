using SEdgeBrowser.Models;
using SEdgeBrowser.Services;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml.Controls;

namespace SEdgeBrowser.Views
{
    public sealed partial class SettingPage : Page
    {
        private string homepageURL;

        public SettingPage()
        {
            InitializeComponent();

            homepageURL = SettingProvider.Load<string>(SettingKeys.HomepageURL);

            InitControls();
        }

        private void InitControls()
        {
            Setting_HomepageURL_TextBox.Text = homepageURL;
        }

        private void CheckHomepageURL()
        {
            Setting_HoempageURL_ApplyButton.IsEnabled = !homepageURL.Equals(Setting_HomepageURL_TextBox.Text);
        }

        private void Setting_HomepageURL_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckHomepageURL();
        }

        private void Setting_HoempageURL_ApplyButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            SettingProvider.Save(SettingKeys.HomepageURL, Setting_HomepageURL_TextBox.Text);

            homepageURL = SettingProvider.Load<string>(SettingKeys.HomepageURL);

            CheckHomepageURL();
        }
    }
}
