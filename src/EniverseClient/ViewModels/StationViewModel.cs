using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eniverse.ClientModel;
using Eniverse.ViewModels;

using Prism.Mvvm;

namespace Eniverse.ViewModels
{
    public class StationViewModel : BindableBase
    {
        private Station _station;

        public int ID
        {
            get { return _station.ID; }
        }

        public string Name
        {
            get { return _station.Name; }
        }

        public string PlanetName
        {
            get { return _station.PlanetName; }
        }

        public string StarSystemName
        {
            get { return _station.StarSystemName; }
        }

        private double _distance;
        public double Distance
        {
            get { return _distance; }
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
        }

        public StationViewModel(Station station, Station merchantStation)
        {
            _station = station ?? throw new ArgumentNullException(nameof(station));

            _products = new ObservableCollection<Product>();

            if (station.StarSystemName == merchantStation.StarSystemName)
            {
                _distance = 0D;
            }
            else
            {
                double deltaX = station.XCoordinate - merchantStation.XCoordinate;
                double deltaY = station.YCoordinate - merchantStation.YCoordinate;
                double deltaZ = station.ZCoordinate - merchantStation.ZCoordinate;

                _distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
            }
        }
    }
}
