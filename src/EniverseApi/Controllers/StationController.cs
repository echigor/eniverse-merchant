using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eniverse;

using Eniverse.ServerModel;

using EniverseApi.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EniverseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StationController : ControllerBase
    {
        private IDatabase _database;

        public StationController(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpGet("by-id")]
        public Eniverse.ClientModel.Station GetByID(int id)
        {
            Station station = _database.GetBy(id);

            Planet planet = station.Planet;
            StarSystem starSystem = planet.StarSystem;

            return new Eniverse.ClientModel.Station()
            {
                ID = station.ID,
                Name = station.Name,
                PlanetName = planet.Name,
                StarSystemName = starSystem.Name,
                XCoordinate = starSystem.XCoordinate,
                YCoordinate = starSystem.YCoordinate,
                ZCoordinate = starSystem.ZCoordinate
            };
        }
    }
}