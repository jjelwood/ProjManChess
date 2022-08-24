using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    public class EnPassantMove : IMove
    {
        public EnPassantMove(Tile from, Tile to, Tile capturingTile)
        {
            Tile = to;
            From = from;
            To = to;
            CapturingTile = capturingTile;
        }

        public Tile From { get; set; }
        public Tile To { get; set; }
        public Tile CapturingTile { get; set; }
        public Tile Tile { get; }

        public void DoMove(ChessBoard board)
        {
            board.MovePiece(From, To);
            board[CapturingTile] = null;
        }
    }
}
