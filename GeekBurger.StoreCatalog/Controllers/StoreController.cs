using GeekBurger.StoreCatalog.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace GeekBurger.StoreCatalog.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class StoreController : Controller
    {
        private IMemoryRepository _memoryRepository;
        public StoreController(IMemoryRepository memoryRepository)
        {
            _memoryRepository = memoryRepository;
        }


        [HttpGet]
        [Route("{storeName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetStoreStatus(string storeName)
        {

           var retorno =  _memoryRepository.AddObject(System.Guid.NewGuid(), "lOJA MORUMBI");
            var teste = _memoryRepository.Get(retorno);


            if (storeName == "teste")
                return StatusCode(503);

            return Ok(retorno);
        }
    }
}
