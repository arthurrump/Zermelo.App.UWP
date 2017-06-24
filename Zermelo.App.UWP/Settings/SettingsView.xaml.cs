﻿using Autofac;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Zermelo.App.UWP.Settings
{
    public sealed partial class SettingsView : Page
    {
        public SettingsView()
        {
            this.InitializeComponent();

            ViewModel = (App.Current as App).Container.Resolve<SettingsViewModel>();

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        public SettingsViewModel ViewModel { get; }

        private void AboutActionsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            (e.ClickedItem as AboutAction)?.Action.Invoke();
        }
    }
}
