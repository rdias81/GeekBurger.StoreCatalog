using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Contract
{
    public class Product
    {
        public string StoreName { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public List<Item> Items { get; set; }
        public decimal Price { get; set; }
    }
}
