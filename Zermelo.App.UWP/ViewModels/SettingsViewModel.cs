using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Services.Store.Engagement;
using Template10.Mvvm;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Zermelo.App.UWP.Models;
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

        public AboutAction[] AboutActions =>
            new AboutAction[] {
                new AboutAction("Geef feedback", (Symbol)59705,
                    async () => {
                        if (StoreServicesFeedbackLauncher.IsSupported())
                            await StoreServicesFeedbackLauncher.GetDefault().LaunchAsync();
                        else
                            // Using this email will make it appear in the HockeyApp feedback section
                            await Launcher.LaunchUriAsync(new Uri("mailto:support+3cbbb9d5373343ccba023d3df52640d9@feedback.hockeyapp.net"));
                    }),
                new AboutAction("Beoordeel in de Store", Symbol.SolidStar, 
                    async () => await Launcher.LaunchUriAsync(new Uri($"ms-windows-store://review/?ProductId={CurrentAppSimulator.AppId}"))),
                new AboutAction("Bekijk op GitHub", Symbol.Link, 
                    async () => await Launcher.LaunchUriAsync(new Uri("https://github.com/arthurrump/Zermelo.App.UWP"))),
                new AboutAction("Open source licenties", Symbol.List,
                    async () => await Launcher.LaunchUriAsync(new Uri("https://github.com/arthurrump/Zermelo.App.UWP/blob/master/NOTICE.md")))
            };
    }
}
