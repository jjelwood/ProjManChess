using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    /// <summary>
    /// Move class that represents an en-passant capture
    /// </summary>
    public class EnPassantMove : IMove
    {
        public EnPassantMove(Tile from, Tile to, Tile capturingTile, PieceColour capturingColour)
        {
            Tile = to;
            From = from;
            To = to;
            CapturingTile = capturingTile;
            CapturingColour = capturingColour;
            MoveName = $"{From}{To}x{CapturingTile}";
        }

        public string MoveName { get; set; }
        public PieceColour CapturingColour { get; }
        public Tile From { get; }
        public Tile To { get; }
        public Tile CapturingTile { get; }
        public Tile Tile { get; }

        public void DoMove(ChessBoard board)
        {
            board.MovePiece(From, To);
            board[CapturingTile] = null;
        }
    }
}
