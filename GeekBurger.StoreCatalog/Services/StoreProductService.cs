using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Contract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeekBurger.StoreCatalog.Services
{
    public class StoreProductService
    {
        private static List<ProductToGet> MockProdutos()
        {
            var beef = new ItemToGet { ItemId = Guid.NewGuid(), Name = "beef" };
            var pork = new ItemToGet { ItemId = Guid.NewGuid(), Name = "pork" };
            var mustard = new ItemToGet { ItemId = Guid.NewGuid(), Name = "mustard" };
            var ketchup = new ItemToGet { ItemId = Guid.NewGuid(), Name = "ketchup" };
            var bread = new ItemToGet { ItemId = Guid.NewGuid(), Name = "bread" };
            var wBread = new ItemToGet { ItemId = Guid.NewGuid(), Name = "whole bread" };

            return new List<ProductToGet>()
            {
                new ProductToGet { ProductId = Guid.NewGuid(), Name = "Darth Bacon",
                    Image = "hamb1.png", StoreId = Guid.NewGuid(),
                    Items = new List<ItemToGet> { beef, ketchup, bread }
                },
                new ProductToGet { ProductId = Guid.NewGuid(), Name = "Cap. Spork",
                    Image = "hamb2.png", StoreId = Guid.NewGuid(),
                    Items = new List<ItemToGet> { pork, mustard, wBread }
                },
                new ProductToGet { ProductId = Guid.NewGuid(), Name = "Beef Turner",
                    Image = "hamb3.png", StoreId = Guid.NewGuid(),
                    Items = new List<ItemToGet> { beef, mustard, bread }
                }
            };
        }

        public static IEnumerable<ProductToGet> GetProductsByRestriction(UserRestriction userRestriction)
        {
            return MockProdutos().Where(p => p.Name == "Cap. Spork");
        }

        public static IEnumerable<ProductToGet> GetProducts()
        {
            return MockProdutos().Where(p => p.Name != "Cap. Spork");
        }
    }
}
