using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zermelo.API;
using Zermelo.API.Models;

namespace Zermelo.App.UWP.Services
{
    public interface IZermeloService
    {
        IObservable<IEnumerable<Appointment>> GetScheduleAsync(DateTimeOffset start, DateTimeOffset end, string user = "~me");
        IObservable<IEnumerable<Announcement>> GetAnnouncementsAsync();
        IObservable<User> GetCurrentUserAsync();
    }
}
