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

        public IObservable<IEnumerable<Appointment>> GetScheduleAsync(DateTimeOffset start, DateTimeOffset end, string user = "~me")
            => BlobCache.LocalMachine.GetAndFetchLatest(
                $"{nameof(GetScheduleAsync)}({start.UtcTicks},{end.UtcTicks},{user})",
                () => _zermelo.GetScheduleAsync(start, end, user)
               );

        public IObservable<IEnumerable<Announcement>> GetAnnouncementsAsync()
            => BlobCache.LocalMachine.GetAndFetchLatest(
                nameof(GetAnnouncementsAsync), 
                () => _zermelo.GetAnnouncementsAsync()
               );

        public IObservable<User> GetCurrentUserAsync()
            => BlobCache.LocalMachine.GetAndFetchLatest(
                nameof(GetCurrentUserAsync), 
                () => _zermelo.GetCurrentUserAsync()
               );
    }
}
