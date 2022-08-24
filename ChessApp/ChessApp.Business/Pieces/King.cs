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
    public class King : BindableBase, IPiece
    {
        public King(bool isWhite, Tile position)
        {
            IsWhite = isWhite;
            Position = position;
        }

        public int Moves { get; set; }

        public bool IsWhite { get; }

        public Tile Position { get; set; }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{Sprites.PieceSpriteName}\{(IsWhite ? 'w' : 'b')}K.svg");

        public char Character => IsWhite ? 'K' : 'k';

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            var tiles = new List<Tile>();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        tiles.Add(Tile.Move(Position, i, j));
                    }
                }
            }
            return tiles.Where(tile => tile.IsOnBoard(board));
        }

        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board)
        {
            return MovementMethods.FilteredTilesWhereOppositeColour(GetAttackedTiles(board), this, board);
        }

        public IEnumerable<IMove> GetMoves(ChessBoard board)
        {
            List<IMove> moves = MovementMethods.GetStandardMoves(this, board).ToList();
            for (int i = -1; i <= 1; i += 2)
            {
                if (board.CanCastle(IsWhite ? 1 : 0, i))
                {
                    moves.Add(new CastlingMove(IsWhite ? 1 : 0, i, board));
                }
            }
            return moves;
        }
    }
}
