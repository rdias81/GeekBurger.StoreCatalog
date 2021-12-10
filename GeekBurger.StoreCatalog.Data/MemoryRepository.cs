using System;
using Microsoft.Extensions.Caching.Memory;

namespace GeekBurger.StoreCatalog.Data
{
    public class MemoryRepository
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

        public void AddObject(Guid key,Object obj)
        {

         //TODO : Verificar se chave ja existe em memoria.
            
            _memory.Set(key, obj, memoryCacheEntryOptions);

         
        }



    }
}
