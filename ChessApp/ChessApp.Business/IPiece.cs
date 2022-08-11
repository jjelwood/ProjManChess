using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business
{
    public interface IPiece
    {
        public int Moves { get; set; }
        public bool IsWhite { get; }
        public string ImagePath { get; }
        public char Character { get; }
        public Tile Position { get; set; }
        public IPiece Clone();
        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board);
        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board);
    }
}
