using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Autofac;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.ViewModels;
using Zermelo.App.UWP.Models;

namespace Zermelo.App.UWP.Views
{
    public sealed partial class AnnouncementsView : Page
    {
        public AnnouncementsView()
        {
            this.InitializeComponent();

            ViewModel = (App.Current as App).Container.Resolve<AnnouncementsViewModel>(); ;
        }

        public AnnouncementsViewModel ViewModel { get; }

        private void AnnouncementsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedAnnouncement = e.ClickedItem as Announcement;
            Modal.IsModal = true;
        }
    }
}
