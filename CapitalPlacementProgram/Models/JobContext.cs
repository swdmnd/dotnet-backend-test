using Microsoft.EntityFrameworkCore;

namespace CapitalPlacementProgram.Models
{
    public class JobContext: DbContext
    {
        public DbSet<JobItem> JobItems { get; set; }

        // TODO: Read configuration from file
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseCosmos(
            "https://localhost:8081",
            "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
            databaseName: "CPJobs");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Jobs");
        }
    }
}
