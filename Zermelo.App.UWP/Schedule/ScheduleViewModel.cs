using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Template10.Mvvm;
using Windows.UI.Popups;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.Schedule
{
    public class ScheduleViewModel : ViewModelBase
    {
        IZermeloService _zermelo;
        IInternetConnectionService _internet;

        public ScheduleViewModel(IZermeloService zermelo, IInternetConnectionService internet)
        {
            _zermelo = zermelo;
            _internet = internet;

            var date = DateTimeOffset.Now;
            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(2);
            else if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(1);
            Date = date;

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Date))
                    GetAppointments();
            };

            GetAppointments();

            Refresh = new DelegateCommand(GetAppointments);

            User = _zermelo.GetCurrentUser().GetAwaiter().GetResult();
        }

        private void GetAppointments()
        {
            IsLoading = true;

            if (!_internet.IsConnected())
            {
                new MessageDialog("Je hebt op dit moment geen internetverbinding. De weergegeven informatie kan verouderd zijn.", "Geen internetverbinding").ShowAsync();
            }

            IDisposable subscription = _zermelo.GetSchedule(Date.Date, Date.Date.AddDays(1))
                .ObserveOnDispatcher()
                .Subscribe(
                    a => Appointments.MorphInto(a.OrderBy(x => x.Start).ToList()),
                    ex => ExceptionHelper.HandleException(ex, nameof(ScheduleViewModel),
                            m => new MessageDialog(m, "Error").ShowAsync()),
                    () => IsLoading = false
            );
        }

        DateTimeOffset date;
        public DateTimeOffset Date
        {
            get => date;
            set
            {
                date = value;
                RaisePropertyChanged();
            }
        }

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

        Appointment selectedAppointment;
        public Appointment SelectedAppointment
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

        public API.Models.User User { get; }
    }
}
