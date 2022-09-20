using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eniverse.ServerModel;

namespace Eniverse.DataGenerators
{
    public class StarSystemGenerator
    {
        public const int TotalStars = 53_141;
        //public const int TotalStars = 531;
        //79
        private string[] _starSystemNames = new string[]
        { 
            "Andromeda", "Antlia", "Apus", "Aquarius", "Aquila",
            "Ara", "Aries", "Auriga", "Botes", "Caelum",
            "Camelopardalis", "Cancer", "Canes", "Capricornus",
            "Carina", "Cassiopeia", "Centaurus", "Cepheus", "Cetus",
            "Chamaeleon", "Circinus", "Columba", "Corvus", "Crater",
            "Crux", "Cygnus", "Delphinus", "Dorado", "Draco",
            "Equuleus", "Eridanus", "Fornax", "Gemini", "Grus",
            "Hercules", "Horologium", "Hydra", "Hydrus", "Indus",
            "Lacerta", "Leo", "Lepus", "Libra", "Lupus",
            "Lynx", "Lyra", "Mensa", "Microscopium", "Monoceros",
            "Musca", "Norma", "Octans", "Ophiuchus", "Orion",
            "Pavo", "Pegasus", "Perseus", "Phoenix", "Pictor",
            "Pisces", "Puppis", "Pyxis", "Reticulum", "Sagitta",
            "Sagittarius", "Scorpius", "Sculptor", "Scutum",
            "Serpens", "Sextans", "Taurus", "Telescopium", "Triangulum",
            "Tucana", "Ursa", "Vela", "Virgo", "Volans", "Vulpecula"
        };

        private int _starSystemCounter;

        private Random _random;

        private double[][] _startingPoints;

        public StarSystemGenerator()
        {
            _starSystemCounter = 0;
            _random = new Random(42);
            _startingPoints = new double[_starSystemNames.Length][];
            InitializeStartingPoints();
        }

        private void InitializeStartingPoints()
        {
            for (int i = 0; i < _starSystemNames.Length; i++)
            {
                double xCoordinate = (_random.NextDouble() - 0.5D) * 9_000_000_000D ;     //~1000 l.y.
                double yCoordinate = (_random.NextDouble() - 0.5D) * 9_000_000_000D;
                double zCoordinate = (_random.NextDouble() - 0.5D) * 90_000_000D;

                _startingPoints[i] = new double[3] { xCoordinate, yCoordinate, zCoordinate };
            }
        }

        public StarSystem GenerateNext()
        {
            StarSystem result = new StarSystem();
            //для каждой системы сначала выбирается последовательно имя
            int starSystemNameIndex = _starSystemCounter % _starSystemNames.Length;
            string starSystemName = _starSystemNames[starSystemNameIndex];
            starSystemName += "-" + (_starSystemCounter % 8999 + 1000);
            
            result.Name = starSystemName;
            result.XCoordinate = Math.Round(_startingPoints[starSystemNameIndex][0] + (_random.NextDouble() - 0.5D) * 90_000_000D);
            result.YCoordinate = Math.Round(_startingPoints[starSystemNameIndex][1] + (_random.NextDouble() - 0.5D) * 90_000_000D);
            result.ZCoordinate = Math.Round(_startingPoints[starSystemNameIndex][2] + (_random.NextDouble() - 0.5D) * 10_000_000D);

            _starSystemCounter++;

            return result;
        }


    }
}
