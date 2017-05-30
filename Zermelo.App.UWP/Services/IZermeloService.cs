using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zermelo.App.UWP.Models;

namespace Zermelo.App.UWP.Services
{
    public interface IZermeloService
    {
        IObservable<IEnumerable<Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me");
        IObservable<IEnumerable<Announcement>> GetAnnouncements();
        IObservable<API.Models.User> GetCurrentUser();
    }
}
