using GeekBurger.StoreCatalog.Client.Interfaces;
using System;
using System.Net.Http;

namespace GeekBurger.StoreCatalog.Client
{
    public class ClientHttp : IClientHttp
    {
        public static readonly HttpClient clientHttp;
        static ClientHttp()
        {
            clientHttp = new HttpClient();
        }
    }
}
