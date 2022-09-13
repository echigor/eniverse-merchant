using System.ComponentModel;
using System.Windows;

using EniverseClient.Services;
using EniverseClient.Views;

using Prism.Ioc;
using Prism.Unity;

namespace EniverseClient
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
            containerRegistry.RegisterInstance<IApiService>(new StubApiService());
        }
    }
}
