using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    /// <summary>
    /// Move class that represents a double pawn push
    /// </summary>
    public class DoublePawnMove : StandardMove
    {
        public DoublePawnMove(Tile from, Tile to) : base(from, to)
        {
        }

        public override void DoMove(ChessBoard board)
        {
            base.DoMove(board);
            board.EnPassantSquares.Add(new(new((From.Row + To.Row) / 2, From.Column), 2));
        }
    }
}
