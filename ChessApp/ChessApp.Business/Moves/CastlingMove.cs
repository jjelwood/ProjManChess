using ChessApp.Business.Exceptions;
using ChessApp.Business.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    /// <summary>
    /// IMove implementation for castling
    /// </summary>
    public class CastlingMove : IMove
    {
        /// <summary>
        /// Castling move
        /// </summary>
        /// <param name="player">The colour of the player that's castling</param>
        /// <param name="direction">The direction of the castle</param>
        /// <param name="board">The board being castled on</param>
        public CastlingMove(PieceColour player, int direction, ChessBoard board)
        {
            Player = player;
            Direction = direction;

            _king = ChessBoard.GetKing(board, Player);
            _rook = ChessBoard.GetRookOnSideOfKing(board, Player, Direction);

            Tile = Tile.Move(_king.Position, 0, 2 * direction);
        }


        private readonly King _king;
        private readonly Rook? _rook;

        public int Direction { get; set; }
        public PieceColour Player { get; set; }
        public Tile Tile { get; }

        public void DoMove(ChessBoard board)
        {
            board.MovePiece(_rook!.Position, Tile.Move(_king.Position, 0, Direction));
            board.MovePiece(_king.Position, Tile.Move(_king.Position, 0, 2 * Direction));
        }
    }
}
