using System;
using System.Collections.Generic;
using System.Linq;

using Eniverse.DataGenerators;
using Eniverse.ServerModel;

using Microsoft.EntityFrameworkCore;

namespace Eniverse
{
    class Program
    {
        static void Main(string[] args)
        {
            using (DatabaseContext database = new DatabaseContext())
            {
                database.Database.EnsureDeleted();
                database.Database.EnsureCreated();

                database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                database.ChangeTracker.AutoDetectChangesEnabled = false;

                GenerateSystems(database);
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                database.ChangeTracker.AutoDetectChangesEnabled = false;

                GenerateProducts(database);
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                database.ChangeTracker.AutoDetectChangesEnabled = false;

                GeneratePlanets(database);
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                database.ChangeTracker.AutoDetectChangesEnabled = false;

                GenerateStations(database);
            }

            using (DatabaseContext database = new DatabaseContext())
            {
                database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                database.ChangeTracker.AutoDetectChangesEnabled = false;

                GenerateStationProducts(database);
            }

            Console.WriteLine("Jobs done!");
            Console.ReadLine();
        }

        static void GenerateSystems(DatabaseContext database)
        {
            StarSystemGenerator starSystemGenerator = new StarSystemGenerator();
            Console.WriteLine("Star system generation started");

            for (int i = 1; i <= StarSystemGenerator.TotalStars; i++)
            {

                StarSystem starSystem = starSystemGenerator.GenerateNext();
                database.StarSystems.Add(starSystem);

                if ((i % 1000 == 0) || i == StarSystemGenerator.TotalStars)
                {
                    database.SaveChanges();
                }

                WriteProgressBar(i, StarSystemGenerator.TotalStars);
            }

            Console.WriteLine($"Star systems successfully added to database: {DateTime.Now.TimeOfDay}\n");
        }

        static void GeneratePlanets(DatabaseContext database)
        {
            PlanetGenerator planetGenerator = new PlanetGenerator();
            Console.WriteLine("Planet generation started");

            for (int i = 1; i <= PlanetGenerator.TotalPlanets; i++)
            {

                Planet planet = planetGenerator.GenerateNext();
                database.Planets.Add(planet);

                if ((i % 1000 == 0) || i == PlanetGenerator.TotalPlanets)
                {
                    database.SaveChanges();
                }

                WriteProgressBar(i, PlanetGenerator.TotalPlanets);
            }

            Console.WriteLine($"Planets successfully added to database: {DateTime.Now.TimeOfDay}\n");
        }

        static void GenerateStations(DatabaseContext database)
        {
            StationGenerator stationGenerator = new StationGenerator();
            Console.WriteLine("Station generation started");

            for (int i = 1; i <= StationGenerator.TotalStations; i++)
            {
                Station station = stationGenerator.GenerateNext();
                database.Stations.Add(station);

                if ((i % 1000 == 0) || i == StationGenerator.TotalStations)
                {
                    database.SaveChanges();
                }

                WriteProgressBar(i, StationGenerator.TotalStations);
            }

            Console.WriteLine($"Stations successfully added to database: {DateTime.Now.TimeOfDay}\n");
        }

        static void GenerateProducts(DatabaseContext database)
        {
            ProductGenerator productGenerator = new ProductGenerator();
            Console.WriteLine("Product generation started");

            for (int i = 1; i <= ProductGenerator.TotalProducts; i++)
            {
                Product product = productGenerator.GenerateNext();
                database.Products.Add(product);
                database.SaveChanges(); //It is important to keep the order in which products are added
            }


            Console.WriteLine($"Products successfully added to database: {DateTime.Now.TimeOfDay}\n");
        }

        static void GenerateStationProducts(DatabaseContext database)
        {
            StationProductGenerator stationProductGenerator = new StationProductGenerator();
            Console.WriteLine("Station product generation started");

            for (int i = 1; i <= StationGenerator.TotalStations; i++)
            {
                List<StationProduct> stationProducts = stationProductGenerator.GenerateNext(i);

                foreach (var stationProduct in stationProducts)
                {
                    database.StationProducts.Add(stationProduct);
                }

                if ((i % 100 == 0) || i == StationGenerator.TotalStations)
                {
                    database.SaveChanges();
                }

                WriteProgressBar(i, StationGenerator.TotalStations);
            }

            Console.WriteLine($"Station products successfully added to database: {DateTime.Now.TimeOfDay}\n");
        }

        static void WriteProgressBar(int index, int totalCount)
        {
            if (index == 1)
            {
                Console.Write("[");
            }
            else if (index % (totalCount / 20) == 0)
            {
                Console.Write("=");
            }
            else if (index == totalCount)
            {
                Console.Write("]\n");
            }
        }
    }
}
