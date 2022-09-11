using System;
using System.Collections.Generic;
using System.Linq;

using EniverseGenerator.DataGenerators;
using EniverseGenerator.Model;

using Microsoft.EntityFrameworkCore;

namespace EniverseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            using(DatabaseContext database = new DatabaseContext())
            {
                database.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                GenerateSystems(database);
                GeneratePlanets(database);
                GenerateStations(database);
                GenerateProducts(database);
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
            }

            Console.WriteLine("Star systems successfully added to database\n");
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
            }

            Console.WriteLine("Planets successfully added to database\n");
        }

        static void GenerateStations(DatabaseContext database)
        {
            StationGenerator stationGenerator = new StationGenerator();
            Console.WriteLine("Station generation started");

            for (int i = 0; i < StationGenerator.TotalStations; i++)
            {
                Station station = stationGenerator.GenerateNext();
                database.Stations.Add(station);

                if ((i % 1000 == 0) || i == StationGenerator.TotalStations)
                {
                    database.SaveChanges();
                }
            }

            Console.WriteLine("Stations successfully added to database\n");
        }

        static void GenerateProducts(DatabaseContext database)
        {
            ProductGenerator productGenerator = new ProductGenerator();
            Console.WriteLine("Product generation started");

            for (int i = 0; i < ProductGenerator.TotalProducts; i++)
            {
                Product product = productGenerator.GenerateNext();
                database.Products.Add(product);
            }

            database.SaveChanges();

            Console.WriteLine("Products successfully added to database\n");
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
            }

            Console.WriteLine("Station products successfully added to database\n");
        }
    }
}
