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
        public Eniverse.ClientModel.Station GetStationByID(int id)
        {
            Station station = _database.GetStationByID(id);

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

        [HttpGet("products-by-station-id")]
        public IEnumerable<Eniverse.ClientModel.Product> GetProducts(int stationID)
        {
            IEnumerable<StationProduct> stationProduct = _database.GetProductsByStationID(stationID);

            return stationProduct.Select(x => new Eniverse.ClientModel.Product()
            {
                ID = x.ProductID,
                Name = x.Product.Name,
                AvailableVolume = x.AvailableVolume,
                Price = x.Price                
            }).ToList();
        }
    }
}