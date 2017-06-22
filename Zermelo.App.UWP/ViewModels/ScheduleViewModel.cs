﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Zermelo.API.Exceptions;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Models;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        IZermeloService _zermelo;
        IInternetConnectionService _internet;

        public ScheduleViewModel(IZermeloService zermelo, IInternetConnectionService internet)
        {
            _zermelo = zermelo;
            _internet = internet;

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

            var date = new DateTimeOffset(2017, 6, 16, 0, 0, 0, new TimeSpan(1, 0, 0));
            IDisposable subscription = _zermelo.GetSchedule(date, date.AddDays(1))
                .ObserveOnDispatcher()
                .Subscribe(
                    a => Appointments.MorphInto(a.OrderBy(x => x.Start).ToList()),
                    ex => ExceptionHelper.HandleException(ex, nameof(ScheduleViewModel),
                            m => new MessageDialog(m, "Error").ShowAsync()),
                    () => IsLoading = false
            );
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

        public API.Models.User User { get; }
    }

    public class ScheduleItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GroupsTemplate { get; set; }
        public DataTemplate TeachersTemplate { get; set; }
        public API.Models.User User { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (User.IsEmployee ?? false)
                return GroupsTemplate;
            else
                return TeachersTemplate;
        }
    }
}
