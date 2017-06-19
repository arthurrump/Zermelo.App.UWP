using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Zermelo.API.Exceptions;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Models;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        ISettingsService _settings;
        IZermeloService _zermelo;
        IInternetConnectionService _internet;

        public ScheduleViewModel(ISettingsService settings, IZermeloService zermelo, IInternetConnectionService internet)
        {
            _settings = settings;
            _zermelo = zermelo;
            _internet = internet;

            GetAppointments();

            Refresh = new DelegateCommand(GetAppointments);

            _settings.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_settings.ShowGroups))
                    RaisePropertyChanged(nameof(GroupsVisibility));
            };
        }

        private void GetAppointments()
        {
            IsLoading = true;

            if (!_internet.IsConnected())
            {
                new MessageDialog("Je hebt op dit moment geen internetverbinding. De weergegeven informatie kan verouderd zijn.", "Geen internetverbinding").ShowAsync();
            }

            IDisposable subscription = _zermelo.GetSchedule(DateTimeOffset.Now.Date, DateTimeOffset.Now.Date.AddDays(1))
                .ObserveOnDispatcher()
                .Subscribe(
                    a => Appointments.MorphInto(a.OrderBy(x => x.Start).ToList()),
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

        public Visibility GroupsVisibility => 
            _settings.ShowGroups ? Visibility.Visible : Visibility.Collapsed;

        ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>();
        public ObservableCollection<Appointment> Appointments
        {
            get => appointments;
            set
            {
                appointments = value;
                RaisePropertyChanged();
            }
        }

        Announcement selectedAppointment;
        public Announcement SelectedAppointment
        {
            get => selectedAppointment;
            set
            {
                selectedAppointment = value;
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

        public string CurrentDate => DateTimeOffset.Now.ToString("D");
    }
}
