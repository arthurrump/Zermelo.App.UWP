using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using Zermelo.API;
using Zermelo.App.UWP.Announcements;
using Zermelo.App.UWP.OtherSchedules;
using Zermelo.App.UWP.Schedule;

namespace Zermelo.App.UWP.Services
{
    public class ZermeloService : IZermeloService
    {
        ZermeloConnection connection;

        public ZermeloService(ISettingsService settings)
        {
            Authentication auth = new Authentication(settings.School, settings.Token);
            connection = new ZermeloConnection(auth);
        }

        public IObservable<IEnumerable<Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me")
            => Observable.FromAsync(
                async () => 
                    (await connection.Appointments.GetByCustomUrlOptionsAsync(new Dictionary<string, string>
                        {
                            { "start", start.ToUnixTimeSeconds().ToString() },
                            { "end", end.ToUnixTimeSeconds().ToString() },
                            { "user", user },
                            { "valid", "true" }
                        },
                        fields: Appointment.Fields))
                        .Select(a => new Appointment(a))
               );

        public IObservable<IEnumerable<Announcement>> GetAnnouncements()
            => Observable.FromAsync(
                async () =>
                    (await connection.Announcements.GetCurrentAsync(fields: Announcement.Fields))
                        .Select(a => new Announcement(a))
                );

        public IObservable<API.Models.User> GetCurrentUser() 
            => Observable.FromAsync(
                () => connection.Users.GetCurrentUserAsync(new List<string> { "firstName", "prefix", "lastName", "code", "isEmployee" })
               );

        public IObservable<API.Models.User> GetStudent(string code)
            => Observable.FromAsync(
                () => connection.Users.GetByCodeAsync(code, new List<string> { "firstName", "prefix", "lastName" })
               );

        public IObservable<API.Models.User> GetEmployee(string code)
            => Observable.FromAsync(
                () => connection.Users.GetByCodeAsync(code, new List<string> { "prefix", "lastName" })
               );

        public IObservable<IEnumerable<SearchItem>> GetAllStudentsAsSearchItems()
            => Observable.FromAsync(
                async () => 
                    (await connection.Users.GetStudentsAsync(fields: new List<string> { "firstName", "prefix", "lastName", "code" }))
                        .Select(s => new SearchItem(s, ScheduleType.Student))
               );

        public IObservable<IEnumerable<SearchItem>> GetAllEmployeesAsSearchItems()
            => Observable.FromAsync(
                async () => 
                    (await connection.Users.GetEmployeesAsync(fields: new List<string> { "prefix", "lastName", "code" }))
                        .Select(e => new SearchItem(e, ScheduleType.Employee))
               );
    }
}
