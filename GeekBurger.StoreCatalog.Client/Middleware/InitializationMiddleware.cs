using GeekBurger.Production.Contract;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Client.Interfaces;
using GeekBurger.StoreCatalog.Client.Services;
using GeekBurger.StoreCatalog.DataCache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client.Middleware
{
    public class InitializationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appSettings;
        private readonly IMemoryCache _memoryCache;

        public InitializationMiddleware(RequestDelegate next, IConfiguration appSettings, IMemoryCache memoryCache)
        {
            _next = next;
            _appSettings = appSettings;
            _memoryCache = memoryCache;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                return;

            string nomeLoja = "Morumbi";
            int produtos = await CarregarProdutos(nomeLoja);
            int areas = await CarregarAreas();

            await context.Response.WriteAsync($"Existem {produtos} produtos disponiveis e {areas} areas cadastradas na loja {nomeLoja}. \n\n");
            await _next(context);
        }

        private async Task<int> CarregarProdutos(string nomeLoja)
        {
            IProducts productsClient = new ProductsClient();
            var respProdutos = productsClient.GetProducts(nomeLoja);

            var repository = new MemoryRepository(_memoryCache);
            var produtoService = new ProductService(repository);
            produtoService.Salvar(await respProdutos);

            return respProdutos.Result.Count; 
        }

        private async Task<int> CarregarAreas()
        {
            IProduction productionClient = new ProductionClient();
            var respAreas = productionClient.GetAreas();

            var repository = new MemoryRepository(_memoryCache);
            var productionService = new ProductionService(repository);
            productionService.Salvar(await respAreas);

            return respAreas.Result.Count;
        }
    }
}
