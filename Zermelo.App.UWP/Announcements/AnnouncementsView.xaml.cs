using Autofac;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Zermelo.App.UWP.Announcements
{
    public sealed partial class AnnouncementsView : Page
    {
        public AnnouncementsView()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        AnnouncementsViewModel _viewModel;
        public AnnouncementsViewModel ViewModel => _viewModel ?? (_viewModel = (AnnouncementsViewModel)DataContext);

        private void AnnouncementsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedAnnouncement = e.ClickedItem as Announcement;
            Modal.IsModal = true;
        }
    }
}
