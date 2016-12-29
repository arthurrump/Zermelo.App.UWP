using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        ISettingsService _settings;

        public SettingsViewModel(ISettingsService settings)
        {
            _settings = settings;
        }

        // Account
        public string AuthCode { get; } // Not changing, no need to be reactive
        public string School { get; } // Not changing, no need to be reactive

        // Personalization
        public bool ShowGroups
        {
            get => _settings.ShowGroups;
            set
            {
                _settings.ShowGroups = value;
                RaisePropertyChanged();
            }
        }

        // About
        public string Version => $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Revision}";
    }
}
