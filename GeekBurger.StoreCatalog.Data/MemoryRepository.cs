using System;
using Microsoft.Extensions.Caching.Memory;

namespace GeekBurger.StoreCatalog.Data
{
    public class MemoryRepository<T> : IMemoryRepository<T> where T : BaseEntity
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



        public T GetById(int id)
        {
            return (T)_memory.Get(id);        
        
        }
      
        public void RemoveById(int id)
        {
            
            if (_memory.TryGetValue(id, out T entity))
            {
                _memory.Remove($"id:{entity.Id}");
               
            }
        }


        public T Add(T objeto)
        {

            return (T)_memory.GetOrCreate($"id:{objeto.Id}", cacheEntry =>
            {
                return _memory.Get(objeto.Id);
            });
        }

    }
}
