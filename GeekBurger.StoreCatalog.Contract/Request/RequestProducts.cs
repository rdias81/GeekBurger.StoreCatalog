using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Contract.Request
{
    public class RequestProducts
    {
        public RequestProducts()
        {

        }
        public String StoreName { get; set; }
        public int UserId { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
