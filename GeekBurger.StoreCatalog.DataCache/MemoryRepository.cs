using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace GeekBurger.StoreCatalog.DataCache
{
    public class MemoryRepository : IMemoryRepository
    {
        private readonly IMemoryCache _memory;
        private readonly MemoryCacheEntryOptions memoryCacheEntryOptions;

        public MemoryRepository(IMemoryCache memory)
        {
            _memory = memory;

            memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromSeconds(3600),
                SlidingExpiration = System.TimeSpan.FromSeconds(3600),
            };
        }

        public T GetById<T>(string id)
        {
            var _key = new KeyValuePair<string, string>(typeof(T).ToString(), id);
            return _memory.Get<T>(_key);
        }

        //public void RemoveById(int id)
        //{
        //    if (_memory.TryGetValue(id, out object objeto))
        //    {
        //        _memory.Remove($"id:{id}");
        //    }
        //}

        public void Add<T>(string id, T item)
        {
            var _key = new KeyValuePair<string, string>(typeof(T).ToString(), id);
            _memory.Set(_key, item);
        }

    }
}
