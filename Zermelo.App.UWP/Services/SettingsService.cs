using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.SettingsService;

namespace Zermelo.App.UWP.Services
{
    public class SettingsService : BindableBase, ISettingsService
    {
        ISettingsHelper _helper;

        public SettingsService(ISettingsHelper settingsHelper)
        {
            _helper = settingsHelper;
        }

        private T Read<T>(string key, T otherwise = default(T))
            => _helper.Read<T>(key, otherwise, SettingsStrategies.Roam);

        private void Write<T>(string key, T value) => _helper.Write(key, value, SettingsStrategies.Roam);

        //Account
        public string School
        {
            get => Read<string>(nameof(School));
            set
            {
                Write(nameof(School), value);
                RaisePropertyChanged();
            }
        }

        public string Token
        {
            get => Read<string>(nameof(Token));
            set
            {
                Write(nameof(Token), value);
                RaisePropertyChanged();
            }
        }
    }
}
