using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using NodaTime;
using Zermelo.App.UWP.Announcements;
using Zermelo.App.UWP.OtherSchedules;
using Zermelo.App.UWP.Schedule;

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

        public IObservable<IEnumerable<Appointment>> GetSchedule(LocalDate date, string user = "~me")
            => _cache.GetAndFetchLatest(
                $"{nameof(GetSchedule)}({date},{user})",
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetSchedule(date, user);
                    else
                        return Observable.Return(new List<Appointment>());
                },
                cacheDate => _internet.IsConnected(),
                date.PlusDays(7).ToDateTimeUnspecified()
               );

        public IObservable<IEnumerable<Appointment>> GetScheduleForGroup(LocalDate date, string code)
            => _cache.GetAndFetchLatest(
                $"{nameof(GetScheduleForGroup)}({date},{code})",
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetScheduleForGroup(date, code);
                    else
                        return Observable.Return(new List<Appointment>());
                },
                cacheDate => _internet.IsConnected(),
                date.PlusDays(7).ToDateTimeUnspecified()
               );

        public IObservable<IEnumerable<Appointment>> GetScheduleForLocation(LocalDate date, string code)
            => _cache.GetAndFetchLatest(
                $"{nameof(GetScheduleForLocation)}({date},{code})",
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetScheduleForLocation(date, code);
                    else
                        return Observable.Return(new List<Appointment>());
                },
                cacheDate => _internet.IsConnected(),
                date.PlusDays(7).ToDateTimeUnspecified()
               );

        public IObservable<IEnumerable<Announcement>> GetAnnouncements()
            => _cache.GetAndFetchLatest(
                nameof(GetAnnouncements),
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetAnnouncements();
                    else
                        return Observable.Return(new List<Announcement>());
                },
                date => _internet.IsConnected()
               );

        public IObservable<API.Models.User> GetCurrentUser()
            => _cache.GetAndFetchLatest(
                nameof(GetCurrentUser),
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetCurrentUser();
                    else
                        return Observable.Return(default(API.Models.User));
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(31)
               );

        public IObservable<API.Models.User> GetStudent(string code)
            => _cache.GetAndFetchLatest(
                $"{nameof(GetStudent)}({code})",
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetStudent(code);
                    else
                        return Observable.Return(default(API.Models.User));
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(14)
               );

        public IObservable<API.Models.User> GetEmployee(string code)
            => _cache.GetAndFetchLatest(
                $"{nameof(GetEmployee)}({code})",
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetEmployee(code);
                    else
                        return Observable.Return(default(API.Models.User));
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(14)
               );

        public IObservable<API.Models.Group> GetGroup(string code)
            => _cache.GetAndFetchLatest(
                $"{nameof(GetGroup)}({code})",
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetGroup(code);
                    else
                        return Observable.Return(default(API.Models.Group));
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(14)
               );

        public IObservable<API.Models.Location> GetLocation(string code)
            => _cache.GetAndFetchLatest(
                $"{nameof(GetLocation)}({code})",
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetLocation(code);
                    else
                        return Observable.Return(default(API.Models.Location));
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(14)
               );

        public IObservable<IEnumerable<SearchItem>> GetAllStudentsAsSearchItems()
            => _cache.GetAndFetchLatest(
                nameof(GetAllStudentsAsSearchItems),
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetAllStudentsAsSearchItems();
                    else
                        return Observable.Return(new List<SearchItem>());
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(7)
               );

        public IObservable<IEnumerable<SearchItem>> GetAllEmployeesAsSearchItems()
            => _cache.GetAndFetchLatest(
                nameof(GetAllEmployeesAsSearchItems),
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetAllEmployeesAsSearchItems();
                    else
                        return Observable.Return(new List<SearchItem>());
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(7)
               );

        public IObservable<IEnumerable<SearchItem>> GetAllGroupsAsSearchItems()
            => _cache.GetAndFetchLatest(
                nameof(GetAllGroupsAsSearchItems),
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetAllGroupsAsSearchItems();
                    else
                        return Observable.Return(new List<SearchItem>());
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(7)
               );

        public IObservable<IEnumerable<SearchItem>> GetAllLocationsAsSearchItems()
            => _cache.GetAndFetchLatest(
                nameof(GetAllLocationsAsSearchItems),
                () => {
                    if (_internet.IsConnected())
                        return _zermelo.GetAllLocationsAsSearchItems();
                    else
                        return Observable.Return(new List<SearchItem>());
                },
                date => _internet.IsConnected() && DateTimeOffset.UtcNow.Subtract(date) > TimeSpan.FromDays(7)
               );
    }
}
