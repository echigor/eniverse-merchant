using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Eniverse.ClientModel;

using Prism.Mvvm;

namespace Eniverse.ViewModels
{
    public class MerchantViewModel : BindableBase
    {
        private Merchant _merchant;
        private Station _station;

        public int ID
        {
            get { return _merchant.ID; }
        }

        public Station CurrentStation
        {
            get { return _station; }
        }

        public decimal Credits
        {
            get { return _merchant.Credits; }
        }

        public string StarshipName
        {
            get { return _merchant.StarshipName; }
        }

        public decimal TravelExpenses
        {
            get { return _merchant.TravelExpenses; }
        }

        public short CargoHoldVolume
        {
            get { return _merchant.CargoHoldVolume; }
        }

        private short _currentCargoHoldVolume;
        public short CurrentCargoHoldVolume
        {
            get { return _currentCargoHoldVolume; }
            set { SetProperty(ref _currentCargoHoldVolume, value); }
        }

        private short _availableCargoHoldVolume;
        public short AvailableCargoHoldVolume
        {
            get { return _availableCargoHoldVolume; }
            set { SetProperty(ref _availableCargoHoldVolume, value); }
        }

        private ObservableCollection<ProductViewModel> _products;
        public ObservableCollection<ProductViewModel> Products
        {
            get { return _products; }
        }

        public MerchantViewModel()
        {
            _merchant = new Merchant();
            _station = new Station();
        }

        public void Update(Merchant merchant, Station station, IEnumerable<Product> merchantProducts)
        {
            _merchant = merchant ?? throw new ArgumentNullException(nameof(merchant));
            _station = station ?? throw new ArgumentNullException(nameof(station));
            _products = new ObservableCollection<ProductViewModel>();

            UpdateProducts(merchantProducts);

            RaisePropertyChanged(string.Empty);
        }
         
        private void CountCurrentCargoHoldVolume()
        {
            _currentCargoHoldVolume = 0;

            foreach (var product in _products)
            {
                CurrentCargoHoldVolume += product.Volume;
            }

            AvailableCargoHoldVolume = (short)(CargoHoldVolume - _currentCargoHoldVolume); 
        }


        public void UpdateProducts(IEnumerable<Product> merchantProducts)
        {
            if (merchantProducts != null)
            {
                _products.Clear();

                foreach (var product in merchantProducts)
                {
                    ProductViewModel productViewModel = new ProductViewModel(product);
                    _products.Add(productViewModel);
                }

                CurrentCargoHoldVolume = (short)_products.Count;

                CountCurrentCargoHoldVolume();
            }
        }
    }
}
