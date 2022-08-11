using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\anarcandy\{(IsWhite ? 'w' : 'b')}B.svg");

        public char Character => IsWhite ? 'B' : 'b';

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

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
