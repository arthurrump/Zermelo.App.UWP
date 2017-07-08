using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Zermelo.App.UWP.Schedule;
using Zermelo.App.UWP.Services;
using Zermelo.App.UWP.Shell;

namespace Zermelo.App.UWP.OtherSchedules
{
    public class OtherSchedulesViewModel : ViewModelBase
    {
        IZermeloService _zermelo;

        public OtherSchedulesViewModel(IZermeloService zermelo)
        {
            _zermelo = zermelo;

            GoToSchedule = new AwaitableDelegateCommand(async par =>
            {
                string symbolText, text;
                switch ((ScheduleType)Type)
                {
                    case ScheduleType.Student:
                        var student = await _zermelo.GetStudent(Code);
                        symbolText = $"{student.FirstName[0]}{student.LastName[0]}";
                        text = $"Rooster van {student.FirstName}";
                        break;
                    case ScheduleType.Employee:
                        var employee = await _zermelo.GetEmployee(Code);
                        symbolText = string.IsNullOrEmpty(employee.Prefix) ? employee.LastName.Substring(0, 2) : $"{employee.Prefix[0]}{employee.LastName[0]}";
                        text = $"Rooster van {employee.FullName}";
                        break;
                    case ScheduleType.Group:
                        throw new NotImplementedException();
                    case ScheduleType.Location:
                        throw new NotImplementedException();
                    default:
                        symbolText = ""; text = "";
                        break;
                }

                var navItems = ((ShellView)WindowWrapper.Current().Content).HamburgerPrimaryButtons;

                if (!navItems.Any(x => x.PageType == typeof(ScheduleView) && (string)x.PageParameter == $"{Type}{Code}"))
                {
                    navItems.Add(
                        new HamburgerButtonInfo()
                        {
                            Content = new TextHamburgerButton()
                            {
                                Text = text,
                                SymbolText = symbolText
                            },
                            PageType = typeof(ScheduleView),
                            PageParameter = ($"{Type}{Code}")
                        }
                    );
                }

                NavigationService.Navigate(typeof(ScheduleView), $"{Type}{Code}");
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

        public AwaitableDelegateCommand GoToSchedule { get; }
    }
}
