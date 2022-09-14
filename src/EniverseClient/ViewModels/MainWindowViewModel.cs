using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseClient.Models;
using EniverseClient.Services;

using Prism.Mvvm;

namespace EniverseClient.ViewModels
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
        }

        private ObservableCollection<ProductViewModel> _products;
        public ObservableCollection<ProductViewModel> Products
        {
            get { return _products; }
            set { _products = value; }
        }

        public MainWindowViewModel(IApiService apiService)
        {
            _title = "Eniverse Merchant";
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));

            Initialize();
        }

        private void Initialize()
        {
            Station station = _apiService.GetStationByID(1);
            _merchant = new MerchantViewModel(station);
        }

    }
}
