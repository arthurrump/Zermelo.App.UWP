using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zermelo.API;
using Zermelo.App.UWP.Models;

namespace Zermelo.App.UWP.Services
{
    public class ZermeloService : IZermeloService
    {
        ZermeloConnection connection;

        public ZermeloService(Authentication auth)
        {
            connection = new ZermeloConnection(auth);
        }

        public IObservable<IEnumerable<Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me")
            => Observable.FromAsync(
                async () => 
                    (await connection.Appointments.GetByDateForUserAsync(start, end, user, fields: Appointment.Fields))
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
                () => connection.Users.GetCurrentUserAsync()
               );
    }
}
