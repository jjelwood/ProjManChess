using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    public class Rook : BindableBase, IPiece
    {
        public int Moves { get; set; } = 0;
        public bool IsWhite { get; }
        public Tile Position { get; set; }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\anarcandy\{(IsWhite ? 'w' : 'b')}R.svg");

        public char Character => IsWhite ? 'R' : 'r';

        public Rook(bool isWhite, Tile position)
        {
            IsWhite = isWhite;
            Position = position;
        }

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            return MovementMethods.HorizontalTileMovement(board, Position);
        }

        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board)
        {
            return MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board), this, board);
        }
    }
}
