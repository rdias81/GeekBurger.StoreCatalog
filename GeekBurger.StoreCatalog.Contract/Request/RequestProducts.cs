using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Contract.Request
{
    public class RequestProducts
    {
        String StoreName { get; set; }
        int UserId { get; set; }
        public List<string> Restrictions { get; set; }
    }
}
