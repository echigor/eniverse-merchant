using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

#warning replace from view model
using System.Windows.Threading;

using Eniverse.ClientModel;
using Eniverse.Services;

using Eniverse.ViewModels;

using Prism.Commands;
using Prism.Mvvm;

namespace Eniverse.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IApiService _apiService;

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
                SetProperty(ref _selectedProductInMarket, value);
            }
        }

        public bool IsSameSystem
        {
            get { return _observableStation?.StarSystemName == _merchant.CurrentStation.StarSystemName; }
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

        private int _tradedVolume;
        public int TradedVolume
        {
            get { return _tradedVolume; }
            set { SetProperty(ref _tradedVolume, value); }
        }

        private DelegateCommand _travelToStationCommand;
        public DelegateCommand TravelToStationCommand
        {
            get { return _travelToStationCommand; }
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

        private DelegateCommand _applyFilterCommand;
        public DelegateCommand ApplyFilterCommand
        {
            get { return _applyFilterCommand; }
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

            _setZeroTradedVolume = new DelegateCommand(() => TradedVolume = 0);
            _setMaximumBuyableVolume = new DelegateCommand(() => TradedVolume = _merchant.AvailableCargoHoldVolume);
           // _setMaximumSellableVolume = new DelegateCommand(() => TradedVolume = _merchant.SelectedProductInStorage.Volume);

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
    }
}
