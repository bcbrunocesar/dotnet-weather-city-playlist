using Ingaia.Challenge.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ingaia.Challenge.WebApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CityRequestEntity> CitiesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var userEntityConfig = builder.Entity<UserEntity>();
            var cityRequestEntityConfig = builder.Entity<CityRequestEntity>();

            cityRequestEntityConfig.HasKey(x => x.Id);
            cityRequestEntityConfig.Ignore(x => x.ValidationResult);
            cityRequestEntityConfig.ToTable("CitiesRequests");

            userEntityConfig.HasKey(x => x.Id);
            userEntityConfig.Ignore(x => x.ValidationResult);
            userEntityConfig.ToTable("Users");

            base.OnModelCreating(builder);
        }
    }
}
