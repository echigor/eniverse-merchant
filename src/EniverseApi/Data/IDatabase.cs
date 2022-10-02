using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Eniverse.ServerModel;

namespace EniverseApi.Data
{
    public interface IDatabase
    {
        Station GetStationByID(int id);
        IEnumerable<Station> GetStations(string starSystemName, string planetName, int productID, short minProductVolume);
        IEnumerable<StationProduct> GetProductsByStationID(int stationID);
        StationProduct GetStationProduct(int stationID, int productID);
        IEnumerable<Product> GetAllProducts();
        Merchant GetMerchantByID(int id);
        IEnumerable<MerchantProduct> GetMerchantProducts(int merchantID);
        MerchantProduct GetMerchantProduct(int merchantID, int productID);
        void SaveMerchantChanges(Merchant merchant);
        void SaveMerchantProductsBuyChanges(MerchantProduct merchantProduct, bool isExists);
        void SaveMerchantProductsSellChanges(MerchantProduct merchantProduct, bool isProductSoldOut);
        void SaveStationChanges(Station station);
        void SaveStationProductsChanges(StationProduct stationProduct);
    }
}
