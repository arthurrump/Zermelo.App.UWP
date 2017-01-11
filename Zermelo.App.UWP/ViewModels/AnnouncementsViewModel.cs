using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;
using Zermelo.API.Models;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.ViewModels
{
    public class AnnouncementsViewModel : ViewModelBase
    {
        IZermeloService _zermelo;

        public AnnouncementsViewModel(IZermeloService zermelo)
        {
            _zermelo = zermelo;

            GetAnnouncements();

            Refresh = new DelegateCommand(GetAnnouncements);
        }

        private void GetAnnouncements()
        {
            IsLoading = true;

            _zermelo.GetAnnouncements().Subscribe(
                a => Announcements = a.ToObservableCollection(),
                () => IsLoading = false
            );
        }

        ObservableCollection<Announcement> announcements;
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
