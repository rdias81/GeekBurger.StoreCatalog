using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Contract.Request
{
    public class RequestStore
    {
        public string StoreName { get; set; }
        public bool Ready { get; set; }
    }
}
