using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Zermelo.App.UWP.OtherSchedules
{
    public sealed partial class OtherSchedulesView : Page
    {
        public OtherSchedulesView()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        OtherSchedulesViewModel _viewModel;
        public OtherSchedulesViewModel ViewModel => _viewModel ?? (_viewModel = (OtherSchedulesViewModel)DataContext);

        private async void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                if (SearchButton.Focus(FocusState.Programmatic))
                {
                    await Task.Delay(200);
                    ViewModel.Search.Execute();
                }
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedSearchItem = e.ClickedItem as SearchItem;
            ViewModel.GoToSchedule.Execute();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Focus(FocusState.Programmatic);
        }
    }
}
