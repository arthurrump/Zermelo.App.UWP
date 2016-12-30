using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Zermelo.App.UWP.ViewModels;
using Template10.Services.SettingsService;
using Zermelo.App.UWP.Services;

namespace Zermelo.App.UWP
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
        }

        public IContainer Container { get; set; }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                Window.Current.SizeChanged += async (sender, e) =>
                {
                    var statusBar = StatusBar.GetForCurrentView();
                    if (ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape)
                        await statusBar.HideAsync();
                    else
                        await statusBar.ShowAsync();
                };
            }

            var builder = new ContainerBuilder();

            builder.RegisterType<SettingsHelper>().As<ISettingsHelper>();
            builder.RegisterType<Services.SettingsService>().As<Services.ISettingsService>().SingleInstance();

            builder.RegisterType<ZermeloService>().AsSelf();
            builder.RegisterType<CachedZermeloService>().As<IZermeloService>();

            builder.RegisterType<AnnouncementsViewModel>().AsSelf();
            builder.RegisterType<ScheduleViewModel>().AsSelf();
            builder.RegisterType<SettingsViewModel>().AsSelf();

            Container = builder.Build();

            // Set up the hamburger menu shell
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
