using Autofac;
using Windows.UI.Xaml.Controls;

namespace Zermelo.App.UWP.Schedule
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
}
