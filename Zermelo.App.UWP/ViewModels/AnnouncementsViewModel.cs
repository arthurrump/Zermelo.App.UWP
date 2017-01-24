using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Xaml;
using Zermelo.App.UWP.Models;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Services;
using Windows.UI.Popups;

namespace Zermelo.App.UWP.ViewModels
{
    public class AnnouncementsViewModel : ViewModelBase
    {
        IZermeloService _zermelo;
        IInternetConnectionService _internet;

        public AnnouncementsViewModel(IZermeloService zermelo, IInternetConnectionService internet)
        {
            _zermelo = zermelo;
            _internet = internet;

            GetAnnouncements();

            Refresh = new DelegateCommand(GetAnnouncements);
        }

        private void GetAnnouncements()
        {
            IsLoading = true;
            
            if (!_internet.IsConnected())
            {
                new MessageDialog("Je hebt op dit moment geen internetverbinding. De weergegeven informatie kan verouderd zijn.", "Geen internetverbinding").ShowAsync();
            }

            _zermelo.GetAnnouncements()
                .ObserveOnDispatcher()
                .Subscribe(
                    a => Announcements.MorphInto(a.OrderBy(x => x.Title)),
                    () => IsLoading = false
            );
        }

        ObservableCollection<Announcement> announcements = new ObservableCollection<Announcement>();
        public ObservableCollection<Announcement> Announcements
        {
            get => announcements;
            set
            {
                announcements = value;
                RaisePropertyChanged();
            }
        }

        Announcement selectedAnnouncement;
        public Announcement SelectedAnnouncement
        {
            get => selectedAnnouncement;
            set
            {
                selectedAnnouncement = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand Refresh { get; }

        bool isLoading;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                RaisePropertyChanged();
            }
        }
    }
}
