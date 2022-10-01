using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eniverse.ServerModel;

using EniverseApi.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EniverseApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MerchantController : ControllerBase
    {
        private IDatabase _database;

        public MerchantController(IDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult GetMerchantByID(int id)
        {
            Merchant merchant = _database.GetMerchantByID(id);

            if (merchant == null)
            {
                return NoContent();
            }

            return Ok(new Eniverse.ClientModel.Merchant()
            {
                ID = merchant.ID,
                Credits = merchant.Credits,
                StarshipName = merchant.StarshipName,
                CargoHoldVolume = merchant.CargoHoldVolume,
                CurrentStationID = merchant.CurrentStationID
            });
        }

        [HttpGet("products")]
        public IActionResult GetMerchantProducts(int merchantID)
        {
            IEnumerable<MerchantProduct> merchantProducts = _database.GetMerchantProducts(merchantID);

            return Ok(merchantProducts.Select(x => new Eniverse.ClientModel.Product()
            {
                ID = x.ProductID,
                Name = x.Product.Name,
                AvailableVolume = x.Volume,
            }).ToList());
        }

        [HttpGet("travel-cost")]
        public IActionResult GetTravelCost(int merchantID, int destinationStationID)
        {
            Merchant merchant = _database.GetMerchantByID(merchantID);
            Station destinationStation = _database.GetStationByID(destinationStationID);

            if (merchant == null || destinationStation == null)
            {
                return NoContent();
            }

            decimal travelCost = CalculateTravelCost(merchant, destinationStation);

            return Ok(travelCost);
        }

        [HttpPatch("change-station")]
        public IActionResult ChangeStation(int merchantID, int destinationStationID)
        {
            Merchant merchant = _database.GetMerchantByID(merchantID);
            Station destinationStation = _database.GetStationByID(destinationStationID);

            if (merchant == null || destinationStation == null)
            {
                return NoContent();
            }

            decimal travelCost = CalculateTravelCost(merchant, destinationStation);

            if (travelCost > merchant.Credits)
            {
                return NoContent();
            }

            merchant.Station = destinationStation;
            merchant.Credits -= travelCost;

            _database.SaveMerchantChanges(merchant);

            return Ok();
        }

        private decimal CalculateTravelCost(Merchant merchant, Station destinationStation)
        {
            StarSystem merchantStarSystem = merchant.Station.Planet.StarSystem;
            StarSystem destinationStarSystem = destinationStation.Planet.StarSystem;

            double deltaX = destinationStarSystem.XCoordinate - merchantStarSystem.XCoordinate;
            double deltaY = destinationStarSystem.YCoordinate - merchantStarSystem.YCoordinate;
            double deltaZ = destinationStarSystem.ZCoordinate - merchantStarSystem.ZCoordinate;

            decimal distance = Math.Round((decimal)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ) / 9_460_800M, 4);

            return merchant.TravelExpenses * distance
                   + destinationStarSystem.PlanetDuty * (merchant.Station.PlanetID != destinationStation.PlanetID ? 1 : 0)
                   + destinationStarSystem.StationDuty * (merchant.Station.ID != destinationStation.ID ? 1 : 0);
        }
    }
}
