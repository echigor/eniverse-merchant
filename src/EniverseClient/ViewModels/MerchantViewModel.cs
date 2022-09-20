using System;
using System.Collections.ObjectModel;
using System.Linq;

using Eniverse.Model;

using Prism.Commands;
using Prism.Mvvm;

namespace Eniverse.ViewModels
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

        private string _starshipName;
        public string StarshipName
        {
            get { return _starshipName; }
            set { SetProperty(ref _starshipName, value); }
        }

        private int _starshipStorage;
        public int StarshipStorage
        {
            get { return _starshipStorage; }
            set { SetProperty(ref _starshipStorage, value); }
        }

        private int _currentStarshipStorage;
        public int CurrentStarshipStorage
        {
            get { return _currentStarshipStorage; }
            set { SetProperty(ref _currentStarshipStorage, value); }
        }

        private int _availableStarshipStorage;
        public int AvailableStarshipStorage
        {
            get { return _availableStarshipStorage; }
            set
            {
                //value = StarshipStorage - CurrentStarshipStorage;
                SetProperty(ref _availableStarshipStorage, value);
            }
        }

        private int _tradedVolume;
        public int TradedVolume
        {
            get { return _tradedVolume; }
            set { SetProperty(ref _tradedVolume, value); }
        }

        private short _fuelTank;
        public short FuelTank
        {
            get { return _fuelTank; }
            set { SetProperty(ref _fuelTank, value); }
        }

        private ProductViewModel _selectedProductInStorage;
        public ProductViewModel SelectedProductInStorage
        {
            get { return _selectedProductInStorage; }
            set { SetProperty(ref _selectedProductInStorage, value); }
        }

        private ProductViewModel _selectedProductInMarket;
        public ProductViewModel SelectedProductInMarket
        {
            get { return _selectedProductInMarket; }
            set { SetProperty(ref _selectedProductInMarket, value); }
        }

        private DelegateCommand _setZeroTradedVolume;
        public DelegateCommand SetZeroTradedVolume
        {
            get { return _setZeroTradedVolume; }
        }

        private DelegateCommand _setMaximumBuyableVolume;
        public DelegateCommand SetMaximumBuyableVolume
        {
            get { return _setMaximumBuyableVolume; }
        }

        private DelegateCommand _setMaximumSellableVolume;
        public DelegateCommand SetMaximumSellableVolume
        {
            get { return _setMaximumSellableVolume; }
        }



        public MerchantViewModel(Station station)
        {
            _currentStation = station ?? throw new ArgumentNullException(nameof(station));

            _credits = 10_000M;
            _products = new ObservableCollection<ProductViewModel>();

            ProductViewModel fuel = new ProductViewModel()
            {
                Name = "Топливо",
                Price = 4,
                Volume = 32
            };

            _products.Add(fuel);

            _starshipName = "Ant-mk3";
            _starshipStorage = 1_000;
            _currentStarshipStorage = 0;

            CountCurrentStarchipStorage();
            CountAvailableFuel();

            _tradedVolume = 0;
            AvailableStarshipStorage = _starshipStorage - _currentStarshipStorage;

            _setZeroTradedVolume = new DelegateCommand(() => TradedVolume = 0);
            _setMaximumBuyableVolume = new DelegateCommand(() => TradedVolume = _availableStarshipStorage);
            _setMaximumSellableVolume = new DelegateCommand(() => TradedVolume = _selectedProductInStorage.Volume);

        }

        private void CountCurrentStarchipStorage()
        {
            foreach (var product in _products)
            {
                _currentStarshipStorage += product.Volume;
            }
        }

        private void CountAvailableFuel()
        {
            if (_products.Any(x => x.Name == "Топливо"))
            {
                _fuelTank = _products.First(x => x.Name == "Топливо").Volume;
            }
            else
            {
                _fuelTank = 0;
            }
        }
    }
}
