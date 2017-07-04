using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Zermelo.App.UWP.OtherSchedules
{
    public sealed partial class OtherSchedulesView : Page
    {
        public OtherSchedulesView()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        OtherSchedulesViewModel _viewModel;
        public OtherSchedulesViewModel ViewModel => _viewModel ?? (_viewModel = (OtherSchedulesViewModel)DataContext);
    }
}
