using ChessApp.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessApp.Menu.ViewModels
{
    public class SettingsViewModel : BindableBase, INavigationAware
    {
        private readonly IApplicationCommands _applicationCommands;
        private readonly IRegionManager _regionManager;

        public SettingsViewModel(IApplicationCommands applicationCommands, IRegionManager regionManager)
        {
            _applicationCommands = applicationCommands;
            _regionManager = regionManager;
            applicationCommands.SaveSettingsCommand.RegisterCommand(Close);
        }

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
        }

        public static Dictionary<string, ColourPair> BoardThemes => Config.boardThemes;

        private KeyValuePair<string, ColourPair> _selectedBoardTheme = Config.DefaultTheme;
        public KeyValuePair<string, ColourPair> SelectedBoardTheme
        {
            get { return _selectedBoardTheme; }
            set { SetProperty(ref _selectedBoardTheme, value); }
        }

        public static List<string> PieceSpriteNames => Config.spriteNames;

        private string _selectedPieceSpriteName = Config.PieceSpriteName;
        public string SelectedPieceSpriteName
        {
            get { return _selectedPieceSpriteName; }
            set { SetProperty(ref _selectedPieceSpriteName, value); }
        }

        private bool _flipBoardBetweenMoves = Config.BoardFlipsBetweenMoves;
        public bool FlipBoardBetweenMoves
        {
            get { return _flipBoardBetweenMoves; }
            set { SetProperty(ref _flipBoardBetweenMoves, value); }
        }

        private DelegateCommand _setSettings;
        public DelegateCommand SetSettings => _setSettings ??= new DelegateCommand(ExecuteSetSettings);

        private string _navigationSender;
        public string NavigationSender
        {
            get { return _navigationSender; }
            set { SetProperty(ref _navigationSender, value); }
        }

        void ExecuteSetSettings()
        {
            Config.BoardTheme = SelectedBoardTheme.Value;
            Config.PieceSpriteName = SelectedPieceSpriteName;
            Config.BoardFlipsBetweenMoves = FlipBoardBetweenMoves;
            _applicationCommands.SaveSettingsCommand.Execute(null);
        }

        private DelegateCommand _close;
        public DelegateCommand Close => _close ??= new DelegateCommand(ExecuteClose);

        void ExecuteClose()
        {
            _regionManager.RequestNavigate("ContentRegion", NavigationSender);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            NavigationSender = navigationContext.Parameters.GetValue<string>("navigationSender");
            FlipBoardBetweenMoves = Config.BoardFlipsBetweenMoves;
            SelectedPieceSpriteName = Config.PieceSpriteName;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
