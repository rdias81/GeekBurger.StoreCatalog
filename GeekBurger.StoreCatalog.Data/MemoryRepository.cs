using System;
using Microsoft.Extensions.Caching.Memory;

namespace GeekBurger.StoreCatalog.Data
{
    public class MemoryRepository : IMemoryRepository
    {
        private readonly IMemoryCache _memory;
        private MemoryCacheEntryOptions memoryCacheEntryOptions;
        public MemoryRepository(IMemoryCache memory)
        {
            _memory = memory;

            memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromSeconds(3600),
                SlidingExpiration = System.TimeSpan.FromSeconds(3600),
            };


        }

     

        public Guid AddObject(Guid key, object obj)
        {
            if (_memory.Get(key) == null)
            {
                _memory.Set(key, obj, memoryCacheEntryOptions);

            }

            return key;
        }

        public object Get(Guid key)
        {
            return _memory.Get(key);


        }


    }
}
