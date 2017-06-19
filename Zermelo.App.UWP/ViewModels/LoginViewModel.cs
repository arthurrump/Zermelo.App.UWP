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

        public LoginViewModel(ISettingsService settings)
        {
            _settings = settings;

            LogIn = new DelegateCommand(() =>
            {
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

        public string Token
        {
            get => _settings.Token;
            set
            {
                _settings.Token = value;
                RaisePropertyChanged();
            }
        }
    }
}
