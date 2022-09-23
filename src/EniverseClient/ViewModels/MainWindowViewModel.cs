using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private MerchantViewModel _merchant;
        public MerchantViewModel Merchant
        {
            get { return _merchant; }
            set { SetProperty(ref _merchant, value); }
        }

        private ObservableCollection<StationViewModel> _stations;
        public ObservableCollection<StationViewModel> Stations
        {
            get { return _stations; }
        }

        private ObservableCollection<ProductName> _products;
        public ObservableCollection<ProductName> Products
        {
            get { return _products; }
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

        private bool _isSameSystem;
        public bool IsSameSystem
        {
            get { return _isSameSystem; }
            set { SetProperty(ref _isSameSystem, value); }
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
            _products = new ObservableCollection<ProductName>();

            _applyFilterCommand = new DelegateCommand(ApplyFilter);

            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(async () =>
            {
                Station startStation = await _apiService.GetStationByIDAsync(42);

                Merchant = new MerchantViewModel(startStation);

                StationViewModel stationViewModel = new StationViewModel(startStation, _merchant.CurrentStation);

                _stations.Add(stationViewModel);

                List<ProductName> productNames = await _apiService.GetProductNames();
                _products.Clear();
                _products.Add(new ProductName() { ID = 0, Name = "(не выбран)" });
                ProductFilter = _products[0];
                _products.AddRange(productNames);
            });
        }

        private void HandleObservableStationChanged()
        {
            if (_observableStation == null)
            {
                return;
            }

            if (_observableStation.Products.Count == 0)
            {
                Dispatcher.CurrentDispatcher.BeginInvoke(async () =>
                {
                    List<Product> products = await _apiService.GetProductsByStationIDAsync(_observableStation.ID);
                    _observableStation.Products.Clear();
                    _observableStation.Products.AddRange(products);
                });
            }
        }

        private void ApplyFilter()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(async () =>
            {
                int productFilterID = 0;

                if (_productFilter != null)
                {
                    productFilterID = _productFilter.ID;
                }

                List<Station> stations = await _apiService.GetStations(_starSystemFilter, _planetFilter, productFilterID, _productVolumeFilter);

                _stations.Clear();

                foreach (var station in stations)
                {
                    StationViewModel stationViewModel = new StationViewModel(station, _merchant.CurrentStation);
                    _stations.Add(stationViewModel);
                }
            });
        }
    }
}
