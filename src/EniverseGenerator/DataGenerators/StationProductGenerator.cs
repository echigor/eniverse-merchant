using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eniverse.ServerModel;

namespace Eniverse.DataGenerators
{
    public class StationProductGenerator
    {
        private Random _random;

        private int[] _productIndexes = new int[ProductGenerator.TotalProducts];

        private decimal[] _basePrices = new decimal[ProductGenerator.TotalProducts]
        {
            100M, 4_300M, 180M, 140M, 273M,
            1_750M, 414M, 940M, 740M, 480M,
            1_114M, 812M, 1_043M, 2_100M, 2_163M,
            11_341M, 11_916M, 3_416M, 36_716M, 50_000M,
            6_815M, 617M, 27_316M, 19_817M, 61_532M,
            216M, 71_514M, 41_320M, 29_791M, 44_320M,
            290M, 2_915M, 941M, 6_319M, 5_140M,
            50M, 10M, 7_216M, 21_517M, 5_516M,
            112M, 1_218M, 71M, 15M, 5M,
            34M, 3M, 7M, 39M,
        };

        public StationProductGenerator()
        {
            Initialize();

            _random = new Random(42);
        }

        private void Initialize()
        {
            for (int i = 0; i < _productIndexes.Length; i++)
            {
                _productIndexes[i] = i;
            }
        }

        public List<StationProduct> GenerateNext(int stationID)
        {
            List<StationProduct> result = new List<StationProduct>();
            int productCount = _random.Next(5, 16);

            int[] randomProductIndexes = new int[productCount];

            for (int i = 0; i < randomProductIndexes.Length; i++)
            {
                int randomIndex = _random.Next(i, _productIndexes.Length);
                int randomProductIndex = _productIndexes[randomIndex];
                _productIndexes[randomIndex] = _productIndexes[i];
                _productIndexes[i] = randomProductIndex;
                randomProductIndexes[i] = randomProductIndex;

                StationProduct stationProduct = new StationProduct();
                stationProduct.StationID = stationID;
                stationProduct.ProductID = randomProductIndex + 1;
                stationProduct.AvailableVolume = (short)_random.Next(1_000, 10_001);
                stationProduct.Price = Math.Round(_basePrices[randomProductIndex] * (decimal)(_random.NextDouble() * 1.5D + 0.25D), 2);

                result.Add(stationProduct);
            }

            return result;
        }
    }
}
