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
                   .Distinct();
        }

        public IEnumerable<StationProduct> GetProductsByStationID(int stationID)
        {
            return _databaseContext
                   .StationProducts
                   .Include(x => x.Product)
                   .Where(x => x.StationID == stationID);
        }

        public StationProduct GetStationProduct(int stationID, int productID)
        {
            return _databaseContext
                   .StationProducts
                   .FirstOrDefault(x => x.StationID == stationID && x.ProductID == productID);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _databaseContext.Products;
        }

        public Merchant GetMerchantByID(int id)
        {
            return _databaseContext
                   .Merchants
                   .Include(x => x.Station).Include(x => x.Station.Planet).Include(x => x.Station.Planet.StarSystem)
                   .FirstOrDefault(x => x.ID == id);
        }

        public IEnumerable<MerchantProduct> GetMerchantProducts(int merchantID)
        {
            return _databaseContext
                   .MerchantProducts
                   .Include(x => x.Product)
                   .Where(x => x.MerchantID == merchantID);
        }

        public MerchantProduct GetMerchantProduct(int merchantID, int productID)
        {
            return _databaseContext
                   .MerchantProducts
                   .FirstOrDefault(x => x.ProductID == productID && x.MerchantID == merchantID);
        }

        public void SaveMerchantChanges(Merchant merchant)
        {
            _databaseContext.Merchants.Update(merchant);
            _databaseContext.SaveChanges();
        }

        public void SaveStationChanges(Station station)
        {
            _databaseContext.Stations.Update(station);
            _databaseContext.SaveChanges();
        }

        public void SaveMerchantProductsBuyChanges(MerchantProduct merchantProduct, bool isExists)
        {
            if (isExists)
            {
                _databaseContext.MerchantProducts.Update(merchantProduct);
            }
            else
            {
                _databaseContext.MerchantProducts.Add(merchantProduct);
            }

            _databaseContext.SaveChanges();
        }

        public void SaveMerchantProductsSellChanges(MerchantProduct merchantProduct, bool isProductSoldOut)
        {
            if (isProductSoldOut)
            {
                _databaseContext.MerchantProducts.Remove(merchantProduct);
            }
            else
            {
                _databaseContext.MerchantProducts.Update(merchantProduct);
            }

            _databaseContext.SaveChanges();
        }

        public void SaveStationProductsChanges(StationProduct stationProduct)
        {
            _databaseContext.StationProducts.Update(stationProduct);
            _databaseContext.SaveChanges();
        }
    }
}