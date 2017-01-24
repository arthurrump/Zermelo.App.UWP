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

        public SettingsViewModel(ISettingsService settings, IZermeloService zermelo, ICacheService cache)
        {
            _settings = settings;

            zermelo.GetCurrentUser()
                .Subscribe(u =>
                {
                    user = $"{u.FullName} ({u.Code})";
                    RaisePropertyChanged(nameof(User));
                });

            ClearCache = new DelegateCommand(async () =>
            {
                ClearCacheButtonEnabled = false;
                await cache.ClearCache();
                ClearCacheButtonEnabled = true;
            });
        }

        // Account
        string user = "";
        public string User => user;
        public string School => _settings.School;

        public DelegateCommand ClearCache { get; }

        bool clearCacheButtonEnabled = true;
        public bool ClearCacheButtonEnabled
        {
            get => clearCacheButtonEnabled;
            set
            {
                clearCacheButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

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
