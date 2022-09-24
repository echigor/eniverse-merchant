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
        private Product _product;
        public int ID
        {
            get { return _product.ID; }
        }

        public string Name
        {
            get { return _product.Name; }
        }

        public short Volume
        {
            get { return _product.AvailableVolume; }
        }

        public decimal Price
        {
            get { return _product.Price; }
        }

        public ProductViewModel(Product product)
        {
            _product = product ?? throw new ArgumentNullException(nameof(product));
        }
    }
}
