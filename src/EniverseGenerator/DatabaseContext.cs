using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseGenerator.Model;

using Microsoft.EntityFrameworkCore;

namespace EniverseGenerator
{
    public class DatabaseContext : DbContext
    {
        public DbSet<StarSystem> StarSystems { get; set; }
        public DbSet<Planet> Planets { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StationProduct> StationProducts { get; set; }

        public DatabaseContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=127.0.0.1\SQLEXPRESS,1433;Database=EniverseMerchant;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StationProduct>().HasKey(x => new { x.StationID, x.ProductID });
        }
    }
}
