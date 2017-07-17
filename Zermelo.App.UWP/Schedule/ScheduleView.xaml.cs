using System;
using System.Linq;
using Autofac;
using NodaTime;
using NodaTime.Extensions;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Zermelo.App.UWP.Helpers;

namespace Zermelo.App.UWP.Schedule
{
    public sealed partial class ScheduleView : Page
    {
        public ScheduleView()
        {
            this.InitializeComponent();

            NavigationCacheMode = NavigationCacheMode.Disabled;
        }

        ScheduleViewModel _viewModel;
        public ScheduleViewModel ViewModel => _viewModel ?? (_viewModel = (ScheduleViewModel)DataContext);

        private void CalendarView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (CalendarView.SelectedDates.Count < 1)
                CalendarView.SelectedDates.Add(ViewModel.CurrentDate.ToMidnightDateTimeOffset());

            CalendarView.SelectedDatesChanged += CalendarView_SelectedDatesChanged;

            ViewModel.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName == nameof(ViewModel.CurrentDate))
                    ViewModel_SelectedDatesChanged();
            };
        }

        private void ScheduleListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedAppointment = e.ClickedItem as Appointment;
            Modal.IsModal = true;
        }

        private void CalendarView_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            if (args.AddedDates.Count > 0)
            {
                if (ViewModel.CurrentDate != args.AddedDates.FirstOrDefault().ToOffsetDateTime().Date)
                    ViewModel.CurrentDate = args.AddedDates.FirstOrDefault().ToOffsetDateTime().Date;
            }
            else
                CalendarView.SelectedDates.Add(ViewModel.CurrentDate.ToMidnightDateTimeOffset());

            CalendarFlyout.Hide();
        }

        private void ViewModel_SelectedDatesChanged()
        {
            if (!CalendarView.SelectedDates.Contains(ViewModel.CurrentDate.ToMidnightDateTimeOffset()))
            {
                CalendarView.SelectedDates.Clear();
                CalendarView.SelectedDates.Add(ViewModel.CurrentDate.ToMidnightDateTimeOffset());
            }
        }

        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender, CalendarViewDayItemChangingEventArgs args)
        {
            var day = args.Item.Date.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
                args.Item.IsBlackout = true;
        }
    }
}
