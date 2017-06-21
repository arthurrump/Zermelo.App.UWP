using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Analytics;
using Template10.Mvvm;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        Stopwatch _stopwatch;

        ISettingsService _settings;
        IZermeloAuthenticationService _authService;

        public LoginViewModel(ISettingsService settings, IZermeloAuthenticationService authService)
        {
            _stopwatch = new Stopwatch();

            _settings = settings;
            _authService = authService;

            LogIn = new DelegateCommand(async () =>
            {
                var auth = await _authService.GetAuthentication(School, Code);
                _settings.Token = auth.Token;

                _stopwatch.Stop();
                Analytics.TrackEvent("LogIn", new Dictionary<string, string>
                {
                    { "TimeElapsed (ms)", _stopwatch.ElapsedMilliseconds.ToString() }
                });

                (App.Current as App).StartAuthenticated();
            });
        }

        public DelegateCommand LogIn { get; }

        public string School
        {
            get => _settings.School;
            set
            {
                _settings.School = value;
                RaisePropertyChanged();
            }
        }

        string code;
        public string Code
        {
            get => code;
            set
            {
                code = value;
                RaisePropertyChanged();
            }
        }

        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            _stopwatch.Start();
            return Task.CompletedTask;
        }
    }
}
