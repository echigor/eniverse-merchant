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

        public Task<List<Product>> GetProductsByStationIDAsync(int stationID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { nameof(stationID), stationID }
            };

            Uri uri = EncodeUriWithParameters(StationController, "products-by-station-id", parameters);
            return RequestGet<List<Product>>(uri);
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

        private Uri EncodeUriWithParameters(string controller, string method, Dictionary<string, object> parameters)
        {
            NameValueCollection namedParameters = HttpUtility.ParseQueryString(string.Empty);

            foreach (var parameter in parameters)
            {
                namedParameters.Add(parameter.Key.ToLower(), parameter.Value?.ToString());
            }

            string result = $"{controller}/{method}?{namedParameters.ToString()}";

            return new Uri(result, UriKind.Relative);
        }
    }
}
