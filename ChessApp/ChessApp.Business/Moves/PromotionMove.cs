using ChessApp.Business.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    public class PromotionMove<T> : StandardMove
    {
        public PromotionMove(Tile from, Tile to) : base(from, to)
        {
            MoveName += typeof(T).Name switch
            {
                nameof(Queen) => 'q',
                nameof(Bishop) => 'b',
                nameof(Rook) => 'r',
                nameof(Knight) => 'n',
                _ => throw new Exception("Cannot promote to this piece"),
            };
        }

        public override void DoMove(ChessBoard board)
        {
            base.DoMove(board);

            if (board[To] is not IPiece oldPiece)
            {
                return;
            }

            IPiece newPiece = typeof(T).Name switch
            {
                nameof(Queen) => new Queen(oldPiece.Colour, To),
                nameof(Bishop) => new Bishop(oldPiece.Colour, To),
                nameof(Rook) => new Rook(oldPiece.Colour, To),
                nameof(Knight) => new Knight(oldPiece.Colour, To),
                _ => throw new Exception("Cannot promote to this piece"),
            };

            board[To] = newPiece;
        }
    }
}
