using System;
using System.Linq;
using Autofac;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Zermelo.App.UWP.Schedule
{
    public sealed partial class ScheduleView : Page
    {
        public ScheduleView()
        {
            this.InitializeComponent();

            ViewModel = (App.Current as App).Container.Resolve<ScheduleViewModel>();

            NavigationCacheMode = NavigationCacheMode.Enabled;

            CalendarView.SelectedDates.Add(ViewModel.Date);
        }

        public ScheduleViewModel ViewModel { get; }

        private void ScheduleListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedAppointment = e.ClickedItem as Appointment;
            Modal.IsModal = true;
        }

        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            ViewModel.Date = args.AddedDates.FirstOrDefault();
        }

        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            var day = args.Item.Date.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
                args.Item.IsBlackout = true;
        }
    }
}
