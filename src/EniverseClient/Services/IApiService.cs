using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eniverse.ClientModel;

namespace Eniverse.Services
{
    public interface IApiService
    {
        Task<Station> GetStationByIDAsync(int id);
        Task<List<Station>> GetStationsAsync(string starSystemName, string planetName, int productID, short minProductVolume);
        Task<List<Product>> GetProductsByStationIDAsync(int stationID);
        Task<List<ProductName>> GetProductNamesAsync();
        Task<Merchant> GetMerchantByIDAsync(int id);
        Task<List<Product>> GetMerchantProductsByMerchantIDAsync(int merchantID);
        Task<decimal> GetTravelCostAsync(int merchantID, int destinationStationID);
        Task<object> ChangeStationAsync(int merchantID, int destinationStationID);
        Task<object> BuyProductAsync(int merchantID, int productID, short tradedVolume);
        Task<object> SellProductAsync(int merchantID, int productID, short tradedVolume);
    }
}
