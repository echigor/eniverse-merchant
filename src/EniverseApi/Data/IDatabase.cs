using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eniverse.ServerModel;

namespace EniverseApi.Data
{
    public interface IDatabase
    {
        public Station GetStationByID(int id);
        public IEnumerable<StationProduct> GetProductsByStationID(int stationID);
    }
}
