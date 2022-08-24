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
    /// <summary>
    /// Bishop piece
    /// </summary>
    public class Bishop : BindableBase, IPiece
    {
        public Bishop(PieceColour colour, Tile position)
        {
            Colour = colour;
            Position = position;
        }

        public int Moves { get; set; } = 0;

        public PieceColour Colour { get; }

        public Tile Position { get; set; }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{Sprites.PieceSpriteName}\{(Colour == PieceColour.White ? 'w' : 'b')}B.svg");

        public char Character => Colour == PieceColour.White ? 'B' : 'b';

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

        public IEnumerable<IMove> GetMoves(ChessBoard board)
        {
            return MovementMethods.GetStandardMoves(this, board);
        }
    }
}
