using Aforo255.Cross.Token.Src;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace MS.AFORO255.Gateway
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
            //services.AddJwtCustomized();
            //services.AddOcelot();
            // Habilitar CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        //.WithOrigins("http://localhost:4200")
                        .WithOrigins("http://localhost:5244", "http://localhost:4200")// Cambia por la URL de tu frontend
                        .AllowAnyMethod()                      // Permitir cualquier método HTTP (GET, POST, PUT, DELETE)
                        .AllowAnyHeader();                     // Permitir cualquier encabezad
                });
            });

            services.AddJwtCustomized();
            services.AddOcelot();
        

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Habilitar CORS
            app.UseCors("AllowAll");
            app.UseOcelot().Wait();
        }
    }
}
