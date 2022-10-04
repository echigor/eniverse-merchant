using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#warning replace from view model
using System.Windows.Threading;

using Eniverse.ClientModel;
using Eniverse.Services;

using Prism.Commands;
using Prism.Mvvm;

namespace Eniverse.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        const short MaximumStorageVolume = short.MaxValue;

        private readonly IApiService _apiService;
        private ProductViewModel _sameProductInMarket;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private readonly MerchantViewModel _merchant;
        public MerchantViewModel Merchant
        {
            get { return _merchant; }
        }

        private ObservableCollection<StationViewModel> _stations;
        public ObservableCollection<StationViewModel> Stations
        {
            get { return _stations; }
        }

        private ObservableCollection<ProductName> _productNames;
        public ObservableCollection<ProductName> ProductNames
        {
            get { return _productNames; }
        }

        private ProductName _productFilter;
        public ProductName ProductFilter
        {
            get { return _productFilter; }
            set { SetProperty(ref _productFilter, value); }
        }

        private StationViewModel _observableStation;
        public StationViewModel ObservableStation
        {
            get { return _observableStation; }
            set { SetProperty(ref _observableStation, value, HandleObservableStationChanged); }
        }

        private ProductViewModel _selectedProductInMarket;
        public ProductViewModel SelectedProductInMarket
        {
            get { return _selectedProductInMarket; }
            set 
            { 
                SetProperty(ref _selectedProductInMarket, value, HandleSelectedProductInMarketChanged);
            }
        }

        private ProductViewModel _selectedProductInCargoHold;
        public ProductViewModel SelectedProductInCargoHold
        {
            get { return _selectedProductInCargoHold; }
            set 
            { 
                SetProperty(ref _selectedProductInCargoHold, value, HandleSelectedProductInCargoHoldChanged);
            }
        }

        private string _starSystemFilter;
        public string StarSystemFilter
        {
            get { return _starSystemFilter; }
            set { SetProperty(ref _starSystemFilter, value); }
        }

        private string _planetFilter;
        public string PlanetFilter
        {
            get { return _planetFilter; }
            set { SetProperty(ref _planetFilter, value); }
        }

        private short _productVolumeFilter;
        public short ProductVolumeFilter
        {
            get { return _productVolumeFilter; }
            set { SetProperty(ref _productVolumeFilter, value); }
        }

        private double _travelDistance;
        public double TravelDistance
        {
            get { return _travelDistance; }
            set { SetProperty(ref _travelDistance, value); }
        }

        private decimal _travelCost;
        public decimal TravelCost
        {
            get { return _travelCost; }
            set { SetProperty(ref _travelCost, value); }
        }

        private short _buyableVolume;
        public short BuyableVolume
        {
            get { return _buyableVolume; }
            set 
            { 
                SetProperty(ref _buyableVolume, value, UpdateTotalBuyPrice);
            }
        }

        private short _sellableVolume;
        public short SellableVolume
        {
            get { return _sellableVolume; }
            set 
            { 
                SetProperty(ref _sellableVolume, value, UpdateTotalSellPrice);
            }
        }

        private short _maximumSellableVolume;
        public short MaximumSellableVolume
        {
            get { return _maximumSellableVolume; }
            set 
            {
                SetProperty(ref _maximumSellableVolume, value);
            }
        }

        private short _maximumBuyableVolume;
        public short MaximumBuyableVolume
        {
            get { return _maximumBuyableVolume; }
            set 
            {
                SetProperty(ref _maximumBuyableVolume, value);
            }
        }

        private decimal _totalBuyPrice;
        public decimal TotalBuyPrice
        {
            get { return _totalBuyPrice; }
            set { SetProperty(ref _totalBuyPrice, value); }
        }

        private decimal _totalSellPrice;
        public decimal TotalSellPrice
        {
            get { return _totalSellPrice; }
            set { SetProperty(ref _totalSellPrice, value); }
        }

        private DelegateCommand _travelToStationCommand;
        public DelegateCommand TravelToStationCommand
        {
            get { return _travelToStationCommand; }
        }

        private DelegateCommand _setMaximumBuyableVolumeCommand;
        public DelegateCommand SetMaximumBuyableVolumeCommand
        {
            get { return _setMaximumBuyableVolumeCommand; }
        }

        private DelegateCommand _setMaximumSellableVolumeCommand;
        public DelegateCommand SetMaximumSellableVolumeCommand
        {
            get { return _setMaximumSellableVolumeCommand; }
        }

        private DelegateCommand _reduceSellableVolumeCommand;
        public DelegateCommand ReduceSellableVolumeCommand
        {
            get { return _reduceSellableVolumeCommand; }
        }

        private DelegateCommand _increaseSellableVolumeCommand;
        public DelegateCommand IncreaseSellableVolumeCommand
        {
            get { return _increaseSellableVolumeCommand; }
        }

        private DelegateCommand _reduceBuyableVolumeCommand;
        public DelegateCommand ReduceBuyableVolumeCommand
        {
            get { return _reduceBuyableVolumeCommand; }
        }

        private DelegateCommand _increaseBuyableVolumeCommand;
        public DelegateCommand IncreaseBuyableVolumeCommand
        {
            get { return _increaseBuyableVolumeCommand; }
        }

        private DelegateCommand _applyFilterCommand;
        public DelegateCommand ApplyFilterCommand
        {
            get { return _applyFilterCommand; }
        }

        private DelegateCommand _buyProductsCommand;
        public DelegateCommand BuyProductsCommand
        {
            get { return _buyProductsCommand; }
        }

        private DelegateCommand _sellProductsCommand;
        public DelegateCommand SellProductsCommand
        {
            get { return _sellProductsCommand; }
        }

        public MainWindowViewModel(IApiService apiService)
        {
            _title = "Eniverse Merchant";
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _stations = new ObservableCollection<StationViewModel>();
            _productNames = new ObservableCollection<ProductName>();
            _merchant = new MerchantViewModel();

            _applyFilterCommand = new DelegateCommand(async () => await ApplyFilterAsync());

            _travelToStationCommand = new DelegateCommand(async () => await TravelToStationAsync(), () =>
                TravelCost > 0 
                && TravelCost <= _merchant.Credits 
                && _observableStation != null)
                    .ObservesProperty(() => ObservableStation)
                    .ObservesProperty(() => TravelCost)
                    .ObservesProperty(() => Merchant.Credits);

            _reduceSellableVolumeCommand = new DelegateCommand(() => SellableVolume--, () => SellableVolume > 0).ObservesProperty(() => SellableVolume);
            _increaseSellableVolumeCommand = new DelegateCommand(() => SellableVolume++, () => SellableVolume < _maximumSellableVolume)
                .ObservesProperty(() => SellableVolume)
                .ObservesProperty(() => MaximumSellableVolume);
            _setMaximumSellableVolumeCommand = new DelegateCommand(() => SellableVolume = MaximumSellableVolume);

            _reduceBuyableVolumeCommand = new DelegateCommand(() => BuyableVolume--, () => BuyableVolume > 0).ObservesProperty(() => BuyableVolume);
            _increaseBuyableVolumeCommand = new DelegateCommand(() => BuyableVolume++, () => BuyableVolume < _maximumBuyableVolume)
                .ObservesProperty(() => BuyableVolume)
                .ObservesProperty(() => MaximumBuyableVolume);
            _setMaximumBuyableVolumeCommand = new DelegateCommand(() => BuyableVolume = MaximumBuyableVolume);

            _buyProductsCommand = new DelegateCommand(async () => await BuyProductsAsync(), CheckCanBuyProduct)
                .ObservesProperty(() => BuyableVolume)
                .ObservesProperty(() => SelectedProductInMarket);
            _sellProductsCommand = new DelegateCommand(async () => await SellProductsAsync(), CheckCanSellProduct)
                .ObservesProperty(() => SellableVolume)
                .ObservesProperty(() => SelectedProductInCargoHold);

            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(async () =>
            {
                List<ProductName> productNames = await _apiService.GetProductNamesAsync();
                IEnumerable<ProductName> sortedProducts = productNames.OrderBy(x => x.Name);
                _productNames.Clear();
                _productNames.Add(new ProductName() { ID = 0, Name = "(не выбран)" });
                ProductFilter = _productNames[0];
                _productNames.AddRange(sortedProducts);

                await UpdateMerchantAsync();

                StationViewModel stationViewModel = new StationViewModel(_merchant.CurrentStation, _merchant.CurrentStation);

                _stations.Add(stationViewModel);
            });
        }

        private void HandleObservableStationChanged()
        {
            if (_observableStation == null)
            {
                return;
            }

            Dispatcher.CurrentDispatcher.BeginInvoke(async () =>
            {
                List<Product> products = await _apiService.GetProductsByStationIDAsync(_observableStation.ID);
                IEnumerable<ProductViewModel> productViewModels = products.Select(x => new ProductViewModel(x));
                _observableStation.Products.Clear();
                _observableStation.Products.AddRange(productViewModels);

                TravelCost = await _apiService.GetTravelCostAsync(_merchant.ID, _observableStation.ID);
                TravelDistance = _observableStation.Distance;

                UpdateSameProduct();
                UpdateMaximumBuyableVolume();
            });

        }

        private async Task ApplyFilterAsync()
        {
            int productFilterID = 0;

            if (_productFilter != null)
            {
                productFilterID = _productFilter.ID;
            }

            List<Station> stations = await _apiService.GetStationsAsync(_starSystemFilter, _planetFilter, productFilterID, _productVolumeFilter);

            _stations.Clear();

            foreach (var station in stations)
            {
                StationViewModel stationViewModel = new StationViewModel(station, _merchant.CurrentStation);
                _stations.Add(stationViewModel);
            }
        }

        private async Task TravelToStationAsync()
        {
            if (_observableStation == null)
            {
                return;
            }

            await _apiService.ChangeStationAsync(_merchant.ID, _observableStation.ID);

            await UpdateMerchantAsync();

            await ApplyFilterAsync();
        }

        private async Task UpdateMerchantAsync()
        {
            Merchant merchant = await _apiService.GetMerchantByIDAsync(42);

            Station startStation = await _apiService.GetStationByIDAsync(merchant.CurrentStationID);

            List<Product> merchantProducts = await _apiService.GetMerchantProductsByMerchantIDAsync(merchant.ID);

            _merchant.Update(merchant, startStation, merchantProducts);
        }

        private async Task UpdateObservableStation()
        {
            Station observableStation = await _apiService.GetStationByIDAsync(_observableStation.ID);

            ObservableStation = new StationViewModel(observableStation, _merchant.CurrentStation);
        }

        private async Task BuyProductsAsync()
        {
            if (_merchant?.CurrentStation.ID != _observableStation?.ID 
                && _selectedProductInMarket != null
                && _merchant?.Credits < _totalBuyPrice 
                && _maximumBuyableVolume < _buyableVolume
                && _selectedProductInMarket.Volume < _buyableVolume)
            {
                return;
            }

            await _apiService.BuyProductAsync(Merchant.ID, _selectedProductInMarket.ID, BuyableVolume);

            await UpdateMerchantAsync();
            await UpdateObservableStation();
        }

        private async Task SellProductsAsync()
        {
            UpdateSameProduct();

            if (_merchant.CurrentStation?.ID != _observableStation?.ID
                && _selectedProductInCargoHold != null
                && _maximumSellableVolume < _sellableVolume
                && _selectedProductInCargoHold.Volume < _sellableVolume
                && _sameProductInMarket != null)
            {
                return;
            }

            await _apiService.SellProductAsync(Merchant.ID, _selectedProductInCargoHold.ID, SellableVolume);

            await UpdateMerchantAsync();
            await UpdateObservableStation();
        }

        private void UpdateSameProduct()
        {
            MaximumSellableVolume = 0;

            if (_observableStation != null && _selectedProductInCargoHold != null)
            {
                _sameProductInMarket = _observableStation.Products.FirstOrDefault(x => x.Name == _selectedProductInCargoHold.Name);

                if (_sameProductInMarket == null)
                {
                    return;
                }

                short availableVolumeInMarket = (short)(MaximumStorageVolume - _sameProductInMarket.Volume);

                if (_selectedProductInCargoHold.Volume <= availableVolumeInMarket)
                {
                    MaximumSellableVolume = _selectedProductInCargoHold.Volume;
                }
                else
                {
                    MaximumSellableVolume = availableVolumeInMarket;
                }
            }
        }

        private void UpdateMaximumBuyableVolume()
        {
            MaximumBuyableVolume = 0;

            //Необходимо реализовать повторное выделение товара в списке
            if (_observableStation != null && _selectedProductInMarket != null)
            {
                int buyableVolumeByCredits = (int)(_merchant.Credits / _selectedProductInMarket.Price);

                if (_selectedProductInMarket.Volume <= _merchant.AvailableCargoHoldVolume)
                {
                    MaximumBuyableVolume = _selectedProductInMarket.Volume;
                }
                else if(_merchant.AvailableCargoHoldVolume >= buyableVolumeByCredits)
                {
                    MaximumBuyableVolume = (short)buyableVolumeByCredits;
                }
                else
                {
                    MaximumBuyableVolume = _merchant.AvailableCargoHoldVolume;
                }
            }

        }

        private void UpdateTotalBuyPrice()
        {
            if (_observableStation != null && _selectedProductInMarket != null)
            {
                TotalBuyPrice = _selectedProductInMarket.Price * _buyableVolume;
            }
        }

        private void UpdateTotalSellPrice()
        {
            if (_sameProductInMarket != null && _observableStation != null && _selectedProductInCargoHold != null)
            {
                TotalSellPrice = _sameProductInMarket.Price * _sellableVolume;
            }
        }

        private void HandleSelectedProductInMarketChanged()
        {
            UpdateMaximumBuyableVolume();
            UpdateTotalBuyPrice();
        }

        private void HandleSelectedProductInCargoHoldChanged()
        {
            UpdateSameProduct();
            UpdateTotalSellPrice();
            SellProductsCommand.RaiseCanExecuteChanged();
        }

        private bool CheckCanBuyProduct()
        {
            bool canBuyProduct = _merchant?.CurrentStation.ID == _observableStation?.ID
                                  && _selectedProductInMarket != null
                                  && _merchant.Credits >= _totalBuyPrice
                                  && _buyableVolume > 0
                                  && _maximumBuyableVolume >= _buyableVolume
                                  && _selectedProductInMarket.Volume >= _buyableVolume;

            return canBuyProduct;
        }

        private bool CheckCanSellProduct()
        {
            bool canSellProduct = _merchant?.CurrentStation.ID == _observableStation?.ID
                                   && _selectedProductInCargoHold != null
                                   && _sellableVolume > 0
                                   && _maximumSellableVolume >= _sellableVolume
                                   && _selectedProductInCargoHold.Volume >= _sellableVolume;

            return canSellProduct;
        }
    }
}
