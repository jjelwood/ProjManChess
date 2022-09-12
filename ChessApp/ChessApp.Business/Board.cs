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
    /// <summary>
    /// Class representing a chess board
    /// </summary>
    public class ChessBoard
    {
        private IPiece?[,] _board;

        public const string StartPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";

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

        public bool WhiteCanCastleKingSide => CanCastle(PieceColour.White, 1);
        public bool WhiteCanCastleQueenSide => CanCastle(PieceColour.White, -1);
        public bool BlackCanCastleKingSide => CanCastle(PieceColour.Black, 1);
        public bool BlackCanCastleQueenSide => CanCastle(PieceColour.Black, -1);

        public int Rows { get; set; }

        public int Columns { get; set; }

        public List<(Tile Tile, int Timer, PieceColour Colour)> EnPassantSquares { get; set; } = new();

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
                        board[i, j] = GetPiece(c, new(i, j), Rows);
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

        public bool CanCastle(PieceColour player, int direction)
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
                    || TileIsAttackedByColour(tile, player.Opposite()))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool PlayerIsChecked(ChessBoard board, PieceColour player)
        {
            var kingPos = GetKingLocation(board, player);
            foreach (IPiece? piece in board.Board)
            {
                if (piece is null)
                    continue;

                if (piece.Colour == player)
                    continue;

                if (piece.GetAttackedTiles(board).Contains(kingPos))
                    return true;
            }
            return false;
        }

        public bool TileIsAttackedByColour(Tile tile, PieceColour attackingPlayer)
        {
            foreach (var piece in Board)
            {
                if (piece is null) continue;

                if (piece.Colour != attackingPlayer) continue;

                if (piece.GetAttackedTiles(this).Contains(tile))
                    return true;
            }

            return false;
        }

        public static IPiece GetPiece(char pieceChar, Tile position, int rowsOnBoard)
        {
            var colour = char.IsUpper(pieceChar) ? PieceColour.White : PieceColour.Black;

            return char.ToLower(pieceChar) switch
            {
                'p' => new Pawn(colour, position, rowsOnBoard),
                'k' => new King(colour, position),
                'n' => new Knight(colour, position),
                'b' => new Bishop(colour, position),
                'q' => new Queen(colour, position),
                'r' => new Rook(colour, position),
                _ => throw new ArgumentException("Invalid piece", nameof(pieceChar)),
            };
        }

        public IPiece?[,] Board
        {
            get { return _board; }
            set { _board = value; }
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

            return Board[tile.Row, tile.Column]!.Colour == PieceColour.White;
        }

        /// <summary>
        /// Moves a piece from one tile to another without checking if the move is valid
        /// </summary>
        /// <param name="from">Tile the piece is being moved from</param>
        /// <param name="to">Tile the piece is being moved to</param>
        public void MovePiece(Tile from, Tile to)
        {
            this[from]!.Moves++;
            this[from]!.Position = to;
            this[to] = this[from];
            this[from] = null;
        }

        /// <summary>
        /// Returns a copy of the board with a move played on it
        /// </summary>
        /// <param name="board">Board the move is being played on</param>
        /// <param name="move">Move to be played</param>
        /// <returns>Copy of the original board with a move played on it</returns>
        public static ChessBoard SetMove(ChessBoard board, IMove move)
        {
            ChessBoard newBoard = new(board);
            move.DoMove(newBoard);
            return newBoard;
        }

        /// <summary>
        /// Gets the king of the player on the board
        /// </summary>
        /// <param name="board">The board to search</param>
        /// <param name="player">The player whose king to look for</param>
        /// <returns>The given player's king</returns>
        /// <exception cref="Exception">No kinf found on the board</exception>
        public static King GetKing(ChessBoard board, PieceColour player)
        {
            foreach (IPiece? piece in board.Board)
            {
                if (piece is King king && piece.Colour == player)
                {
                    return king;
                }
            }
            throw new Exception("There is no king of the specified colour on this board");
        }

        /// <summary>
        /// Returns the tile that the player's king is on
        /// </summary>
        /// <param name="board">The board to search</param>
        /// <param name="player">The player whose king to look for</param>
        /// <returns>The location of the player's king</returns>
        /// <exception cref="Exception">No king found on the board</exception>
        public static Tile GetKingLocation(ChessBoard board, PieceColour player)
        {
            return GetKing(board, player).Position;
        }

        /// <summary>
        /// Finds the rook on a specifed side on the same row of a player's king
        /// </summary>
        /// <param name="board">Board to search</param>
        /// <param name="player">Player whose rook</param>
        /// <param name="direction">Direction to search</param>
        /// <returns>Rook if piece exists else null</returns>
        public static Rook? GetRookOnSideOfKing(ChessBoard board, PieceColour player, int direction)
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

        public IPiece? this[Tile tile]
        {
            get { return Board[tile.Row, tile.Column]; }
            set { Board[tile.Row, tile.Column] = value; }
        }

        /// <summary>
        /// Gets a string representation of the board
        /// </summary>
        /// <returns>A string representation of the board</returns>
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
