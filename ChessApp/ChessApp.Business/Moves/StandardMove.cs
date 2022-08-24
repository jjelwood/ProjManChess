using ChessApp.Business.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Moves
{
    public class StandardMove : IMove
    {
        public StandardMove(Tile from, Tile to)
        {
            _from = from;
            _to = to;
            Tile = to;
        }

        private Tile _from;

        public Tile From
        {
            get { return _from; }
            set { _from = value; }
        }

        private Tile _to;

        public Tile To
        {
            get { return _to; }
            set { _to = value; }
        }

        public Tile Tile { get; }
        public virtual void DoMove(ChessBoard board)
        {
            if (!board.TileIsOccupied(From))
            {
                throw new MoveInvalidException("No piece at that location");
            }

            board[From]!.Moves++;
            board[From]!.Position = To;
            board[To] = board[From];
            board[From] = null;
        }
    }
}
