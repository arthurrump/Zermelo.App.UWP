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
    public sealed partial class ScheduleView : Page
    {
        public ScheduleView()
        {
            this.InitializeComponent();

            ViewModel = (App.Current as App).Container.Resolve<ScheduleViewModel>();
        }

        public ScheduleViewModel ViewModel { get; }
    }

    public class ScheduleItemStyleSelector : StyleSelector
    {
        public Style NormalStyle { get; set; }
        public Style NewStyle { get; set; }
        public Style CancelledStyle { get; set; }
        public Style ModifiedStyle { get; set; }

        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if (item is Appointment a)
            {
                if (a.Status == AppointmentStatus.New)
                    return NewStyle;
                else if (a.Status == AppointmentStatus.Cancelled)
                    return CancelledStyle;
                else if (a.Status == AppointmentStatus.Invalid || a.Status == AppointmentStatus.MovedValid)
                    return ModifiedStyle;
                else
                    return NormalStyle;
            }

            return base.SelectStyleCore(item, container);
        }
    }
}
