using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile.Analytics;
using Template10.Mvvm;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP.Login
{
    public class LoginViewModel : ViewModelBase
    {
        Stopwatch _stopwatch;

        ISettingsService _settings;
        IZermeloAuthenticationService _authService;
        IInternetConnectionService _internet;

        public LoginViewModel(ISettingsService settings, IZermeloAuthenticationService authService, IInternetConnectionService internet)
        {
            _stopwatch = new Stopwatch();

            _settings = settings;
            _authService = authService;
            _internet = internet;

            LogIn = new DelegateCommand(async () => await logIn());

            _stopwatch.Start();
        }

        private async Task logIn()
        {
            if (string.IsNullOrWhiteSpace(School) || string.IsNullOrWhiteSpace(Code))
            {
                new MessageDialog("Vul een school en een code in.").ShowAsync();
                return;
            }

            if (!_internet.IsConnected())
            {
                new MessageDialog("Je hebt op dit moment geen internetverbinding. Je kunt dus niet ingelogd worden.", "Geen internetverbinding").ShowAsync();
                return;
            }

            InputEnabled = false;

            try
            {
                var auth = await _authService.GetAuthentication(School, Code);
                _settings.Token = auth.Token;

                _stopwatch.Stop();
                Analytics.TrackEvent("LogIn", new Dictionary<string, string>
                {
                    { "TimeElapsed (ms)", _stopwatch.ElapsedMilliseconds.ToString() }
                });

                (App.Current as App).StartAuthenticated();
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex, nameof(LoginViewModel), 
                    m => new MessageDialog(m).ShowAsync(),
                    zermelo404Text: "Deze school is niet gevonden. Let op dat je dezelfde naam gebruikt als in het .zportal.nl webadres.",
                    zermelo400Text: "De gebruikte code is incorrect. Controleer of je de code juist hebt overgenomen uit het Zermelo Portal.");
            }
            finally
            {
                InputEnabled = true;
            }
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

        bool inputEnabled = true;
        public bool InputEnabled
        {
            get => inputEnabled;
            set
            {
                inputEnabled = value;
                RaisePropertyChanged();
            }
        }
    }
}
