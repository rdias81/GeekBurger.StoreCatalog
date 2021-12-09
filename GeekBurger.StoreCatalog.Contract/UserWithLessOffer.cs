using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Contract
{
    public class UserWithLessOffer
    {
        public int UserId { get; set; }
        public IList<string> Restrictions { get; set; }
    }
}
