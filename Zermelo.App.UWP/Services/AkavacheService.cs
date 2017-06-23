using Akavache;
using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Zermelo.App.UWP.Services
{
    public class AkavacheService : ICacheService
    {
        public Task Shutdown() => BlobCache.Shutdown();

        public IObservable<T> GetAndFetchLatest<T>(string key,
            Func<IObservable<T>> fetchFunc,
            Func<DateTimeOffset, bool> fetchPredicate = null,
            DateTimeOffset? absoluteExpiration = null)
                => BlobCache.LocalMachine.GetAndFetchLatest(key, fetchFunc, fetchPredicate, absoluteExpiration);

        public IObservable<Unit> ClearCache() => BlobCache.LocalMachine.InvalidateAll();
    }
}
