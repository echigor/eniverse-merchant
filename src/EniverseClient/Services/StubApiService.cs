using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eniverse.ClientModel;

namespace Eniverse.Services
{
    public class StubApiService : IApiService
    {
        public StubApiService()
        {

        }

        public Station GetStationByID(int id)
        {
            return new Station()
            {
                ID = id,
                Name = "B-84154",
                PlanetName = "Beta-C845",
                StarSystemName = "Andromeda-3124",
                XCoordinate = 24_642_541D,
                YCoordinate = 41_642_541D,
                ZCoordinate = -24_642_541D
            };
        }
    }
}
