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
        IEnumerable<Product> GetAllProducts();
        Merchant GetMerchantByID(int id);
        IEnumerable<MerchantProduct> GetMerchantProducts(int merchantID);
        void SaveMerchantChanges(Merchant merchant);
    }
}
