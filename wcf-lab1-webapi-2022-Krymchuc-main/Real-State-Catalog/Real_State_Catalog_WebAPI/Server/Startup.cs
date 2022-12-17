using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Real_State_Catalog_WebAPI.Data;
using Real_State_Catalog_WebAPI.Models;
using System.Reflection;

namespace Real_State_Catalog_WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Цей метод викликається середовищем виконання. Використовується цей метод, щоб додати служби до контейнера.
        public void ConfigureServices(IServiceCollection services)
        {
            // Встановлює інформацію про базу даних із файлу appsettings.json
            services.AddDbContext<AppContextDB>(options => options.UseSqlServer(Configuration.GetConnectionString("AppContextDB")));

            services.Configure<SecurityStampValidatorOptions>(o =>
                o.ValidationInterval = TimeSpan.FromSeconds(0));

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        // Цей метод викликається середовищем виконання. Використовується цей метод для налаштування конвеєра запиту HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
