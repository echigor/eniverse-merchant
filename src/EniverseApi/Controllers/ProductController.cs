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
    public class ProductController : ControllerBase
    {
        private IDatabase _database;

        public ProductController(IDatabase database)
        {
            _database = database;
        }

        [HttpGet("list-by-station-id")]
        public IActionResult GetProducts(int stationID)
        {
            IEnumerable<StationProduct> stationProducts = _database.GetProductsByStationID(stationID);

            return Ok(stationProducts.Select(x => new Eniverse.ClientModel.Product()
            {
                ID = x.ProductID,
                Name = x.Product.Name,
                AvailableVolume = x.AvailableVolume,
                Price = x.Price
            }).ToList());
        }

        [HttpGet("names")]
        public IActionResult GetNames()
        {
            return Ok(_database.GetAllProducts().Select(x => new Eniverse.ClientModel.ProductName()
            {
                ID = x.ID,
                Name = x.Name
            }).ToList());
        }
    }
}