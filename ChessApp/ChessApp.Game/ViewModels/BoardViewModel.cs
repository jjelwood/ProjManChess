using ChessApp.Business;
using ChessApp.Business.Pieces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ChessApp.Game.ViewModels
{
    public class BoardViewModel : BindableBase
    {
        public const string boardColour1 = "#f07373";
        public const string boardColour2 = "#ebecba";
        public bool isFlipped = true;

        public BoardViewModel() 
        {
            _game = new(new("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR", 8, 8));
        }

        private Business.Game _game;
        public Business.Game Game
        {
            get { return _game; }
            set { SetProperty(ref _game, value); }
        }

        public ChessBoard Board => Game.Board;

        public IPiece?[][] JaggedArrayBoard
        {
            get
            {
                IPiece?[][] toReturn = new IPiece?[Board.Rows][];
                for (int i = 0; i < Board.Rows; i++)
                {
                    IPiece?[] row = new IPiece?[Board.Columns];
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        var column = isFlipped ? j : Board.Columns - 1 - j;
                        row[column] = Board.Board[i, j];
                    }
                    toReturn[isFlipped ? i : Board.Rows - 1 - i] = row;
                }
                return toReturn;
            }
        }


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
                    var row = isFlipped ? i : Board.Rows - 1 - i;
                    result[row] = new string[Board.Columns];
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        var column = isFlipped ? j : Board.Columns - 1 - j;
                        result[row][column] = i % 2 == 0 ^ j % 2 == 0 ? boardColour1 : boardColour2;
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

                if (SelectedPiece is null)
                    return null;

                var tiles = SelectedPiece.GetMoveableTiles(Board);

                if (tiles is null)
                    return null;

                for (int i = 0; i < Board.Rows; i++)
                {
                    var row = isFlipped ? i : Board.Rows - 1 - i;
                    result[row] = new BoolTilePair[Board.Columns];
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        var column = isFlipped ? j : Board.Columns - 1 - j;
                        var tile = new Tile(i, j);
                        var pos = SelectedPiece.Position;
                        var move = new Move(tile, pos);

                        var moveIsValid = Business.Game.MoveIsValid(Game.Board, Game.PlayerToMove, move);

                        result[row][column] = new BoolTilePair(tile, moveIsValid);
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

            Game.MovePiece(new(tile, SelectedPiece.Position));
            isFlipped = !isFlipped;

            RaisePropertyChanged(nameof(TileMoveableArray));
            RaisePropertyChanged(nameof(JaggedArrayBoard));
            RaisePropertyChanged(nameof(BoardColours));
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
