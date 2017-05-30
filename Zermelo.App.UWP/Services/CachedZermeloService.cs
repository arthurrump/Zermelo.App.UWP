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
        IInternetConnectionService _internet;

        public CachedZermeloService(ZermeloService zermelo, ICacheService cache, IInternetConnectionService internet)
        {
            _zermelo = zermelo;
            _cache = cache;
            _internet = internet;
        }

        public IObservable<IEnumerable<Appointment>> GetSchedule(DateTimeOffset start, DateTimeOffset end, string user = "~me")
            => _cache.GetAndFetchLatest(
                $"{nameof(GetSchedule)}({start.UtcTicks},{end.UtcTicks},{user})",
                () => _zermelo.GetSchedule(start, end, user),
                date => _internet.IsConnected(),
                end.AddDays(7)
               );

        public IObservable<IEnumerable<Announcement>> GetAnnouncements()
            => _cache.GetAndFetchLatest(
                nameof(GetAnnouncements), 
                () => _zermelo.GetAnnouncements(),
                date => _internet.IsConnected()
               );

        public IObservable<API.Models.User> GetCurrentUser()
            => _cache.GetAndFetchLatest(
                nameof(GetCurrentUser), 
                () => _zermelo.GetCurrentUser(),
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(7)
               );
    }
}
