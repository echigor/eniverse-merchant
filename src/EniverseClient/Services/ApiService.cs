using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using Eniverse.ClientModel;

using Newtonsoft.Json;

namespace Eniverse.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;
        private const string StationController = "station";
        private const string ProductController = "product";
        private const string MerchantController = "merchant";

        public ApiService(string apiEndpoint)
        {
            SocketsHttpHandler socketsHttpHandler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(15)
            };

            _client = new HttpClient(socketsHttpHandler);

            if (string.IsNullOrWhiteSpace(apiEndpoint) || !Uri.IsWellFormedUriString(apiEndpoint, UriKind.RelativeOrAbsolute))
            {
                throw new UriFormatException(nameof(apiEndpoint));
            }

            Uri uriEndpoint = new Uri(apiEndpoint);
            _client.BaseAddress = uriEndpoint;
        }

        public Task<Station> GetStationByIDAsync(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(id), id }
            };

            Uri uri = EncodeUriWithParameters(StationController, "station-by-id", parameters);

            return RequestGet<Station>(uri);
        }

        public Task<List<Station>> GetStationsAsync(string starSystemName, string planetName, int productID, short minProductVolume)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(starSystemName), starSystemName },
                { nameof(planetName), planetName },
                { nameof(productID), productID },
                { nameof(minProductVolume), minProductVolume }
            };

            Uri uri = EncodeUriWithParameters(StationController, "filter", parameters);

            return RequestGet<List<Station>>(uri);
        }

        public Task<List<Product>> GetProductsByStationIDAsync(int stationID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(stationID), stationID }
            };

            Uri uri = EncodeUriWithParameters(ProductController, "list-by-station-id", parameters);

            return RequestGet<List<Product>>(uri);
        }

        public Task<List<ProductName>> GetProductNamesAsync()
        {
            Uri uri = EncodeUriWithParameters(ProductController, "names", null);

            return RequestGet<List<ProductName>>(uri);
        }

        public Task<Merchant> GetMerchantByIDAsync(int id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(id), id }
            };

            Uri uri = EncodeUriWithParameters(MerchantController, string.Empty, parameters);

            return RequestGet<Merchant>(uri);
        }

        public Task<List<Product>> GetMerchantProductsByMerchantIDAsync(int merchantID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(merchantID), merchantID }
            };

            Uri uri = EncodeUriWithParameters(MerchantController, "products", parameters);

            return RequestGet<List<Product>>(uri);
        }

        public Task<decimal> GetTravelCostAsync(int merchantID, int destinationStationID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(merchantID), merchantID },
                { nameof(destinationStationID), destinationStationID }
            };

            Uri uri = EncodeUriWithParameters(MerchantController, "travel-cost", parameters);

            return RequestGet<decimal>(uri);
        }

        public Task<object> ChangeStationAsync(int merchantID, int destinationStationID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(merchantID), merchantID },
                { nameof(destinationStationID), destinationStationID }
            };

            Uri uri = EncodeUriWithParameters(MerchantController, "change-station", parameters);

            return RequestPatch<object>(uri);
        }

        public Task<decimal> GetBuySellPriceAsync(int marketStationID, string tradedProductName, short tradedVolume)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(marketStationID), marketStationID },
                { nameof(tradedProductName), tradedProductName },
                { nameof(tradedVolume), tradedVolume }
            };

            Uri uri = EncodeUriWithParameters(MerchantController, "buy-sell-price", parameters);

            return RequestGet<decimal>(uri);
        }

        private async Task<TResult> RequestGet<TResult>(Uri uri) where TResult: new()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                TResult result = JsonConvert.DeserializeObject<TResult>(responseBody);

                return result;
            }
            catch (Exception exception) when (exception is HttpRequestException || exception is JsonException)
            {
                Debug.WriteLine("\nException caught!");
                Debug.WriteLine($"Message: {exception.Message}");
            }

            return new TResult();
        }

        private async Task<TResult> RequestPatch<TResult>(Uri uri) where TResult : new()
        {
            try
            {
                HttpContent httpContent = new StringContent(string.Empty);

                HttpResponseMessage response = await _client.PatchAsync(uri, httpContent);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                TResult result = JsonConvert.DeserializeObject<TResult>(responseBody);

                return result;
            }
            catch (Exception exception) when (exception is HttpRequestException || exception is JsonException)
            {
                Debug.WriteLine("\nException caught!");
                Debug.WriteLine($"Message: {exception.Message}");
            }

            return new TResult();
        }

        private Uri EncodeUriWithParameters(string controller, string method, Dictionary<string, object> parameters)
        {
            NameValueCollection namedParameters = HttpUtility.ParseQueryString(string.Empty);

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    namedParameters.Add(parameter.Key.ToLower(), parameter.Value?.ToString());
                }
            }

            string result = $"{controller}/{method}?{namedParameters.ToString()}";

            return new Uri(result, UriKind.Relative);
        }

    }
}
