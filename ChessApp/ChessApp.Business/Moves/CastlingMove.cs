using ChessApp.Business.Exceptions;
using ChessApp.Business.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    public class CastlingMove : IMove
    {
        public CastlingMove(int player, int direction, ChessBoard board)
        {
            Player = player;
            Direction = direction;
            _king = ChessBoard.GetKing(board, Player);
            _rook = ChessBoard.GetRookOnSideOfKing(board, Player, Direction);
            Tile = Tile.Move(_king.Position, 0, 2 * direction);
        }

        public int Direction { get; set; }

        private readonly King _king;
        private readonly Rook? _rook;

        public int Player { get; set; }

        public Tile Tile { get; }
        public void DoMove(ChessBoard board)
        {
            board.MovePiece(_rook!.Position, Tile.Move(_king.Position, 0, Direction));
            board.MovePiece(_king.Position, Tile.Move(_king.Position, 0, 2 * Direction));
        }
    }
}
