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

        public ChessBoard(ChessBoard board)
        {
            int width = board.Board.GetLength(0);
            int height = board.Board.GetLength(1);
            _board = new IPiece?[width, height];

            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    _board[w, h] = board.Board[w, h]?.Clone();
                }
            }
            Rows = board.Board.GetLength(0);
            Columns = board.Board.GetLength(1);
        }

        public bool WhiteCanCastleKingSide => CanCastle(1, 1);
        public bool WhiteCanCastleQueenSide => CanCastle(1, -1);
        public bool BlackCanCastleKingSide => CanCastle(0, 1);
        public bool BlackCanCastleQueenSide => CanCastle(0, -1);

        public int Rows { get; set; }

        public int Columns { get; set; }

        public List<(Tile Tile, int Timer)> EnPassantSquares { get; set; } = new();

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

        /// <summary>
        /// Lowers the timer of each enpassant square by 1 and gets rid of squares with a timer of 0
        /// </summary>
        public void TickEnPassantSquares()
        {
            EnPassantSquares = EnPassantSquares
                .Select(square => square with { Timer = square.Timer - 1 })
                .Where(square => square.Timer > 0)
                .ToList();
        }

        public bool CanCastle(int player, int direction)
        {
            var king = GetKing(this, player);
            var rook = GetRookOnSideOfKing(this, player, direction);

            if (rook is null || king.Moves != 0 || rook.Moves != 0 || PlayerIsChecked(this, player))
            {
                return false;
            }

            for (int i = king.Position.Column + direction; i != rook.Position.Column; i += direction)
            {
                var tile = new Tile(king.Position.Row, i);
                if (TileIsOccupied(tile)
                    || TileIsAttackedByColour(tile, Math.Abs(player - 1)))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool PlayerIsChecked(ChessBoard board, int player)
        {
            var kingPos = GetKingLocation(board, player);
            foreach (IPiece? piece in board.Board)
            {
                if (piece is null)
                    continue;

                if ((piece.IsWhite ? 1 : 0) == player)
                    continue;

                if (piece.GetAttackedTiles(board).Contains(kingPos))
                    return true;
            }
            return false;
        }

        public bool TileIsAttackedByColour(Tile tile, int attackingPlayer)
        {
            foreach (var piece in Board)
            {
                if (piece is null) continue;

                if ((piece.IsWhite ? 1 : 0) != attackingPlayer) continue;

                if (piece.GetAttackedTiles(this).Contains(tile))
                    return true;
            }

            return false;
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

        public static ChessBoard SetMove(ChessBoard board, IMove move)
        {
            ChessBoard newBoard = new(board);
            move.DoMove(newBoard);
            return newBoard;
        }

        public static Tile GetKingLocation(ChessBoard board, int player)
        {
            foreach (IPiece? piece in board.Board)
            {
                if (piece is King && (piece.IsWhite ? 1 : 0) == player)
                {
                    return piece.Position;
                }
            }
            throw new Exception("There is no king of the specified colour on this board");
        }

        public static King GetKing(ChessBoard board, int player)
        {
            foreach (IPiece? piece in board.Board)
            {
                if (piece is King king && (piece.IsWhite ? 1 : 0) == player)
                {
                    return king;
                }
            }
            throw new Exception("There is no king of the specified colour on this board");
        }

        /// <summary>
        /// Finds the rook on a specifed side on the same row of a player's king
        /// </summary>
        /// <param name="board">Board to search</param>
        /// <param name="player">Player whose rook</param>
        /// <param name="direction">Direction to search</param>
        /// <returns>Rook if piece exists else null</returns>
        public static Rook? GetRookOnSideOfKing(ChessBoard board, int player, int direction)
        {
            Tile kingPos = GetKingLocation(board, player);
            for (int i = kingPos.Column; 0 <= i && i < board.Columns; i += direction)
            {
                if (board.Board[kingPos.Row, i] is Rook rook)
                {
                    return rook;
                }
            }
            return null;
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
