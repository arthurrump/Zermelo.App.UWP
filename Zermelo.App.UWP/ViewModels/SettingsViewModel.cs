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
        IZermeloService _zermelo;

        public SettingsViewModel(ISettingsService settings, IZermeloService zermelo)
        {
            _settings = settings;
            _zermelo = zermelo;

            _zermelo.GetCurrentUserAsync()
                .Subscribe(u =>
                {
                    user = $"{u.FullName} ({u.Code})";
                    RaisePropertyChanged(nameof(User));
                });
        }

        // Account
        string user = "";
        public string User => user;
        public string School => _settings.School;

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
