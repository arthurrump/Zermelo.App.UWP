using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Zermelo.App.UWP.Schedule;
using Zermelo.App.UWP.Shell;

namespace Zermelo.App.UWP.OtherSchedules
{
    public class OtherSchedulesViewModel : ViewModelBase
    {
        public OtherSchedulesViewModel()
        {
            GoToSchedule = new DelegateCommand(() =>
            {
                ((ShellView)WindowWrapper.Current().Content).HamburgerPrimaryButtons.Add(
                    new HamburgerButtonInfo()
                    {
                        Content = new HamburgerButton()
                        {
                            Text = Code,
                            Symbol = Symbol.Contact
                        },
                        PageType = typeof(ScheduleView),
                        PageParameter = ((ScheduleType)Type, Code)
                    }
                );

                NavigationService.Navigate(typeof(ScheduleView), ((ScheduleType)Type, Code));
            });
        }

        private string _code;
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                RaisePropertyChanged();
            }
        }

        private int _type;
        public int Type
        {
            get => _type;
            set
            {
                _type = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand GoToSchedule { get; }
    }
}
