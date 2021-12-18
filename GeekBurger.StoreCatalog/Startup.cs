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
using System.Text.Json;

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
            services.AddScoped<IMemoryCache, MemoryCache>();
            services.AddScoped<IMemoryRepository, MemoryRepository>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

                await respProdutos;
                //await respAreas;


                ServiceBusEngine serviceBusEngine = new ServiceBusEngine();
                var connectionBus = Configuration["ServiceBusConnectionString"];
                var config = new QueueConfigurationEngineServiceBus
                {
                    ConnectionBus = connectionBus,
                    QueueName = null,
                    TopicName = "storecatalogready",
                    Subscripton = "store-catalog"
                };
                await serviceBusEngine.PublishMessage(config, JsonSerializer.Serialize(new StoreCatalogReady() { StoreName = nomeLoja, Ready = true }));
                await context.Response.WriteAsync($"Existem {respProdutos.Result.Count} produtos disponiveis na loja {nomeLoja}");

            });
          
        }

    }
}
