using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eniverse.ClientModel;

using Prism.Mvvm;

namespace Eniverse.ViewModels
{
    public class ProductViewModel : BindableBase
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private short _volume;
        public short Volume
        {
            get { return _volume; }
            set { SetProperty(ref _volume, value); }
        }

        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }

        public ProductViewModel()
        {
        }
    }
}
