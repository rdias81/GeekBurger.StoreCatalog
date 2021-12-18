using GeekBurger.StoreCatalog.Client;
using GeekBurger.StoreCatalog.Client.Interfaces;
using GeekBurger.StoreCatalog.Client.Middleware;
using GeekBurger.StoreCatalog.Client.ServiceBus;
using GeekBurger.StoreCatalog.Contract;
using GeekBurger.StoreCatalog.DataCache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBurger.StoreCatalog.Client;
using GeekBurger.StoreCatalog.Client.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using GeekBurger.StoreCatalog.DataCache;
using GeekBurger.StoreCatalog.Services;
using GeekBurger.Products.Contract;
using GeekBurger.Production.Contract;

namespace GeekBurger.StoreCatalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeekBurger.StoreCatalog", Version = "v1" });
            });

            services.AddSingleton<IServiceBusEngine, ServiceBusEngine>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            services.AddSingleton<IMemoryRepository, MemoryRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCache memoryCache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeekBurger.StoreCatalog v1"));


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseMiddleware<QueueServiceBusMiddleware>();

            app.Run(async (context) =>
            {
                string nomeLoja = "Morumbi";
                IProducts productsClient = new ProductsClient();
                var respProdutos = productsClient.GetProducts(nomeLoja);

                IProduction productionClient = new ProductionClient();
                var respAreas = productionClient.GetAreas();


                var repository = new MemoryRepository(memoryCache);
                var produtoService = new ProductService(repository);
                produtoService.Salvar(await respProdutos);

                var productionService = new ProductionService(repository);
                productionService.Salvar(await respAreas);

                //await respProdutos;
                //await respAreas;

                ProductToGet p1 = produtoService.TesteGet(respProdutos.Result.First().ProductId.ToString());
                Areas p2 = productionService.TesteGet(respAreas.Result.First().ProductionId.ToString());

                ServiceBusEngine serviceBusEngine = new ServiceBusEngine();
                var connectionBus = Configuration["ServiceBusConnectionString"];
                var config = new QueueConfigurationEngineServiceBus
                {
                    ConnectionBus = connectionBus,
                    QueueName = null,
                    TopicName = "storecatalogready",
                    Subscripton = "store-catalog"
                };
                await serviceBusEngine.PublishMessage(config, System.Text.Json.JsonSerializer.Serialize(new StoreCatalogReady() { StoreName = nomeLoja, Ready = true }));
                await context.Response.WriteAsync($"Existem {respProdutos.Result.Count} produtos disponiveis na loja {nomeLoja}");


                await context.Response.WriteAsync($"Existem {respProdutos.Result.Count} produtos disponiveis e {respAreas.Result.Count} areas cadastradas na loja {nomeLoja} \n {p1.Name} \n {p2.ProductionId} - {p2.On}");
                //await context.Response.WriteAsync("StoreCatalog is runnning...");

            });
          
        }

    }
}
