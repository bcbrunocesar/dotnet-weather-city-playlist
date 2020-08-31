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

        public DbSet<CityStatisticModel> CitiesStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CityStatisticModel>().HasKey(x => x.Id);
            builder.Entity<CityStatisticModel>().ToTable("CitiesStatistics");

            // Ms.Vs.Web.CodeGeneration.Design 3.1.4

            base.OnModelCreating(builder);
        }
    }
}
