using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    public class Queen : BindableBase, IPiece
    {
        public int Moves {get; set;} = 0;
        public bool IsWhite { get; }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{PieceSprites.PieceSpriteName}\{(IsWhite ? 'w' : 'b')}Q.svg");
        public char Character => IsWhite ? 'Q' : 'q';

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public Tile Position { get; set; }

        public Queen(bool isWhite, Tile position)
        {
            IsWhite = isWhite;
            Position = position;
        }

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            return MovementMethods.DiagonalTileMovement(board, Position).
                Union(MovementMethods.HorizontalTileMovement(board, Position));
        }

        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board)
        {
            return MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board),
                this, board);
        }
    }
}
