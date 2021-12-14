using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.StoreCatalog.Controllers
{
    [ApiController]
    [Route("api/store")]
    public class StoreController : Controller
    {
        [HttpGet]
        [Route("{storeName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
        public IActionResult GetStoreStatus(string storeName)
        {
            if (storeName == "testeErro")
                return StatusCode(503);

            return Ok(new { StoreName = storeName, Ready = true });
        }
    }
}
