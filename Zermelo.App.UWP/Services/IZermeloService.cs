using System;
using System.Collections.Generic;
using Zermelo.App.UWP.Announcements;
using Zermelo.App.UWP.OtherSchedules;
using Zermelo.App.UWP.Schedule;

namespace Zermelo.App.UWP.Services
{
    public interface IZermeloService
    {
        IObservable<IEnumerable<Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me");
        IObservable<IEnumerable<Appointment>> GetScheduleForGroup(DateTimeOffset start, DateTimeOffset end, string code);
        IObservable<IEnumerable<Announcement>> GetAnnouncements();
        IObservable<API.Models.User> GetCurrentUser();
        IObservable<API.Models.User> GetStudent(string code);
        IObservable<API.Models.User> GetEmployee(string code);
        IObservable<API.Models.Group> GetGroup(string code);
        IObservable<IEnumerable<SearchItem>> GetAllStudentsAsSearchItems();
        IObservable<IEnumerable<SearchItem>> GetAllEmployeesAsSearchItems();
        IObservable<IEnumerable<SearchItem>> GetAllGroupsAsSearchItems();
    }
}