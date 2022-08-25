using ChessApp.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    /// <summary>
    /// Move class that represents a standard move, where a piece moves from one square to another
    /// </summary>
    public class StandardMove : IMove
    {
        public StandardMove(Tile from, Tile to)
        {
            From = from;
            To = to;
            Tile = to;
            MoveName = $"{from}{to}";
        }

        public string MoveName { get; set; }
        public Tile From { get; set; }
        public Tile To { get; set; }

        public Tile Tile { get; }
        public virtual void DoMove(ChessBoard board)
        {
            board[From]!.Moves++;
            board[From]!.Position = To;
            board[To] = board[From];
            board[From] = null;
        }
    }
}
