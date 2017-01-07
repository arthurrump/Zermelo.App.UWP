using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
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
            => _zermelo.GetAnnouncements()
                .Subscribe(a => Announcements = a);

        IEnumerable<Announcement> announcements;
        public IEnumerable<Announcement> Announcements
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
    }
}
