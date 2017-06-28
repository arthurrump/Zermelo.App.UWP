using Template10.Mvvm;
using Template10.Services.SettingsService;
using Windows.Storage;

namespace Zermelo.App.UWP.Services
{
    public class SettingsService : BindableBase, ISettingsService
    {
        ApplicationDataContainer _settings;

        public SettingsService()
        {
            _settings = ApplicationData.Current.RoamingSettings;
        }

        private T Read<T>(string key)
            => (T)_settings.Values[key];

        private void Write<T>(string key, T value) 
            => _settings.Values[key] = value;

        //Account
        public string School
        {
            get => Read<string>("Host");
            set
            {
                Write("Host", value);
                RaisePropertyChanged();
            }
        }

        public string Token
        {
            get => Read<string>("Token");
            set
            {
                Write("Token", value);
                RaisePropertyChanged();
            }
        }
    }
}
