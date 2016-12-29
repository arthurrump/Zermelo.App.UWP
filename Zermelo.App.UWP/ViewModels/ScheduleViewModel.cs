using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.ViewModels
{
    public class ScheduleViewModel : ViewModelBase
    {
        ISettingsService _settings;

        public ScheduleViewModel(ISettingsService settings)
        {
            _settings = settings;

            _settings.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_settings.ShowGroups))
                    RaisePropertyChanged(nameof(GroupsVisibility));
            };
        }

        public Visibility GroupsVisibility => 
            _settings.ShowGroups ? Visibility.Visible : Visibility.Collapsed;
    }
}
