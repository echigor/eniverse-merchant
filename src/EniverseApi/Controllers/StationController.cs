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

        [HttpGet("station-by-id")]
        public IActionResult GetStationByID(int id)
        {
            Station station = _database.GetStationByID(id);

            if (station == null)
            {
                return NoContent();
            }

            Planet planet = station.Planet;
            StarSystem starSystem = planet.StarSystem;

            return Ok(new Eniverse.ClientModel.Station()
            {
                ID = station.ID,
                Name = station.Name,
                PlanetName = planet.Name,
                StarSystemName = starSystem.Name,
                XCoordinate = starSystem.XCoordinate,
                YCoordinate = starSystem.YCoordinate,
                ZCoordinate = starSystem.ZCoordinate
            });
        }

        [HttpGet("filter")]
        public IActionResult GetStations(string starSystemName, string planetName, int productID, short minProductVolume)
        {
            return Ok(_database.GetStations(starSystemName, planetName, productID, minProductVolume).Select(x => new Eniverse.ClientModel.Station()
            {
                ID = x.ID,
                Name = x.Name,
                PlanetName = x.Planet.Name,
                StarSystemName = x.Planet.StarSystem.Name,
                XCoordinate = x.Planet.StarSystem.XCoordinate,
                YCoordinate = x.Planet.StarSystem.YCoordinate,
                ZCoordinate = x.Planet.StarSystem.ZCoordinate
            }).ToList());
        }
    }
}