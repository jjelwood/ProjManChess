using ChessApp.Game.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace ChessApp.Game
{
    public class GameModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public GameModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            _regionManager.RegisterViewWithRegion<Board>("ContentRegion");
        }
    }
}