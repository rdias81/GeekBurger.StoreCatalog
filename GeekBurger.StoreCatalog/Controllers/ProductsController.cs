using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        [HttpGet("{storeName}/{UserId}")]
        public IActionResult GetProductsByStoreName(string storeName,
                                                    int UserId,
                                                    [FromBody] IEnumerable<string> restrictions)
        {
            return Ok();
        }
    }
}
