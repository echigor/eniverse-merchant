using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseClient.Models;

namespace EniverseClient.Services
{
    public interface IApiService
    {
        Station GetStationByID(int id);
    }
}
