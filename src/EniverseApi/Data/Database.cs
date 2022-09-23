﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eniverse.ServerModel;

using Microsoft.EntityFrameworkCore;

namespace EniverseApi.Data
{
    public class Database : IDatabase
    {
        private DatabaseContext _databaseContext;

        public Database(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext ?? throw new ArgumentNullException(nameof(databaseContext));
        }

        public Station GetStationByID(int id)
        {
            return _databaseContext
                   .Stations
                   .Include(x => x.Planet).Include(x => x.Planet.StarSystem)
                   .FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<Station> GetStations(string starSystemName, string planetName, int productID, short minProductVolume)
        {
            return _databaseContext
                   .StationProducts
                   .Include(x => x.Station).Include(x => x.Station.Planet).Include(x => x.Station.Planet.StarSystem)
                   .Where(x => string.IsNullOrEmpty(starSystemName) || x.Station.Planet.StarSystem.Name.StartsWith(starSystemName))
                   .Where(x => string.IsNullOrEmpty(planetName) || x.Station.Planet.Name.StartsWith(planetName))
                   .Where(x => (productID == 0) || x.ProductID == productID)
                   .Where(x => (minProductVolume == 0) || x.AvailableVolume >= minProductVolume)
                   .Select(x => x.Station)
                   .Distinct()
                   .Take(5_000);
        }

        public IEnumerable<StationProduct> GetProductsByStationID(int stationID)
        {
            return _databaseContext
                   .StationProducts
                   .Include(x => x.Product)
                   .Where(x => x.StationID == stationID);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _databaseContext.Products;
        }

    }
}