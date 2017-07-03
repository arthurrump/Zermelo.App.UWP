using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Microsoft.HockeyApp;
using Template10.Common;
using Template10.Services.NavigationService;
using Template10.Services.SettingsService;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Zermelo.App.UWP.Announcements;
using Zermelo.App.UWP.Login;
using Zermelo.App.UWP.Schedule;
using Zermelo.App.UWP.Services;
using Zermelo.App.UWP.Settings;
using Zermelo.App.UWP.Shell;

namespace Zermelo.App.UWP
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        bool _authenticated;

        public App()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<SettingsHelper>().As<ISettingsHelper>();
            builder.RegisterType<Services.SettingsService>().As<Services.ISettingsService>().SingleInstance();

            builder.RegisterType<InternetConnectionService>().As<IInternetConnectionService>();

            builder.RegisterType<AkavacheService>().As<ICacheService>();
            builder.RegisterType<ZermeloService>().AsSelf();
            builder.RegisterType<CachedZermeloService>().As<IZermeloService>();
            builder.RegisterType<ZermeloAuthenticationService>().As<IZermeloAuthenticationService>();

            builder.RegisterType<AnnouncementsViewModel>().AsSelf();
            builder.RegisterType<LoginViewModel>().AsSelf();
            builder.RegisterType<ScheduleViewModel>().AsSelf();
            builder.RegisterType<SettingsViewModel>().AsSelf();

            Container = builder.Build();

            InitializeComponent();
        }

        public IContainer Container { get; set; }

        public override Task OnInitializeAsync(IActivatedEventArgs args)
        {
#if !DEBUG
            HockeyClient.Current.Configure(Secrets.HockeyAppAppId);
            MobileCenter.Start(Secrets.MobileCenterAppSecret, typeof(Analytics), typeof(Crashes));
#endif

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

            var settings = Container.Resolve<Services.ISettingsService>();
            if (string.IsNullOrEmpty(settings.School) || string.IsNullOrEmpty(settings.Token))
                _authenticated = false;
            else
                _authenticated = true;            

            return Task.CompletedTask;
        }

        public override Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if (_authenticated)
                NavigationService.Navigate(typeof(ScheduleView));
            else
                NavigationService.Navigate(typeof(LoginView));

            return Task.CompletedTask;
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            if (_authenticated)
                return GetAuthenticatedRootElement();
            else
                return GetLoginRootElement();
        }

        public UIElement GetAuthenticatedRootElement()
        {
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ShellView(nav);
        }

        public void StartAuthenticated()
        {
            WindowWrapper.Current().NavigationServices.Clear();
            Window.Current.Content = GetAuthenticatedRootElement();
            NavigationService.Navigate(typeof(ScheduleView));
        }

        public UIElement GetLoginRootElement()
        {
            var frame = new Frame();
            var nav = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude, frame);
            return frame;
        }

        public void StartLogin()
        {
            WindowWrapper.Current().NavigationServices.Clear();
            Window.Current.Content = GetLoginRootElement();
            NavigationService.Navigate(typeof(LoginView));
        }

        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            switch (page)
            {
                case LoginView _:
                    return Container.Resolve<LoginViewModel>();
                case ScheduleView _:
                    return Container.Resolve<ScheduleViewModel>();
                case AnnouncementsView _:
                    return Container.Resolve<AnnouncementsViewModel>();
                case SettingsView _:
                    return Container.Resolve<SettingsViewModel>();
                default:
                    return base.ResolveForPage(page, navigationService);
            }
        }
    }
}
