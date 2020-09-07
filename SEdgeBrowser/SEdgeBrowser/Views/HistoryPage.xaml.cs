using SEdgeBrowser.Models;
using SEdgeBrowser.Services;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml.Controls;

namespace SEdgeBrowser.Views
{
    public sealed partial class HistoryPage : Page
    {
        public IEnumerable<History> HistoryList { get; set; }

        public HistoryPage()
        {
            InitializeComponent();

            HistoryList = HistoryDataService.urlHistory.ToArray().OrderByDescending(x => x.Id);
        }

        private void HistoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }

            var item = e.AddedItems.First() as History;

            WebViewService.NavigateURL(item.Url);

            HistoryListView.SelectedItem = null;
        }
    }
}
