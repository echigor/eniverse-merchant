using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseClient.Models;

using Prism.Mvvm;

namespace EniverseClient.ViewModels
{
    public class MerchantViewModel : BindableBase
    {
        private int _id;
        public int ID
        {
            get { return _id; }
        }

        private Station _currentStation;
        public Station CurrentStation
        {
            get { return _currentStation; }
            set { SetProperty(ref _currentStation, value); }
        }

        private decimal _credits;
        public decimal Credits
        {
            get { return _credits; }
            set { SetProperty(ref _credits, value); }
        }

        private ObservableCollection<ProductViewModel> _products;
        public ObservableCollection<ProductViewModel> Products
        {
            get { return _products; }
        }

        public MerchantViewModel(Station station)
        {
            _currentStation = station ?? throw new ArgumentNullException(nameof(station));

            _products = new ObservableCollection<ProductViewModel>();

            _credits = 10_000M;
        }
    }
}
