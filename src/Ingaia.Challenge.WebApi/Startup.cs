using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddControllers();
            services.AddResponseCaching();

            services.AddOptions();
            services.Configure<WeatherForecastConfig>(Configuration.GetSection("OpenWeatherMapConfig"));
            services.Configure<PlaylistConfig>(Configuration.GetSection("PlaylistConfig"));

            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<IPlaylistService, PlaylistService>();
            services.AddTransient<IAppService, AppService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
