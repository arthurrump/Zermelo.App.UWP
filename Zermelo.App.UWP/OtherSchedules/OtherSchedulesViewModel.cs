﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Zermelo.App.UWP.Helpers;
using Zermelo.App.UWP.Schedule;
using Zermelo.App.UWP.Services;
using Zermelo.App.UWP.Shell;

namespace Zermelo.App.UWP.OtherSchedules
{
    public class OtherSchedulesViewModel : ViewModelBase
    {
        IZermeloService _zermelo;
        IEnumerable<SearchItem> _students = new List<SearchItem>();
        IEnumerable<SearchItem> _employees = new List<SearchItem>();
        IEnumerable<SearchItem> _groups = new List<SearchItem>();

        public OtherSchedulesViewModel(IZermeloService zermelo)
        {
            _zermelo = zermelo;

            GoToSchedule = new AwaitableDelegateCommand(GoToScheduleDelegate);
            Search = new DelegateCommand(SearchDelegate);

            _zermelo.GetAllStudentsAsSearchItems()
                .ObserveOnDispatcher()
                .Subscribe(
                    s => _students = s,
                    ex => ExceptionHelper.HandleException(ex, $"{nameof(OtherSchedulesViewModel)}.GetStudents",
                        x => new MessageDialog(x, "Error").ShowAsync())
                );

            _zermelo.GetAllEmployeesAsSearchItems()
                .ObserveOnDispatcher()
                .Subscribe(
                    e => _employees = e,
                    ex => ExceptionHelper.HandleException(ex, $"{nameof(OtherSchedulesViewModel)}.GetEmployees",
                        x => new MessageDialog(x, "Error").ShowAsync())
                );

            _zermelo.GetAllGroupsAsSearchItems()
                .ObserveOnDispatcher()
                .Subscribe(
                    e => _employees = e,
                    ex => ExceptionHelper.HandleException(ex, $"{nameof(OtherSchedulesViewModel)}.GetGroups",
                        x => new MessageDialog(x, "Error").ShowAsync())
                );
        }

        private void SearchDelegate()
        {
            SearchItems.MorphInto(
                _students
                    .Concat(_employees)
                    .Concat(_groups)
                    .Where(s => s.DisplayText.ToLowerInvariant().Contains(SearchText.ToLowerInvariant()))
                    .OrderBy(s => s.Type)
                    .ThenBy(s => s.Code)
            );
        }

        private async Task GoToScheduleDelegate(AwaitableDelegateCommandParameter par)
        {
            string symbolText, text;
            switch (SelectedSearchItem.Type)
            {
                case ScheduleType.Student:
                    var student = await _zermelo.GetStudent(SelectedSearchItem.Code);
                    symbolText = $"{student.FirstName[0]}{student.LastName[0]}";
                    text = $"Rooster van {student.FirstName}";
                    break;
                case ScheduleType.Employee:
                    var employee = await _zermelo.GetEmployee(SelectedSearchItem.Code);
                    symbolText = string.IsNullOrEmpty(employee.Prefix) ? employee.LastName.Substring(0, 2) : $"{employee.Prefix[0]}{employee.LastName[0]}";
                    text = $"Rooster van {employee.FullName}";
                    break;
                case ScheduleType.Group:
                    var group = await _zermelo.GetGroup(SelectedSearchItem.Code);
                    symbolText = group.Name.Substring(0, 2).ToUpperInvariant();
                    text = $"Rooster van {group.ExtendedName}";
                    break;
                case ScheduleType.Location:
                    throw new NotImplementedException();
                default:
                    symbolText = ""; text = "";
                    break;
            }

            var parameter = $"{(int)SelectedSearchItem.Type}{SelectedSearchItem.Code}";

            var navItems = ((ShellView)WindowWrapper.Current().Content).HamburgerPrimaryButtons;
            if (!navItems.Any(x => x.PageType == typeof(ScheduleView) && (string)x.PageParameter == parameter))
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
                        PageParameter = parameter
                    }
                );
            }

            NavigationService.Navigate(typeof(ScheduleView), parameter);
        }

        string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                RaisePropertyChanged();
            }
        }

        SearchItem _selectedSearchItem;
        public SearchItem SelectedSearchItem
        {
            get => _selectedSearchItem;
            set
            {
                _selectedSearchItem = value;
                RaisePropertyChanged();
            }
        }

        ObservableCollection<SearchItem> _searchItems = new ObservableCollection<SearchItem>();
        public ObservableCollection<SearchItem> SearchItems => _searchItems;

        public DelegateCommand Search { get; }

        public AwaitableDelegateCommand GoToSchedule { get; }
    }
}
