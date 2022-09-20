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

        public Station GetBy(int id)
        {
            return _databaseContext
                .Stations
                .Include(x => x.Planet).Include(x => x.Planet.StarSystem)
                .FirstOrDefault(x => x.ID == id);
        }
    }
}