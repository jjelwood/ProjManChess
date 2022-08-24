using ChessApp.Business;
using ChessApp.Business.Moves;
using ChessApp.Business.Pieces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace ChessApp.Game.ViewModels
{
    public class BoardViewModel : BindableBase
    {
        public string boardColour1 = Sprites.BoardTheme.BoardColourOne;
        public string boardColour2 = Sprites.BoardTheme.BoardColourTwo;
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
            set { SetProperty(ref _selectedPiece, value, () => RaisePropertyChanged(nameof(MoveArray))); }
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

        public MoveVisibiltyPair[][]? MoveArray
        {
            get
            {
                MoveVisibiltyPair[][] result = new MoveVisibiltyPair[Board.Rows][];

                if (SelectedPiece is null)
                    return null;

                var moves = Business.Game.GetValidMovesForPiece(SelectedPiece, Board, Game.PlayerToMove);

                if (moves is null)
                    return null;

                for (int i = 0; i < Board.Rows; i++)
                {
                    result[i] = new MoveVisibiltyPair[Board.Columns];
                    for (int j = 0; j < Board.Columns; j++)
                    {
                        result[i][j] = new(null, Visibility.Collapsed);
                    }
                }

                foreach (var move in moves)
                {
                    var tile = move.Tile;

                    var row = isFlipped ? tile.Row : Board.Rows - 1 - tile.Row;
                    var column = isFlipped ? tile.Column : Board.Rows - 1 - tile.Column;

                    result[row][column].Move = move;
                    result[row][column].Visibility = Visibility.Visible;
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

        private DelegateCommand<IMove> _movePiece;
        public DelegateCommand<IMove> MovePiece => _movePiece ??= new DelegateCommand<IMove>(ExecuteMovePiece);

        void ExecuteMovePiece(IMove move)
        {
            if (SelectedPiece is null) return;

            Game.MovePiece(move);
            isFlipped = !isFlipped;

            RaisePropertyChanged(nameof(MoveArray));
            RaisePropertyChanged(nameof(JaggedArrayBoard));
            RaisePropertyChanged(nameof(BoardColours));
            SelectedPiece = null;
        }
    }

    public class MoveVisibiltyPair
    {
        public MoveVisibiltyPair(IMove? move, Visibility visibilty)
        {
            Move = move;
            Visibility = visibilty;
        }
        public IMove? Move { get; set; }
        public Visibility Visibility { get; set; }
    }
}
