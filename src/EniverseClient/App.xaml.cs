using System.ComponentModel;
using System.Windows;

using Eniverse.Services;
using Eniverse.Views;

using Prism.Ioc;
using Prism.Unity;

namespace Eniverse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            MainWindow window = Container.Resolve<MainWindow>();
            return window;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IApiService>(new ApiService(@"http://localhost:8031/"));
        }
    }
}
