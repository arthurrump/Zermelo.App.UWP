using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Schedule
{
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
