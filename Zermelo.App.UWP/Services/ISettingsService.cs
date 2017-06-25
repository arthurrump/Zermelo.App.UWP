using System.ComponentModel;

namespace Zermelo.App.UWP.Services
{
    public interface ISettingsService : INotifyPropertyChanged
    {
        //Account
        string School { get; set; }
        string Token { get; set; }
    }
}
