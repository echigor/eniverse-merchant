using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseGenerator.Model;

namespace EniverseGenerator.DataGenerators
{
    public class PlanetGenerator
    {
        public const int TotalPlanets = 243_538;
        //public const int TotalPlanets = 2_435;

        //23
        private string[] _greekAlphabet = new string[]
        { 
            "Alpha", "Beta", "Gamma", "Delta",
            "Epsilon", "Zeta", "Eta", "Theta", 
            "Iota", "Kappa", "Lambda", "Mu",
            "Nu", "Xi", "Omicron", "Pi",
            "Rho", "Sigma", "Tau", "Upsilon",
            "Phi", "Psi", "Omega"
        };

        private Random _random;
        private int _planetCounter;

        public PlanetGenerator()
        {
            _planetCounter = 0;
            _random = new Random(42);
        }

        public Planet GenerateNext()
        {
            Planet result = new Planet();

            int greekLetterIndex = _planetCounter % _greekAlphabet.Length;
            char latinLetter = (char)(_planetCounter % 26 + 65);
            
            result.Name = _greekAlphabet[greekLetterIndex] + "-" + latinLetter + (_planetCounter % 899 + 100);
            result.StarSystemID = _random.Next(1, StarSystemGenerator.TotalStars + 1);

            _planetCounter++;

            return result;
        }
    }
}
