using System;
using System.Collections.Generic;
using Zermelo.App.UWP.Announcements;
using Zermelo.App.UWP.Schedule;

namespace Zermelo.App.UWP.Services
{
    public interface IZermeloService
    {
        IObservable<IEnumerable<Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me");
        IObservable<IEnumerable<Announcement>> GetAnnouncements();
        IObservable<API.Models.User> GetCurrentUser();
    }
}
