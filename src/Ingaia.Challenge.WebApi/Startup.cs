using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Repositories;
using Ingaia.Challenge.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ingaia.Challenge.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration["SqlLiteConnectionString:IngaiaDb"]));

            services.AddControllers();
            services.AddMemoryCache();
            services.AddLogging();

            services.AddOptions();
            services.Configure<WeatherForecastConfig>(Configuration.GetSection("OpenWeatherMapConfig"));
            services.Configure<PlaylistConfig>(Configuration.GetSection("PlaylistConfig"));

            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<IPlaylistService, PlaylistService>();
            services.AddTransient<IAppService, AppService>();
            services.AddTransient<ICityRequestRepository, CityRequestRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseResponseCaching();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
