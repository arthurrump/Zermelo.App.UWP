using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Zermelo.API;
using Zermelo.API.Models;

namespace Zermelo.App.UWP.Services
{
    public class ZermeloService : IZermeloService
    {
        ZermeloConnection connection;

        public ZermeloService(Authentication auth)
        {
            connection = new ZermeloConnection(auth);
        }

        public IObservable<IEnumerable<Appointment>> GetScheduleAsync(DateTimeOffset start, DateTimeOffset end, string user = "~me")
            => Observable.FromAsync(() => connection.Appointments.GetByDateAsync(start, end, user));

        public IObservable<IEnumerable<Announcement>> GetAnnouncementsAsync() 
            => Observable.FromAsync(() => connection.Announcements.GetAllAsync());

        public IObservable<User> GetCurrentUserAsync() 
            => Observable.FromAsync(() => connection.Users.GetCurrentUserAsync());
    }
}
