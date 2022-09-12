using ChessApp.Core;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessApp.Menu.ViewModels
{
    public class SettingsViewModel : BindableBase
    {
        private readonly IApplicationCommands _applicationCommands;

        public SettingsViewModel(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
        }

        public IApplicationCommands ApplicationCommands
        {
            get { return _applicationCommands; }
        }

        public static Dictionary<string, (string BoardColourOne, string BoardColourTwo)> BoardThemes => Config.boardThemes;

        private KeyValuePair<string, (string BoardColourOne, string BoardColourTwo)> _selectedBoardTheme;
        public KeyValuePair<string, (string BoardColourOne, string BoardColourTwo)> SelectedBoardTheme
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

        void ExecuteSetSettings()
        {
            Config.BoardTheme = SelectedBoardTheme.Value;
            Config.PieceSpriteName = SelectedPieceSpriteName;
            Config.BoardFlipsBetweenMoves = FlipBoardBetweenMoves;
            _applicationCommands.SaveSettingsCommand.Execute(null);
        }
    }
}
