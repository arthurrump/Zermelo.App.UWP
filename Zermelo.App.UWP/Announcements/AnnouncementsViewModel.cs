using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Template10.Mvvm;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Services;
using Windows.UI.Popups;

namespace Zermelo.App.UWP.Announcements
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

            IDisposable subscription = _zermelo.GetAnnouncements()
                .ObserveOnDispatcher()
                .Subscribe(
                    a => Announcements.MorphInto(a.OrderBy(x => x.Title).ToList()),
                    ex => ExceptionHelper.HandleException(ex, nameof(AnnouncementsViewModel), 
                            m => new MessageDialog(m, "Error").ShowAsync()),
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
