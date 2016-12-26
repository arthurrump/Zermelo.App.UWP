using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zermelo.App.UWP.ViewModels
{
    public class SettingsViewModel : MVVM.ViewModelBase
    {
        public SettingsViewModel()
        {

        }

        // Account
        public string AuthCode { get; set; } // Not changing, no need to be reactive
        public string School { get; set; } // Not changing, no need to be reactive

        // Personalization
        [Reactive]
        public bool ShowGroups { get; set; }
    }
}
