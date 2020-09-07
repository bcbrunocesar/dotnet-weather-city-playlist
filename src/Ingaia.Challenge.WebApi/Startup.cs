using Ingaia.Challenge.WebApi.Config;
using Ingaia.Challenge.WebApi.Context;
using Ingaia.Challenge.WebApi.Repositories.CityRequestRepository;
using Ingaia.Challenge.WebApi.Repositories.UserRepository;
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
using Ingaia.Challenge.WebApi.Services.WeatherForecastService;
using Ingaia.Challenge.WebApi.Services.UserService;
using Ingaia.Challenge.WebApi.Services.PlaylistService;
using Ingaia.Challenge.WebApi.Services.AppService;
using Ingaia.Challenge.WebApi.Infrastructure.Notificator;

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

            InitializeDatabase(app);

            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });

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

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();
        }

        private void InitializeDI(IServiceCollection services)
        {
            services.AddTransient<IWeatherForecastService, WeatherForecastService>();
            services.AddTransient<IPlaylistService, PlaylistService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAppService, AppService>();
            services.AddTransient<ICityRequestRepository, CityRequestRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<INotificator, Notificator>();

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
