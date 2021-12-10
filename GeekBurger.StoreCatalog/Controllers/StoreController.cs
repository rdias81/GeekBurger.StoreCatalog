using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GeekBurger.StoreCatalog.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class StoreController : Controller
    {
        private readonly IMemoryCache _memory;

        public StoreController(IMemoryCache memory)
        {
            _memory = memory;
        }



        [HttpGet]
        [Route("{storeName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetStoreStatus(string storeName)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = System.TimeSpan.FromSeconds(3600),
                SlidingExpiration = System.TimeSpan.FromSeconds(3600),
            };
            
            _memory.Set(1, "Morumbi",memoryCacheEntryOptions);

            var teste = _memory.Get(1);


            

            if (storeName == "teste")
                return StatusCode(503);

            return Ok();
        }
    }
}
