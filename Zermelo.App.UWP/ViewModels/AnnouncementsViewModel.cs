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
using Zermelo.API.Exceptions;

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

            IDisposable subscription = _zermelo.GetAnnouncements()
                .ObserveOnDispatcher()
                .Subscribe(
                    a => Announcements.MorphInto(a.OrderBy(x => x.Title).ToList()),
                    ex =>
                    {
                        switch (ex)
                        {
                            case ZermeloHttpException e:
                                if (e.StatusCode == 404)
                                    new MessageDialog("De opgevraagde gegevens zijn niet gevonden in Zermelo.").ShowAsync();
                                else if (e.StatusCode == 403)
                                    new MessageDialog("Je hebt niet voldoende rechten om deze informatie te bekijken.").ShowAsync();
                                else
                                    new MessageDialog($"Er is iets fout gegaan. Zermelo geeft de volgende foutmelding: {e.StatusCode} {e.Status}").ShowAsync();
                                break;
                            default:
                                new MessageDialog("Er is iets fout gegaan. De ontwikkelaar wordt op de hoogte gesteld.").ShowAsync();
                                // TODO: implement error logging
                                break;
                        }
                    },
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
