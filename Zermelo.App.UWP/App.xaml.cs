using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Zermelo.App.UWP
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            var nav = (NavigationService)NavigationServiceFactory(BackButton.Attach, ExistingContent.Include);
            Window.Current.Content = new Views.Shell(nav);
            return Task.FromResult<object>(null);
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            NavigationService.Navigate(typeof(Views.ScheduleView));
            return Task.FromResult<object>(null);
        }
    }
}
