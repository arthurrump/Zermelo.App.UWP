using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Schedule
{
    public class ScheduleItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate GroupsTemplate { get; set; }
        public DataTemplate TeachersTemplate { get; set; }
        public API.Models.User User { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (User.IsEmployee ?? false)
                return GroupsTemplate;
            else
                return TeachersTemplate;
        }
    }
}
