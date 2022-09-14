using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseClient.Models;

using Prism.Mvvm;

namespace EniverseClient.ViewModels
{
    public class ProductViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private short _availableVolume;
        public short AvailableVolume
        {
            get { return _availableVolume; }
            set { SetProperty(ref _availableVolume, value); }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        private Station _currentStation;
        public Station CurrentStation
        {
            get { return _currentStation; }
        }

        public ProductViewModel(Station station)
        {
            _currentStation = station ?? throw new ArgumentNullException(nameof(station));
        }
    }
}
