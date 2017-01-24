using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akavache;
using Zermelo.App.UWP.Models;

namespace Zermelo.App.UWP.Services
{
    public class CachedZermeloService : IZermeloService
    {
        ZermeloService _zermelo;
        ICacheService _cache;

        public CachedZermeloService(ZermeloService zermelo, ICacheService cache)
        {
            _zermelo = zermelo;
            _cache = cache;
        }

        public IObservable<IEnumerable<API.Models.Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me")
            => _cache.GetAndFetchLatest(
                $"{nameof(GetSchedule)}({start.UtcTicks},{end.UtcTicks},{user})",
                () => _zermelo.GetSchedule(start, end, user),
                absoluteExpiration: end.AddDays(2)
               );

        public IObservable<IEnumerable<Announcement>> GetAnnouncements()
            => _cache.GetAndFetchLatest(
                nameof(GetAnnouncements), 
                () => _zermelo.GetAnnouncements()
               );

        public IObservable<API.Models.User> GetCurrentUser()
            => _cache.GetAndFetchLatest(
                nameof(GetCurrentUser), 
                () => _zermelo.GetCurrentUser(),
                date => DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(7)
               );
    }
}
