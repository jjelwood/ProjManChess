using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public interface IMove
    {
        public void DoMove(ChessBoard board);
        public Tile Tile { get; }
    }
}
