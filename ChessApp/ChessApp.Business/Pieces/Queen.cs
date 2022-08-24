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
        public PieceColour Colour { get; }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{Sprites.PieceSpriteName}\{(Colour == PieceColour.White ? 'w' : 'b')}Q.svg");
        public char Character => Colour == PieceColour.White ? 'Q' : 'q';

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public Tile Position { get; set; }

        public Queen(PieceColour colour, Tile position)
        {
            Colour = colour;
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

        public IEnumerable<IMove> GetMoves(ChessBoard board)
        {
            return MovementMethods.GetStandardMoves(this, board);
        }
    }
}
