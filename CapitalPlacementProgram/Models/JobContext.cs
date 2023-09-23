using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CapitalPlacementProgram.Models
{
    public class JobContext: DbContext
    {
        private IConfiguration config;
        public DbSet<JobItem> JobItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseCosmos(
                config.GetValue<string>("CosmosDbHost"),
                config.GetValue<string>("CosmosDbKey"),
                databaseName: config.GetValue<string>("CosmosDbName"));
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer(config.GetValue<string>("CosmosDbContainer"));
        }
    }
}
