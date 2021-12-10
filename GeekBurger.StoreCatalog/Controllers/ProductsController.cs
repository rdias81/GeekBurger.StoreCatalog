using GeekBurger.StoreCatalog.Contract;
using Microsoft.AspNetCore.Mvc;

namespace GeekBurger.StoreCatalog.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        [HttpPost]
        [Route("{storeName}")]
        public IActionResult GetProducts(string storeName, [FromBody] UserRestriction userRestriction)
        {
            if (string.IsNullOrWhiteSpace(storeName))
                return NotFound();

            if (userRestriction.Restrictions != null && userRestriction.Restrictions.Count > 0)
            {
                var produtos = Services.StoreProductService.GetProductsByRestriction(userRestriction);
                return Ok(produtos);
            }
            else
            {
                var produtos = Services.StoreProductService.GetProducts();
                return Ok(produtos);
            }
        }
    }
}
