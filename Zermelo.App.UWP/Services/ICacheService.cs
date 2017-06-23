using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Zermelo.App.UWP.Services
{
    public interface ICacheService
    {
        Task Shutdown();

        IObservable<T> GetAndFetchLatest<T>(string key,
            Func<IObservable<T>> fetchFunc,
            Func<DateTimeOffset, bool> fetchPredicate = null,
            DateTimeOffset? absoluteExpiration = null);

        IObservable<Unit> ClearCache();
    }
}
