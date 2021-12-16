using System;

namespace GeekBurger.StoreCatalog.Contract
{
    public class StoreCatalogReady
    {
        public StoreCatalogReady()
        {

        }
        public string StoreName { get; set; }
        public bool Ready { get; set; }
    }
}
