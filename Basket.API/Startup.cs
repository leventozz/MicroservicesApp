using AutoMapper;
using Basket.API.Data;
using Basket.API.Data.Interfaces;
using Basket.API.Repositories;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API
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
            services.AddSingleton<ConnectionMultiplexer>(x =>
            {
                var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
                configuration.AbortOnConnectFail = false;
                return ConnectionMultiplexer.Connect(configuration);
            });
            services.AddControllers();
            services.AddTransient<IBasketContext, BasketContext>();
            services.AddTransient<IBasketRepository, BasketRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket API", Version = "v1" });
            });

            services.AddSingleton<IRabbitMQConnection>(x =>
           {
               var fac = new ConnectionFactory()
               {
                   HostName = Configuration["EventBus:HostName"]
               };

               if (!string.IsNullOrEmpty(Configuration["EventBus:UserName"]))
                   fac.UserName = Configuration["EventBus:UserName"];

               if (!string.IsNullOrEmpty(Configuration["EventBus:Password"]))
                   fac.Password = Configuration["EventBus:Password"];

               return new RabbitMQConnection(fac);
           });
            services.AddSingleton<EventBusRabbitMQProducer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
            });
        }
    }
}
