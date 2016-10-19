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

            builder.RegisterType<AnnouncementsViewModel>();
            builder.RegisterType<ScheduleViewModel>();
            builder.RegisterType<SettingsViewModel>();

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

        // Automagically bind a viewmodel to a view using the following convention:
        // View: Namespaces.Views.SomeView => ViewModel: Namespaces.ViewModels.SomeViewModel
        // Returns null if the viewmodel type doesn't exist or isn't found in the container
        public override INavigable ResolveForPage(Page page, NavigationService navigationService)
        {
            string viewNamespace = page.GetType().Namespace;
            string viewName = page.GetType().Name;

            string vmNamespace = Regex.Replace(viewNamespace, "Views$", "ViewModels");
            string vmName = viewName + "Model";

            Type vmType = Type.GetType(vmNamespace + "." + vmName);

            if (vmType != null)
                return Container.ResolveOptional(vmType) as INavigable;

            return base.ResolveForPage(page, navigationService);
        }
    }
}
