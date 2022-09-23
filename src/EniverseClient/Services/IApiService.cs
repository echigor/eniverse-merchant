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
        Task<List<Product>> GetProductsByStationIDAsync(int id);
    }
}
