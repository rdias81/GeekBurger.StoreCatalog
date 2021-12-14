using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client.Entities
{
    internal class Ingredients
    {
        public List<string> Restrictions { get; set; }
        public String StoreName { get; set; }

        public Ingredients()
        {
            Restrictions = new List<string>();
        }
    }


}
