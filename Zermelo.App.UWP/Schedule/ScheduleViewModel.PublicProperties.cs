using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NodaTime;
using Template10.Mvvm;

namespace Zermelo.App.UWP.Schedule
{
    public partial class ScheduleViewModel
    {
        string _header = "Rooster";
        public string Header
        {
            get => _header;
            set => Set(ref _header, value);
        }

        ScheduleType _type;
        public ScheduleType Type
        {
            get => _type;
            set => Set(ref _type, value);
        }

        DateInterval _currentWeek;
        public DateInterval CurrentWeek
        {
            get => _currentWeek;
            set => Set(ref _currentWeek, value);
        }

        LocalDate _currentDate;
        public LocalDate CurrentDate
        {
            get => _currentDate;
            set => Set(ref _currentDate, value);
        }
        
        ObservableCollection<ScheduleDay> _days = new ObservableCollection<ScheduleDay>();
        public ObservableCollection<ScheduleDay> Days
        {
            get => _days;
            set => Set(ref _days, value);
        }

        Appointment _selectedAppointment;
        public Appointment SelectedAppointment
        {
            get => _selectedAppointment;
            set => Set(ref _selectedAppointment, value);
        }

        public DelegateCommand RefreshCommand { get; }

        bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        bool _isModal = false;
        public bool IsModal
        {
            get => _isModal;
            set => Set(ref _isModal, value);
        }

        bool _isClosable;
        public bool IsClosable
        {
            get => _isClosable;
            set => Set(ref _isClosable, value);
        }

        public DelegateCommand CloseCurrentViewCommand { get; }
    }
}
