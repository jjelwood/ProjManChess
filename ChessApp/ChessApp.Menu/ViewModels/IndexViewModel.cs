using ChessApp.Business;
using ChessApp.Core;
using ChessApp.Game.Views;
using ChessApp.Menu.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChessApp.Menu.ViewModels
{
    public class IndexViewModel : BindableBase
    {
        public IndexViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            applicationCommands.SaveSettingsCommand.RegisterCommand(UpdateSettings);
            _applicationCommands = applicationCommands;
        }

        public string Colour1 => Config.BoardTheme.BoardColourOne;
        public string Colour2 => Config.BoardTheme.BoardColourTwo;
        public bool gameIsDefault = false;

        private DelegateCommand _navigateToSettings;
        private readonly IRegionManager _regionManager;
        private readonly IApplicationCommands _applicationCommands;

        public DelegateCommand NavigateToSettings => _navigateToSettings ??= new DelegateCommand(ExecuteNavigateToSettings);

        void ExecuteNavigateToSettings()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(Settings), new NavigationParameters()
            {
                { "navigationSender", nameof(Views.Index) }
            });
        }

        private DelegateCommand _continueGame;
        public DelegateCommand ContinueGame =>
            _continueGame ?? (_continueGame = new DelegateCommand(ExecuteContinueGame, CanExecuteContinueGame));

        void ExecuteContinueGame()
        {
            _regionManager.RequestNavigate("ContentRegion", nameof(GameWindow));
        }

        bool CanExecuteContinueGame()
        {
            return true;
        }

        private DelegateCommand _startNewGame;
        public DelegateCommand StartNewGame => _startNewGame ?? (_startNewGame = new DelegateCommand(ExecuteStartNewGame));

        void ExecuteStartNewGame()
        {
            _applicationCommands.NewGame.Execute(null);
            _regionManager.RequestNavigate("ContentRegion", nameof(GameWindow));
        }

        private DelegateCommand _updateSettings;
        public DelegateCommand UpdateSettings => _updateSettings ??= new DelegateCommand(ExecuteUpdateSettings);

        void ExecuteUpdateSettings()
        {
            RaisePropertyChanged(nameof(Colour1));
            RaisePropertyChanged(nameof(Colour2));
        }
    }
}
