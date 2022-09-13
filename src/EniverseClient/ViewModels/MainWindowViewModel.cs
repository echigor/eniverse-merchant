using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EniverseClient.Services;

using Prism.Mvvm;

namespace EniverseClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IApiService _apiService;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IApiService apiService)
        {
            _title = "Eniverse Merchant";
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
        }
    }
}
