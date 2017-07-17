using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using NodaTime;
using NodaTime.Extensions;
using Zermelo.API;
using Zermelo.App.UWP.Announcements;
using Zermelo.App.UWP.Helpers;
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

        public IObservable<IEnumerable<Appointment>> GetSchedule(LocalDate date, string user = "~me")
            => Observable.FromAsync(
                async () => 
                    (await connection.Appointments.GetByCustomUrlOptionsAsync(new Dictionary<string, string>
                        {
                            { "start", date.ToMidnightUnixTimeString() },
                            { "end", date.PlusDays(1).ToMidnightUnixTimeString() },
                            { "user", user },
                            { "valid", "true" }
                        },
                        fields: Appointment.Fields))
                        .Select(a => new Appointment(a))
               );

        public IObservable<IEnumerable<Appointment>> GetScheduleForGroup(LocalDate date, string code)
            => Observable.FromAsync(
                async () =>
                    (await connection.Appointments.GetByCustomUrlOptionsAsync(new Dictionary<string, string>
                        {
                            { "start", date.ToMidnightUnixTimeString() },
                            { "end", date.PlusDays(1).ToMidnightUnixTimeString() },
                            { "containsStudentsFromGroupInDepartment", code },
                            { "valid", "true" }
                        },
                        fields: Appointment.Fields))
                        .Select(a => new Appointment(a))
               );

        public IObservable<IEnumerable<Appointment>> GetScheduleForLocation(LocalDate date, string code)
            => Observable.FromAsync(
                async () =>
                    (await connection.Appointments.GetByCustomUrlOptionsAsync(new Dictionary<string, string>
                        {
                            { "start", date.ToMidnightUnixTimeString() },
                            { "end", date.PlusDays(1).ToMidnightUnixTimeString() },
                            { "locationsOfBranch", code },
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

        public IObservable<API.Models.Group> GetGroup(string code)
            => Observable.FromAsync(
                () => connection.Groups.GetSingleById(long.Parse(code), new List<string> { "id", "name", "extendedName" })
               );

        public IObservable<API.Models.Location> GetLocation(string code)
            => Observable.FromAsync(
                () => connection.Locations.GetSingleById(long.Parse(code), new List<string> { "id", "name" })
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

        public IObservable<IEnumerable<SearchItem>> GetAllGroupsAsSearchItems()
            => Observable.FromAsync(
                async () =>
                    (await connection.Groups.GetAllAsync(new List<string> { "id", "name", "extendedName" }))
                        .Select(g => new SearchItem(g))
               );

        public IObservable<IEnumerable<SearchItem>> GetAllLocationsAsSearchItems()
            => Observable.FromAsync(
                async () =>
                    (await connection.Locations.GetAllAsync(new List<string> { "id", "name" }))
                        .Select(l => new SearchItem(l))
               );
    }
}
