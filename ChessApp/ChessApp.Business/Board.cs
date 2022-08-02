using ChessApp.Business.Pieces;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public class ChessBoard : BindableBase
    {
        private IPiece?[,] _board;

        public ChessBoard(IPiece?[,] board)
        {
            _board = board;
            Rows = board.GetLength(0);
            Columns = board.GetLength(1);
        }

        public int Rows { get; set; }

        public int Columns { get; set; }

        /// <summary>
        /// Intialises a chess board
        /// </summary>
        /// <param name="FEN">FEN representation of the board</param>
        public ChessBoard(string FEN, int rows, int columns)
        {
            IPiece?[,] board = new IPiece?[rows, columns];
            Rows = rows;
            Columns = columns;
            var FENRows = FEN.Split('/');
            for (int i = 0; i < rows; i++)
            {
                int j = 0;
                foreach (char c in FENRows[i]) 
                { 
                    if (char.IsNumber(c))
                    {
                        j += (int)char.GetNumericValue(c);
                    }
                    else
                    {
                        board[i, j] = GetPiece(c, new(i, j));
                        j++;
                    }
                }
            }
            _board = board;
        }

        public static IPiece GetPiece(char pieceChar, Tile position)
        {
            var isWhite = char.IsUpper(pieceChar);

            return char.ToLower(pieceChar) switch
            {
                'p' => new Pawn(isWhite, position),
                'k' => new King(isWhite, position),
                'n' => new Knight(isWhite, position),
                'b' => new Bishop(isWhite, position),
                'q' => new Queen(isWhite, position),
                'r' => new Rook(isWhite, position),
                _ => throw new ArgumentException("Invalid piece", nameof(pieceChar)),
            };
        }

        public IPiece?[,] Board
        {
            get { return _board; }
            set { SetProperty(ref _board, value); }
        }

        /// <summary>
        /// Method which returns whether a given tile has a piece in it
        /// </summary>
        /// <param name="tile">Tile to check</param>
        /// <returns>Whether tile has a piece. False if tile not on board</returns>
        public bool TileIsOccupied(Tile tile)
        {
            if (!tile.IsOnBoard(this))
                return false;

            return Board[tile.Row, tile.Column] is not null;
        }

        /// <summary>
        /// Method which returns whether a given tile has a piece in it
        /// </summary>
        /// <param name="column">Column of tile to check</param>
        /// <param name="row">Row of tile to check</param>
        /// <returns>Whether tile has a piece. False if tile not on board</returns>
        public bool TileIsOccupied(int row, int column)
        {
            if (column < 0 || column > Columns - 1 ||
                row < 0 || row > Rows - 1)
                return false;

            return Board[row, column] is not null;
        }

        /// <summary>
        /// Method which returns if a given tile has a white piece in it
        /// </summary>
        /// <param name="tile">Tile to check</param>
        /// <returns>Whether tile has a white piece. Null if tile not on board or empty</returns>
        public bool? TileIsWhite(Tile tile)
        {
            if (!tile.IsOnBoard(this))
                return null;

            if (Board[tile.Row, tile.Column] is null)
                return null;

            return Board[tile.Row, tile.Column]!.IsWhite;
        }

        /// <summary>
        /// Method which returns if a given tile has a white piece in it
        /// </summary>
        /// <param name="row">Row of tile to check</param>
        /// <param name="column">Column of tile to check</param>
        /// <returns>Whether tile has a white piece. Null if tile not on board or empty</returns>
        public bool? TileIsWhite(int row, int column)
        {
            if (column < 0 || column > Columns - 1 ||
                row < 0 || row > Rows - 1)
                return null;

            if (Board[row, column] is null)
                return null;

            return Board[row, column]!.IsWhite;
        }

        public IPiece?[][] GetJaggedArray()
        {
            IPiece?[][] toReturn = new IPiece?[Board.GetLength(0)][];
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                IPiece?[] row = new IPiece?[Board.GetLength(1)];
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    row[j] = Board[i, j];
                }
                toReturn[i] = row;
            }
            return toReturn;
        }

        public void MovePiece(Tile from, Tile to)
        {
            if (!TileIsOccupied(from))
            {
                throw new ArgumentException("No piece at that location", nameof(from));
            }

            this[from]!.Moves++;
            this[from]!.Position = to;
            this[to] = this[from];
            this[from] = null;
        }

        public static string GetMoveableTilesRepresentation(IPiece piece, ChessBoard board, Tile tile)
        {
            var moves = piece.GetMoveableTiles(board);
            string toReturn = "";
            for (int i = 0; i < board.Board.GetLength(0); i++)
            {
                for (int j = 0; j < board.Board.GetLength(1); j++)
                {
                    if (moves.Contains(new Tile(i, j))) {
                        toReturn += 'O';
                    }
                    else if (new Tile(i, j) == tile)
                    {
                        toReturn += piece.Character;
                    }
                    else
                    {
                        toReturn += ' ';
                    }
                }
                toReturn += '\n';
            }
            return toReturn;
        }

        public IPiece? this[Tile tile]
        {
            get { return Board[tile.Row, tile.Column]; }
            set { Board[tile.Row, tile.Column] = value; }
        }

        public override string ToString()
        {
            string toReturn = "";
            for (int i = 0; i < Board.GetLength(0); i++)
            {
                for (int j = 0; j < Board.GetLength(1); j++)
                {
                    char piece = Board[i, j]?.Character ?? ' ';
                    toReturn += piece;
                }
                toReturn += '\n';
            }
            return toReturn;
        }
    }
}
