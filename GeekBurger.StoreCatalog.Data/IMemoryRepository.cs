using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Data
{
    public  interface IMemoryRepository     {
        Guid AddObject(Guid key, Object obj);

        object Get(Guid key);

    }
}
