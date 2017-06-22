using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zermelo.App.UWP.Services
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        //Account
        string School { get; set; }
        string Token { get; set; }
    }
}
