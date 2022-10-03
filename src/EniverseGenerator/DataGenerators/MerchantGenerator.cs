using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eniverse.ServerModel;

namespace Eniverse.DataGenerators
{
    public class MerchantGenerator
    {
        public const int TotalMerchants = 100;

        private Random _random;

        public MerchantGenerator()
        {
            _random = new Random(42);
        }

        public Merchant GenerateNext()
        {
            Merchant result = new Merchant();

            int stationID = _random.Next(1, StationGenerator.TotalStations + 1);
            int travelExpenses = _random.Next(200, 301);

            result.Credits = 10_000;
            result.CurrentStationID = stationID;
            result.StarshipName = "Ant-mk3";
            result.CargoHoldVolume = 1000;
            result.TravelExpenses = travelExpenses;

            return result;
        }
    }
}
