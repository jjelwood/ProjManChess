using ChessApp.Business;
using ChessApp.Business.Pieces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessApp.Game.ViewModels
{
    public class BoardViewModel : BindableBase
    {
        public const string boardColour1 = "#769656";
        public const string boardColour2 = "#eeeed2";
        public BoardViewModel()
        {
            _board = new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8);
        }

        private ChessBoard _board;
        public ChessBoard Board
        {
            get { return _board; }
            set { SetProperty(ref _board, value, () => RaisePropertyChanged(nameof(JaggedArrayBoard))); }
        }

        public IPiece?[][] JaggedArrayBoard => Board.GetJaggedArray();

        private IPiece? _selectedPiece;
        public IPiece? SelectedPiece
        {
            get { return _selectedPiece; }
            set { SetProperty(ref _selectedPiece, value, () => RaisePropertyChanged(nameof(TileMoveableArray))); }
        }

        public string[][] BoardColours
        {
            get
            {
                string[][] result = new string[Board.Rows][];
                for (int i = 0; i < Board.Rows; i++)
                {
                    result[i] = new string[Board.Columns];
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        result[i][j] = i % 2 == 0 ^ j % 2 == 0 ? boardColour1 : boardColour2;
                    }
                }
                return result;
            }
        }

        public BoolTilePair[][]? TileMoveableArray
        {
            get
            {
                BoolTilePair[][] result = new BoolTilePair[Board.Rows][];
                var tiles = SelectedPiece?.GetMoveableTiles(Board);
                if (tiles is null)
                {
                    return null;
                }
                for (int i = 0; i < Board.Rows; i++)
                {
                    result[i] = new BoolTilePair[Board.Columns];
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        var tile = new Tile(i, j);
                        result[i][j] = new BoolTilePair(tile, tiles.Contains(tile));
                    }
                }
                return result;
            }
        }

        private DelegateCommand<IPiece?> _updateSelectedPiece;
        public DelegateCommand<IPiece?> UpdateSelectedPiece => _updateSelectedPiece ??= new DelegateCommand<IPiece?>(ExecuteUpdateSelectedPiece);

        void ExecuteUpdateSelectedPiece(IPiece? newSelectedPiece)
        {
            SelectedPiece = newSelectedPiece;
        }

        private DelegateCommand<Tile> _movePiece;
        public DelegateCommand<Tile> MovePiece => _movePiece ??= new DelegateCommand<Tile>(ExecuteMovePiece);

        void ExecuteMovePiece(Tile tile)
        {
            if (SelectedPiece is null) return;

            Board.MovePiece(SelectedPiece.Position, tile);
            RaisePropertyChanged(nameof(TileMoveableArray));
            RaisePropertyChanged(nameof(JaggedArrayBoard));
            RaisePropertyChanged(nameof(Board));
            SelectedPiece = null;
        }
    }

    public class BoolTilePair
    {
        public bool Boolean { get; set; }
        public Tile Tile { get; set; }

        public BoolTilePair(Tile tile, bool boolean)
        {
            Tile = tile;
            Boolean = boolean;
        }
    }
}
