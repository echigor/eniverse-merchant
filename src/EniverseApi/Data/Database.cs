using System;
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

        public IEnumerable<StationProduct> GetProductsByStationID(int stationID)
        {
            return _databaseContext
                   .StationProducts
                   .Include(x => x.Product)
                   .Where(x => x.StationID == stationID);
        }
    }
}