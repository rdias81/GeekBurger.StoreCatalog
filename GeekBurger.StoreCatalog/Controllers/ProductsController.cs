using GeekBurger.StoreCatalog.Client.Interfaces;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.Contract.Request;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Controllers
{



    [ApiController]
    [Route("api/products")]
    public class ProductsController : Controller
    {
        [HttpPost()]
        public IActionResult GetProductsByStoreName(RequestProducts requestProducts)
        {
            //Processo de filtro com os dados enviados em requestProducts
            return Ok(Mock());
        }

        List<Product> Mock()
        {
            var response = new List<Product>() {new Product()
                {
                    StoreName = "Paulista",
                    ProductId = new System.Guid(),
                    Name = "Darth, Bacon",
                    Image = "img_db.jpg",
                    Items = new List<Item>() { new Item() { ItemId = new System.Guid(), Name = "bread" }, new Item() { ItemId = new System.Guid(), Name = "meat" } },
                    Price = 10
                }
            };

            return response;

        }
    }



}
