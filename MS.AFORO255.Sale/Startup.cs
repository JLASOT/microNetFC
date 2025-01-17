using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MS.AFORO255.Sale.Services;
using MS.AFORO255.Sale.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aforo255.Cross.Event.Src;
using MediatR;
using System.Reflection;
using MS.AFORO255.Sale.Messages.Commands;
using MS.AFORO255.Sale.Messages.CommandHandlers;
using MS.AFORO255.Sale.Messages.Events;
using Aforo255.Cross.Http.Src;

namespace MS.AFORO255.Sale
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
            services.AddDbContext<ContextDatabase>(
              options =>
              {
                  options.UseMySQL(Configuration["mysql:cn"]);
                  //options.UseNpgsql(Configuration["postgres:cn"]);
                  //options.UseNpgsql(Configuration["cnpostgressales"]);
              });
            /*Start RabbitMQ*/
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);
            services.AddRabbitMQ();
            services.AddTransient<IRequestHandler<SaleCreateCommand, bool>, SaleCommandHandler>();
            //services.AddTransient<IRequestHandler<NotificationCreateCommand, bool>, NotificationCommandHandler>();
            /*End RabbitMQ*/
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddProxyHttp();

            services.AddScoped<PdfService>();
            services.AddScoped<EmailService>();
            /*Start - Tracer distributed*/
            //services.AddJaeger();
            services.AddOpenTracing();
            //services.AddSingleton<IConfiguration>(Configuration);
            /*End - Tracer distributed*/
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
        }
    }
}
