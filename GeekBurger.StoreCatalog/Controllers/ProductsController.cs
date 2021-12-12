using GeekBurger.StoreCatalog.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Controllers
{

    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {

        private readonly IProduction _production;   

        public ProductsController(IProduction service)
        {
            _production = service;
        }


        [HttpGet()]
        public IActionResult GetProductsByStoreName()
        {
            _production.ProductionTest();
            return Ok();
        }
    }
}
