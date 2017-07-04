using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Schedule
{
    public class ScheduleItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GroupsTemplate { get; set; }
        public DataTemplate TeachersTemplate { get; set; }
        public ScheduleType ScheduleType { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (ScheduleType == ScheduleType.Employee)
                return GroupsTemplate;
            else
                return TeachersTemplate;
        }
    }
}
