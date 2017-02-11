using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Autofac;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.Models;
using Zermelo.App.UWP.ViewModels;

namespace Zermelo.App.UWP.Views
{
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            this.InitializeComponent();

            ViewModel = (App.Current as App).Container.Resolve<SettingsViewModel>();
        }

        public SettingsViewModel ViewModel { get; }

        private void AboutActionsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            (e.ClickedItem as AboutAction)?.Action.Invoke();
        }
    }
}
