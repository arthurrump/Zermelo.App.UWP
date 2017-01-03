using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using Zermelo.API.Models;

namespace Zermelo.App.UWP.Services
{
    public class CachedZermeloService : IZermeloService
    {
        ZermeloService _zermelo;

        public CachedZermeloService(ZermeloService zermelo)
        {
            _zermelo = zermelo;
        }

        public IObservable<IEnumerable<Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me")
            => BlobCache.LocalMachine.GetAndFetchLatest(
                $"{nameof(GetSchedule)}({start.UtcTicks},{end.UtcTicks},{user})",
                () => _zermelo.GetSchedule(start, end, user)
               );

        public IObservable<IEnumerable<Announcement>> GetAnnouncements()
            => BlobCache.LocalMachine.GetAndFetchLatest(
                nameof(GetAnnouncements), 
                () => _zermelo.GetAnnouncements()
               );

        public IObservable<User> GetCurrentUser()
            => BlobCache.LocalMachine.GetAndFetchLatest(
                nameof(GetCurrentUser), 
                () => _zermelo.GetCurrentUser()
               );
    }
}
