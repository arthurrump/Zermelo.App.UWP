using Autofac;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Zermelo.App.UWP.Settings
{
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        SettingsViewModel _viewModel;
        public SettingsViewModel ViewModel => _viewModel ?? (_viewModel = (SettingsViewModel)DataContext);

        private void AboutActionsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            (e.ClickedItem as AboutAction)?.Action.Invoke();
        }
    }
}
