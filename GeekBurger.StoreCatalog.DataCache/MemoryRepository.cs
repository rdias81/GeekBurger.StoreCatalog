using System;

namespace GeekBurger.StoreCatalog.DataCache
{
    using System;
    using Microsoft.Extensions.Caching.Memory;
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



        public object GetById(int id)
        {
            return _memory.Get(id);

        }

        public void RemoveById(int id)
        {

            if (_memory.TryGetValue(id, out object objeto))
            {
                _memory.Remove($"id:{id}");

            }
        }


        public object Add(object objeto)
        {

            return _memory.GetOrCreate($"id:{objeto.GetType().GetProperty("Id").GetValue(objeto,null)}", cacheEntry =>
            {
                return _memory.Get(objeto.GetType().GetProperty("Id").GetValue(objeto, null));
            });
        }






    }
}
