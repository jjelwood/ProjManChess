using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public interface IMove
    {
        /// <summary>
        /// Does the move on the passed board
        /// </summary>
        /// <param name="board">The board to do the move on</param>
        public void DoMove(ChessBoard board);

        /// <summary>
        /// The tile associated with the move. This is the tile to be clicked on to do the move
        /// </summary>
        public Tile Tile { get; }
    }
}
