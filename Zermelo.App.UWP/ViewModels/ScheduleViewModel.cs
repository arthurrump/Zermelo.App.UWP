using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Zermelo.API.Models;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        ISettingsService _settings;
        IZermeloService _zermelo;

        public ScheduleViewModel(ISettingsService settings, IZermeloService zermelo)
        {
            _settings = settings;
            _zermelo = zermelo;

            _settings.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_settings.ShowGroups))
                    RaisePropertyChanged(nameof(GroupsVisibility));
            };
        }

        public Visibility GroupsVisibility => 
            _settings.ShowGroups ? Visibility.Visible : Visibility.Collapsed;

        IEnumerable<Appointment> appointments;
        public IEnumerable<Appointment> Appointments
        {
            get => appointments;
            set
            {
                appointments = value;
                RaisePropertyChanged();
            }
        }
    }
}
