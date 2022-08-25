using ChessApp.Business.Moves;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    /// <summary>
    /// Rook piece class
    /// </summary>
    public class Rook : BindableBase, IPiece
    {
        public int Moves { get; set; } = 0;
        public PieceColour Colour { get; }
        public Tile Position { get; set; }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{Sprites.PieceSpriteName}\{(Colour == PieceColour.White ? 'w' : 'b')}R.svg");

        public char Character => Colour == PieceColour.White ? 'R' : 'r';

        public Rook(PieceColour colour, Tile position)
        {
            Colour = colour;
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

        public IEnumerable<StandardMove> GetStandardMoves(ChessBoard board)
        {
            var tiles = MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board),
                this, board);
            return MovementMethods.ConvertToStandardMoves(this, tiles, board);
        }

        public IEnumerable<IMove> GetMoves(ChessBoard board)
        {
            return GetStandardMoves(board);
        }
    }
}
