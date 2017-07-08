using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Services;
using Zermelo.App.UWP.Shell;

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

            CloseCurrentView = new DelegateCommand(() =>
            {
                string param = $"{(int)Type}{_code}";

                Func<PageStackEntry, bool> isCurrentPage =
                    page => page.SourcePageType == typeof(ScheduleView) && ((string)page.Parameter).Contains(param);
                    
                while (NavigationService.Frame.BackStack.Any(isCurrentPage))
                {
                    NavigationService.Frame.BackStack.Remove(
                        NavigationService.Frame.BackStack.First(isCurrentPage)
                    );
                }

                NavigationService.GoBack();

                while (NavigationService.Frame.ForwardStack.Any(isCurrentPage))
                {
                    NavigationService.Frame.ForwardStack.Remove(
                        NavigationService.Frame.ForwardStack.First(isCurrentPage)
                    );
                }

                // Doing this before GoBack() causes a NullReferenceException inside Template10
                // I have no idea why.
                var navItems = ((ShellView)WindowWrapper.Current().Content).HamburgerPrimaryButtons;
                navItems.Remove(navItems.Single(x => x.PageType == typeof(ScheduleView) && (string)x.PageParameter == param));
            });
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (parameter is string p && !string.IsNullOrEmpty(p) && p != "~me")
            {
                Type = (ScheduleType)int.Parse(p.Substring(0, 1));
                _code = p.Substring(1);
                IsClosable = true;
            }
            else
            {
                _code = "~me";
                var user = await _zermelo.GetCurrentUser();
                Type = user?.IsEmployee ?? false ? ScheduleType.Employee : ScheduleType.Student;
            }

            if (SessionState.ContainsKey(_code))
            {
                var s = (ScheduleViewState)SessionState[_code];
                Date = s.Date;
                Type = s.Type;
                SelectedAppointment = s.SelectedAppointment;
                IsModal = s.IsModal;
            }

            if (_code != "~me")
            {
                switch (Type)
                {
                    case ScheduleType.Student:
                        var student = await _zermelo.GetStudent(_code);
                        Header = $"Rooster van {student.FullName}";
                        break;
                    case ScheduleType.Employee:
                        var employee = await _zermelo.GetEmployee(_code);
                        Header = $"Rooster van {employee.FullName}";
                        break;
                    case ScheduleType.Group:
                        throw new NotImplementedException();
                    case ScheduleType.Location:
                        throw new NotImplementedException();
                }
            }
            else
            {
                Header = "Mijn rooster";
            }

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Date))
                    GetAppointments();
            };

            GetAppointments();

            PreloadAppointments();
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            if (SessionState.ContainsKey(_code))
                SessionState.Remove(_code);

            SessionState.Add(_code, new ScheduleViewState
            {
                Date = Date,
                Type = Type,
                SelectedAppointment = SelectedAppointment,
                IsModal = IsModal
            });
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

        string _header = "Rooster";
        public string Header
        {
            get => _header;
            set
            {
                _header = value;
                RaisePropertyChanged();
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

        bool isModal = false;
        public bool IsModal
        {
            get => isModal;
            set
            {
                isModal = value;
                RaisePropertyChanged();
            }
        }

        bool isClosable;
        public bool IsClosable
        {
            get => isClosable;
            set
            {
                isClosable = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand CloseCurrentView { get; }
    }
}
