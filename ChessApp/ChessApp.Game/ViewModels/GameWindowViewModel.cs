using ChessApp.Game.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessApp.Game.ViewModels
{
    public class GameWindowViewModel : BindableBase
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public GameWindowViewModel(IRegionManager regionManager)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _regionManager = regionManager;
        }

        private DelegateCommand _navigateToSettings;
        private readonly IRegionManager _regionManager;

        public DelegateCommand NavigateToSettings => _navigateToSettings ??= new DelegateCommand(ExecuteNavigateToSettings);

        void ExecuteNavigateToSettings()
        {
            _regionManager.RequestNavigate("ContentRegion", "Settings", new NavigationParameters()
            {
                { "navigationSender", nameof(GameWindow) }
            });
        }

        private DelegateCommand _exitToMenu;
        public DelegateCommand ExitToMenu => _exitToMenu ??= new DelegateCommand(ExecuteExitToMenu);

        void ExecuteExitToMenu()
        {
            _regionManager.RequestNavigate("ContentRegion", "Index");
        }
    }
}
