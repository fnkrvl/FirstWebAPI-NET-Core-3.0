using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstWebAPI.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FirstWebAPI.Helpers;


namespace FirstWebAPI
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

            services.AddScoped<FiltroDeAccion>();

            // Habilita la funcionalidad de servicios para la funcionalidad de guardar información en caché
            services.AddResponseCaching();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();            

            // Cuando se indique que hay una dependencia ICLaseB en cualquier lugar de nuestra aplicación, entonces la dependencia va a ser satisfecha con la clase ClaseB2
            // services.AddTransient<IClaseB, ClaseB2>();

            // CICLOS DE VIDA
            // 
            // AddTransient - Cada vez que un servicio sea solicitado, se va a servir una nueva instancia de la clase.
            // AddScoped    - Los servicios Scope son creados uno por petición HTTP. Si distintas clases piden el mismo servicio durante una petición HTTP,
            //                se les va a entregar la misma instancia del servicio.   
            // Singleton    - Siempre se nos va a dar la misma instancia. Sólo hay variación cuando el servidor es apagado y encendido nuevamente.
            //                Siempre vamos a tener la misma instancia de servicio siendo enviada una y otra vez a distintos servicios de nuestra aplicación,
            //                aún si es para servir a distintas peticiones HTTP de distintos usuarios

            // AddDbContext -> Método para configurar el servicio de un contexto de datos a usar. Internamente, AddDbContext usa la función AddScope
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc(options => options.Filters.Add(new FiltroDeExepcion()));
            // services.AddMvc(options => options.Filters.Add(typeof(FiltroDeExepcion))); -> Si hubiese inyección de dependencias
            

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseResponseCaching();

            app.UseAuthorization();

            app.UseMvc();
        }
    }

    
}
