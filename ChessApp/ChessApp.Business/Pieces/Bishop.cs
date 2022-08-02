using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    public class Bishop : BindableBase, IPiece
    {
        public Bishop(bool isWhite, Tile position)
        {
            IsWhite = isWhite;
            Position = position;
        }

        public int Moves { get; set; } = 0;

        public bool IsWhite { get; }

        public Tile Position { get; set; }

        public char Character => IsWhite ? 'B' : 'b';

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            return MovementMethods.DiagonalTileMovement(board, Position);
        }

        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board)
        {
            return MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board), this, board);
        }
    }
}
