using System;
using System.Collections.ObjectModel;
using System.Linq;

using Eniverse.ClientModel;

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
            private set { SetProperty(ref _currentStation, value); }
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

        //private ProductViewModel _selectedProductInStorage;
        //public ProductViewModel SelectedProductInStorage
        //{
        //    get { return _selectedProductInStorage; }
        //    set { SetProperty(ref _selectedProductInStorage, value); }
        //}

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
            set { SetProperty(ref _availableStarshipStorage, value); }
        }

        private short _fuelTank;
        public short FuelTank
        {
            get { return _fuelTank; }
            set { SetProperty(ref _fuelTank, value); }
        }


        public MerchantViewModel(Station station)
        {
            _currentStation = station ?? throw new ArgumentNullException(nameof(station));

            _credits = 10_000M;
            _products = new ObservableCollection<ProductViewModel>();

            _starshipName = "Ant-mk3";
            _starshipStorage = 1_000;

            Product fuel = new Product()
            {
                ID = 0,
                AvailableVolume = 32,
                Name = "Топливо",
                Price = 0
            };

            ProductViewModel fuelViewModel = new ProductViewModel(fuel);
            _products.Add(fuelViewModel);

            CountAvailableFuel();
            CountCurrentStarchipStorage();
        }
         
        private void CountCurrentStarchipStorage()
        {
            _currentStarshipStorage = 0;

            foreach (var product in _products)
            {
                CurrentStarshipStorage += product.Volume;
            }

            AvailableStarshipStorage = _starshipStorage - _currentStarshipStorage; 
        }

        private void CountAvailableFuel()
        {
            if (_products.Any(x => x.Name == "Топливо"))
            {
                FuelTank = _products.First(x => x.Name == "Топливо").Volume;
            }
            else
            {
                FuelTank = 0;
            }
        }

        public void ChangeStation(Station destination)
        {
            CurrentStation = destination ?? throw new ArgumentNullException(nameof(destination));
        }
    }
}
