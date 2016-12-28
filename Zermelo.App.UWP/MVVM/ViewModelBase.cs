using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Template10.Common;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

namespace Zermelo.App.UWP.MVVM
{
    public class ViewModelBase : INavigable
    {
        public virtual Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            return Task.CompletedTask;
        }

        [JsonIgnore]
        public INavigationService NavigationService { get; set; }

        [JsonIgnore]
        public IDispatcherWrapper Dispatcher { get; set; }

        [JsonIgnore]
        public IStateItems SessionState { get; set; }
    }
}
