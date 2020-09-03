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
            builder.Entity<CityRequestEntity>().HasKey(x => x.Id);
            builder.Entity<CityRequestEntity>().ToTable("CitiesRequests");

            builder.Entity<UserEntity>().HasKey(x => x.Id);

            base.OnModelCreating(builder);
        }
    }
}
