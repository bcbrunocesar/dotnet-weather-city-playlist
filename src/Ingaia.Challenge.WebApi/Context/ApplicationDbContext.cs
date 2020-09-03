using Ingaia.Challenge.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Ingaia.Challenge.WebApi.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<CityRequestModel> CitiesRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CityRequestModel>().HasKey(x => x.Id);
            builder.Entity<CityRequestModel>().ToTable("CitiesRequests");

            builder.Entity<UserModel>().HasKey(x => x.Id);

            base.OnModelCreating(builder);
        }
    }
}
