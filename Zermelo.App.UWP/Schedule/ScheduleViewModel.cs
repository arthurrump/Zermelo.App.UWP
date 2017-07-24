using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Analytics;
using NodaTime;
using Template10.Common;
using Template10.Mvvm;
using Template10.Utils;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Services;
using Zermelo.App.UWP.Shell;

namespace Zermelo.App.UWP.Schedule
{
    public partial class ScheduleViewModel : ViewModelBase
    {
        IZermeloService _zermelo;
        IInternetConnectionService _internet;

        string _code;
        CompositeDisposable _subscriptions = new CompositeDisposable();
        MultiOpLoadingStatus _loading = new MultiOpLoadingStatus();

        public ScheduleViewModel(IZermeloService zermelo, IInternetConnectionService internet)
        {
            _zermelo = zermelo;
            _internet = internet;

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(CurrentDate))
                    OnCurrentDateChanged();
                else if (e.PropertyName == nameof(CurrentWeek))
                    OnCurrentWeekChanged();
            };

            _loading.LoadingChanged += (sender, isLoading) => IsLoading = isLoading;

            var date = LocalDate.FromDateTime(DateTime.Now);
            if (date.DayOfWeek >= IsoDayOfWeek.Saturday)
                date = date.Next(IsoDayOfWeek.Monday);
            CurrentDate = date;

            RefreshCommand = new DelegateCommand(GetAppointments);
            CloseCurrentViewCommand = new DelegateCommand(CloseCurrentView);
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            IsLoading = true;

            if (parameter is string p && !string.IsNullOrEmpty(p) && p != "~me")
            {
                Type = (ScheduleType)int.Parse(p.Substring(0, 1));
                _code = p.Substring(1);
                IsClosable = true;
            }
            else
            {
                _code = "~me";
                _loading.AddLoadingOperation();
                var user = await _zermelo.GetCurrentUser();
                _loading.FinishLoadingOperation();
                Type = user?.IsEmployee ?? false ? ScheduleType.Employee : ScheduleType.Student;
            }

            if (SessionState.ContainsKey(_code))
            {
                var s = (ScheduleViewState)SessionState[_code];
                CurrentDate = s.CurrentDate;
                Type = s.Type;
                SelectedAppointment = s.SelectedAppointment;
                IsModal = s.IsModal;
            }

            await SetHeader(Type, _code);
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            if (SessionState.ContainsKey(_code))
                SessionState.Remove(_code);

            SessionState.Add(_code, new ScheduleViewState
            {
                CurrentDate = CurrentDate,
                Type = Type,
                SelectedAppointment = SelectedAppointment,
                IsModal = IsModal
            });

            _subscriptions.Dispose();
        }

        void OnCurrentDateChanged()
        {
            if (!CurrentWeek?.Contains(CurrentDate) ?? true)
            {
                CurrentWeek = new DateInterval(
                    CurrentDate.DayOfWeek == IsoDayOfWeek.Monday ? CurrentDate : CurrentDate.Previous(IsoDayOfWeek.Monday),
                    CurrentDate.DayOfWeek == IsoDayOfWeek.Friday ? CurrentDate : CurrentDate.Next(IsoDayOfWeek.Friday)
                );
            }
        }

        void OnCurrentWeekChanged()
        {
            var days = new ObservableCollection<ScheduleDay>();
            for (var day = CurrentWeek.Start; day <= CurrentWeek.End; day += Period.FromDays(1))
            {
                days.Add(new ScheduleDay(day, new ObservableCollection<ScheduleRow>()));
            }

            Days = days;

            GetAppointments();
        }

        IObservable<IEnumerable<Appointment>> GetAppointmentsObservable(ScheduleType type, LocalDate date, string code)
        {
            switch (type)
            {
                case ScheduleType.Student:
                case ScheduleType.Employee:
                    return _zermelo.GetSchedule(date, code);
                case ScheduleType.Group:
                    return _zermelo.GetScheduleForGroup(date, code);
                case ScheduleType.Location:
                    return _zermelo.GetScheduleForLocation(date, code);
                default:
                    Analytics.TrackEvent("Type is not a ScheduleType", new Dictionary<string, string>
                    {
                        { "Location", $"{nameof(ScheduleViewModel)}.{nameof(GetAppointmentsObservable)}" },
                        { "Type", ((int)type).ToString() }
                    });
                    return Observable.Empty<IEnumerable<Appointment>>();
            }
        }

        void GetAppointments()
        {
            if (!_internet.IsConnected())
            {
                new MessageDialog("Je hebt op dit moment geen internetverbinding. De weergegeven informatie kan verouderd zijn.", "Geen internetverbinding").ShowAsync();
            }

            for (var day = CurrentWeek.Start; day <= CurrentWeek.End; day += Period.FromDays(1))
            {
                _loading.AddLoadingOperation();
                _subscriptions.Add(
                    GetAppointmentsObservable(Type, day, _code)
                        .ObserveOnDispatcher()
                        .Subscribe(new AppointmentsObserver(Days.Single(x => x.Date == day).Appointments, _loading))
                );
            }
        }

        async Task SetHeader(ScheduleType type, string code)
        {
            if (code != "~me")
            {
                _loading.AddLoadingOperation();
                switch (type)
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
                        var group = await _zermelo.GetGroup(_code);
                        Header = $"Rooster van {group.ExtendedName}";
                        break;
                    case ScheduleType.Location:
                        var location = await _zermelo.GetLocation(_code);
                        Header = $"Rooster van {location.Name}";
                        break;
                }
                _loading.FinishLoadingOperation();
            }
            else
            {
                Header = "Mijn rooster";
            }
        }

        void CloseCurrentView()
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
            var navItems = ShellView.Instance.HamburgerPrimaryButtons;
            navItems.Remove(navItems.Single(x => x.PageType == typeof(ScheduleView) && (string)x.PageParameter == param));
        }
    }
}
