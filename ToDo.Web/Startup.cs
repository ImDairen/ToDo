using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System.Globalization;
using System.IO;
using ToDo.Data;
using ToDo.Data.Interfaces;
using ToDo.Services;
using ToDo.Services.Interfaces;
using ToDo.Web.Infrastructure.Culture;
using ToDo.Web.Infrastructure.Extensions;
using ToDo.Web.Infrastructure.Logger;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;

namespace ToDo.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Db

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            #endregion

            #region DI

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddTransient<IStringLocalizer, CustomStringLocalizer>();
            services.AddTransient<DataSeeder>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<IDoService, DoService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            #endregion

            services.AddRazorPages();

            #region Localization

            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization()
                .AddViewLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ru")
                };

                options.DefaultRequestCulture = new RequestCulture("ru");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            DataSeeder dataSeeder,
            ILoggerManager logger
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseRequestLocalization();

            app.ConfigureCustomExceptionMiddleware();
            dataSeeder.Seed();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Do}/{action=Index}/{id?}");
            });
        }
    }
}
