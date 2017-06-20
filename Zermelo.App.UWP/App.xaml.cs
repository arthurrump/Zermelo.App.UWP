﻿using Autofac;
using Microsoft.HockeyApp;
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
using Zermelo.API;

namespace Zermelo.App.UWP
{
    [Bindable]
    sealed partial class App : BootStrapper
    {
        bool _authenticated;

        public App()
        {
            HockeyClient.Current.Configure(Secrets.HockeyAppAppId);

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
                NavigationService.Navigate(typeof(Views.ScheduleView));
            else
                NavigationService.Navigate(typeof(Views.LoginView));

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
            return new Views.Shell(nav);
        }

        public void StartAuthenticated()
        {
            WindowWrapper.Current().NavigationServices.Clear();
            Window.Current.Content = GetAuthenticatedRootElement();
            NavigationService.Navigate(typeof(Views.ScheduleView));
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
            NavigationService.Navigate(typeof(Views.LoginView));
        }
    }
}
