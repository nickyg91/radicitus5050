using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Radicitus.Data.Contexts.Raffles.Entities;
using System.IO;

namespace Radicitus.Data.Contexts.Raffles
{
    public class RadicitusDbContext : DbContext
    {
        private readonly string _connectionString;

        public RadicitusDbContext(DbContextOptions<RadicitusDbContext> options) : base(options)
        {
            var configuration = new ConfigurationBuilder();
            var builtConfig = configuration.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            _connectionString = builtConfig.GetConnectionString("radicitus-5050");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connectionString, builder =>
                {
                    builder.MigrationsAssembly("Radicitus.Raffle");
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("rad");
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<RadRaffle> Raffles { get; set; }
        public DbSet<RaffleNumber> RaffleNumbers { get; set; }
    }
}
