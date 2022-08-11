using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ChessApp.Business
{
    public class Game
    {
        private readonly ChessBoard _board;

        public Game(ChessBoard board)
        {
            _board = board;
        }

        public ChessBoard Board
        {
            get { return _board; }
        }

        private int _moves = 0;

        public int Moves
        {
            get { return _moves; }
            set { _moves = value; }
        }

        public int PlayerToMove => (Moves + 1) % 2;

        public static bool MoveIsValid(ChessBoard board, int playerToMove, Move move)
        {
            // If no piece a from location then move is invalid
            if (board.TileIsOccupied(move.From))
            {
                return false;
            }

            IPiece piece = board[move.From]!;
            int player = piece.IsWhite ? 1 : 0;

            // Move not valid if it's not in the moveable tiles of the piece
            // If it's not the right colour
            // Or if you move into check
            
            if (player != playerToMove
                || !piece.GetMoveableTiles(board).Contains(move.To)
                || PlayerIsCheckedAfterMove(board, player, move))
            {
                return false;
            }

            return true;
        }

        public static bool PlayerIsChecked(ChessBoard board, int player)
        {
            var kingPos = ChessBoard.GetKingLocation(board, player);
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

        public static bool PlayerIsCheckedAfterMove(ChessBoard board, int player, Move move)
        {
            var newBoard = ChessBoard.SetMove(board, move);
            Trace.WriteLine(move);
            if (PlayerIsChecked(newBoard, player))
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<Move> AllMovesForPlayer(ChessBoard board, int player)
        {
            List<Move> result = new();
            foreach (var piece in board.Board)
            {
                if (piece is null)
                    continue;

                foreach (Tile tile in piece.GetMoveableTiles(board))
                {
                    var move = new Move(piece.Position, tile);
                    if (MoveIsValid(board, player, move))
                    {
                        result.Add(move);
                    }
                }
            }
            return result;
        }

        public void MovePiece(Move move)
        {
            if (!MoveIsValid(Board, PlayerToMove, move))
            {
                throw new ArgumentException("Move invalid");
            }

            Board.MovePiece(move);
            Moves++;
        }
    }
}
