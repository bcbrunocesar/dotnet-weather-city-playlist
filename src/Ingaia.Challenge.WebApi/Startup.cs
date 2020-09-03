using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Interfaces;
using Ingaia.Challenge.WebApi.Repositories.CityRequestRepository;
using Ingaia.Challenge.WebApi.Repositories.UserRepository;
using Ingaia.Challenge.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

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

            ConfigureAuthentication(services);

            services.AddCors();
            services.AddControllers();
            services.AddMemoryCache();
            services.AddLogging();

            InitializeConfigModels(services);
            InitializeDI(services);

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo { Title = "Challenge InGaia", Version = "v1" });
            });
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge InGaia - V1");
                x.RoutePrefix = string.Empty;
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration["AuthConfig:SecretKey"]);
            services
                .AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    jwt.RequireHttpsMetadata = false;
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        private void InitializeDI(IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<IPlaylistService, PlaylistService>();
            services.AddTransient<IAppService, AppService>();
            services.AddTransient<ICityRequestRepository, CityRequestRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
        }

        private void InitializeConfigModels(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<AuthConfig>(Configuration.GetSection("AuthConfig"));
            services.Configure<WeatherForecastConfig>(Configuration.GetSection("OpenWeatherMapConfig"));
            services.Configure<PlaylistConfig>(Configuration.GetSection("PlaylistConfig"));
        }
    }
}
