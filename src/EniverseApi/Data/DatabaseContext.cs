using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eniverse.ServerModel;

using Microsoft.EntityFrameworkCore;

namespace EniverseApi.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<StarSystem> StarSystems { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StationProduct> StationProducts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
           // Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StationProduct>().HasKey(x => new { x.StationID, x.ProductID });
        }
    }
}
