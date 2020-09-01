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

        public DbSet<CityRequestModel> CitiesStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CityRequestModel>().HasKey(x => x.Id);
            builder.Entity<CityRequestModel>().ToTable("CitiesRequests");
            
            base.OnModelCreating(builder);
        }
    }
}
