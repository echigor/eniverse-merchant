using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

#warning replace from view model
using System.Windows.Threading;

using Eniverse.ClientModel;
using Eniverse.Services;

using Eniverse.ViewModels;

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
            set { SetProperty(ref _stations, value); }
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

        public MainWindowViewModel(IApiService apiService)
        {
            _title = "Eniverse Merchant";
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _stations = new ObservableCollection<StationViewModel>();

            Initialize();
        }

        private void Initialize()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(async () =>
            {
                Station startStation = await _apiService.GetStationByIDAsync(42);

                Merchant = new MerchantViewModel(startStation);

                StationViewModel stationViewModel = new StationViewModel(startStation, Merchant.CurrentStation);

                _stations.Add(stationViewModel);
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
    }
}
