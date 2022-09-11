using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseGenerator.Model;

namespace EniverseGenerator.DataGenerators
{
    public class StationGenerator
    {
        public const int TotalStations = 783_541;
        //public const int TotalStations = 7_541;

        private Random _random;
        private int _stationCounter;

        public StationGenerator()
        {
            _stationCounter = 0;
            _random = new Random(42);
        }

        public Station GenerateNext()
        {
            Station result = new Station();
            char latinLetter = (char)(_stationCounter % 26 + 65);

            result.Name = latinLetter + "-" + (_stationCounter % 89_999 + 10_000);
            result.PlanetID = _random.Next(1, PlanetGenerator.TotalPlanets + 1);

            _stationCounter++;

            return result;
        }
    }
}