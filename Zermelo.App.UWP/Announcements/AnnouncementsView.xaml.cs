using Autofac;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Announcements
{
    public sealed partial class AnnouncementsView : Page
    {
        public AnnouncementsView()
        {
            this.InitializeComponent();

            ViewModel = (App.Current as App).Container.Resolve<AnnouncementsViewModel>();
        }

        public AnnouncementsViewModel ViewModel { get; }

        private void AnnouncementsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedAnnouncement = e.ClickedItem as Announcement;
            Modal.IsModal = true;
        }
    }
}
