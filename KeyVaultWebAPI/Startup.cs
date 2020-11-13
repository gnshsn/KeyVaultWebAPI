using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KeyVaultWebAPI.Authentication;
using KeyVaultWebAPI.Controllers;
using KeyVaultWebAPI.Model;
using KeyVaultWebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KeyVaultWebAPI
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("https://localhost:44379");
                                  });
            });
            services.AddControllers();

            services.AddMvc(); //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

           services.AddDbContext<KeyvaultContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
           // services.AddDbContextPool<KeyvaultContext>(options =>
           //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // For Identity  
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<KeyvaultContext>()
                .AddDefaultTokenProviders();
            //services.AddAuthentication()
            //    .AddFacebook();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });
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

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                     .RequireCors(MyAllowSpecificOrigins);

                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=WeatherForecast}/{action=Index}/{id?}");
                //MapControllers();
                                                           //Route(
                                                           //name: "default",
                                                           //        pattern: "{controller}/{action}/{id?}")
            });
        }
    }
}
