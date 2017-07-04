using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.Schedule
{
    public class ScheduleViewModel : ViewModelBase
    {
        IZermeloService _zermelo;
        IInternetConnectionService _internet;

        string _code;

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

            Refresh = new DelegateCommand(GetAppointments);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter != null)
                (Type, _code) = ((ScheduleType, string))parameter;
            else
                (Type, _code) = (ScheduleType.Student, "~me");

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Date))
                    GetAppointments();
            };

            if (!IsLoading)
                GetAppointments();

            PreloadAppointments();
        }

        private void GetAppointments()
        {
            IsLoading = true;

            if (!_internet.IsConnected())
            {
                new MessageDialog("Je hebt op dit moment geen internetverbinding. De weergegeven informatie kan verouderd zijn.", "Geen internetverbinding").ShowAsync();
            }

            IObservable<IEnumerable<Appointment>> observable;

            switch (Type)
            {
                case ScheduleType.Student:
                case ScheduleType.Employee:
                    observable = _zermelo.GetSchedule(Date.Date, Date.Date.AddDays(1), _code);
                    break;
                case ScheduleType.Group:
                    throw new NotImplementedException();
                case ScheduleType.Location:
                    throw new NotImplementedException();
                default:
                    observable = null;
                    break;
            }

            IDisposable subscription = observable
                .ObserveOnDispatcher()
                .Subscribe(
                    a => Appointments.MorphInto(
                        a.GroupBy(x => x.Start).OrderBy(x => x.Key)
                         .Select(x => new ScheduleRow(x.Key, x.OrderBy(y => y.Status)))
                    ),
                    ex => ExceptionHelper.HandleException(ex, nameof(ScheduleViewModel),
                            m => new MessageDialog(m, "Error").ShowAsync()),
                    () => IsLoading = false
            );
        }

        private void PreloadAppointments()
        {
            if (_internet.IsConnected())
            {
                for (int i = 1; i < 8; i++)
                {
                    var day = Date.Date.AddDays(i);
                    if (day.DayOfWeek == DayOfWeek.Saturday || day.DayOfWeek == DayOfWeek.Sunday)
                        break;

                    _zermelo.GetSchedule(day.Date, day.Date.AddDays(1), _code).Subscribe(
                        _ => { }, 
                        ex => ExceptionHelper.HandleException(ex, $"{nameof(ScheduleViewModel)}.Preload", _ => { })
                    );
                }
            }
        }

        ScheduleType _type;
        public ScheduleType Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged();
            }
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

        ObservableCollection<ScheduleRow> appointments = new ObservableCollection<ScheduleRow>();
        public ObservableCollection<ScheduleRow> Appointments
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
    }
}
