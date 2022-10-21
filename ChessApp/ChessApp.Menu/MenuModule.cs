using ChessApp.Menu.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ChessApp.Menu
{
    public class MenuModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MenuModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Index));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Index>();
            containerRegistry.RegisterForNavigation<Settings>();
        }
    }
}