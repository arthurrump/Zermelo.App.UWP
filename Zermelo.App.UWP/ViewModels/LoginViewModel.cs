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
    public class LoginViewModel : ViewModelBase
    {
        ISettingsService _settings;
        IZermeloAuthenticationService _authService;

        public LoginViewModel(ISettingsService settings, IZermeloAuthenticationService authService)
        {
            _settings = settings;
            _authService = authService;

            LogIn = new DelegateCommand(async () =>
            {
                var auth = await _authService.GetAuthentication(School, Code);
                _settings.Token = auth.Token;
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
    }
}
