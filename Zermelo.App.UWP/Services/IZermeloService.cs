using System;
using System.Collections.Generic;
using NodaTime;
using Zermelo.App.UWP.Announcements;
using Zermelo.App.UWP.OtherSchedules;
using Zermelo.App.UWP.Schedule;

namespace Zermelo.App.UWP.Services
{
    public interface IZermeloService
    {
        IObservable<IEnumerable<Appointment>> GetSchedule(LocalDate date, string user = "~me");
        IObservable<IEnumerable<Appointment>> GetScheduleForGroup(LocalDate date, string code);
        IObservable<IEnumerable<Appointment>> GetScheduleForLocation(LocalDate date, string code);
        IObservable<IEnumerable<Announcement>> GetAnnouncements();
        IObservable<API.Models.User> GetCurrentUser();
        IObservable<API.Models.User> GetStudent(string code);
        IObservable<API.Models.User> GetEmployee(string code);
        IObservable<API.Models.Group> GetGroup(string code);
        IObservable<API.Models.Location> GetLocation(string code);
        IObservable<IEnumerable<SearchItem>> GetAllStudentsAsSearchItems();
        IObservable<IEnumerable<SearchItem>> GetAllEmployeesAsSearchItems();
        IObservable<IEnumerable<SearchItem>> GetAllGroupsAsSearchItems();
        IObservable<IEnumerable<SearchItem>> GetAllLocationsAsSearchItems();
    }
}