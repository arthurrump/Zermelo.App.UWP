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

            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        ScheduleViewModel _viewModel;
        public ScheduleViewModel ViewModel => _viewModel ?? (_viewModel = (ScheduleViewModel)DataContext);

        private void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (CalendarView.SelectedDates.Count < 1)
                CalendarView.SelectedDates.Add(ViewModel.Date);
        }

        private void ScheduleListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedAppointment = e.ClickedItem as Appointment;
            Modal.IsModal = true;
        }

        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (args.AddedDates.Count > 0)
                ViewModel.Date = args.AddedDates.FirstOrDefault();
            else
                CalendarView.SelectedDates.Add(ViewModel.Date);
        }

        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            var day = args.Item.Date.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
                args.Item.IsBlackout = true;
        }
    }
}
