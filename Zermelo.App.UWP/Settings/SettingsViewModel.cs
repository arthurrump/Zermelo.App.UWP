using System;
using System.Reactive.Linq;
using Microsoft.Services.Store.Engagement;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.Settings
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

            LogOut = new DelegateCommand(async () =>
            {
                settings.School = null;
                settings.Token = null;
                await cache.ClearCache();
                (App.Current as App).StartLogin();
            });
        }

        // Account
        string user = "";
        public string User => user;
        public string School => _settings.School;

        public DelegateCommand LogOut { get; }

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

        // About
        public string Version => $"{Package.Current.Id.Version.Major}.{Package.Current.Id.Version.Minor}.{Package.Current.Id.Version.Build}";

        public AboutAction[] AboutActions =>
            new AboutAction[] {
                new AboutAction("Geef feedback", (Symbol)59705,
                    async () => {
                        if (StoreServicesFeedbackLauncher.IsSupported())
                            await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();
                        else
                            // Using this email will make it appear in the HockeyApp feedback section
                            await Launcher.LaunchUriAsync(new Uri("mailto:feedback@schoolrooster.arthurrump.com"));
                    }),
                new AboutAction("Beoordeel in de Store", Symbol.SolidStar, 
                    async () => await Launcher.LaunchUriAsync(new Uri($"ms-windows-store://review/?ProductId={CurrentApp.AppId}"))),
                new AboutAction("Bekijk op GitHub", Symbol.Link, 
                    async () => await Launcher.LaunchUriAsync(new Uri("https://github.com/arthurrump/Zermelo.App.UWP"))),
                new AboutAction("Open source licenties", Symbol.List,
                    async () => await Launcher.LaunchUriAsync(new Uri("https://github.com/arthurrump/Zermelo.App.UWP/blob/master/NOTICE.md")))
            };
    }
}
