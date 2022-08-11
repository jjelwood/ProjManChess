using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    public class Pawn : BindableBase, IPiece
    {
        public Pawn(bool isWhite, Tile position)
        {
            IsWhite = isWhite;
            Direction = IsWhite ? -1 : 1;
            Position = position;
        }

        public bool IsWhite { get; }
        public int Direction { get; }
        public Tile Position { get; set; }

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\anarcandy\{(IsWhite ? 'w' : 'b')}P.svg");

        private int _moves;
        public int Moves
        {
            get { return _moves; }
            set { SetProperty(ref _moves, value); }
        }

        public char Character => IsWhite ? 'P' : 'p';

        public IEnumerable<Tile> GetAttackedTiles(ChessBoard board)
        {
            var tiles = new List<Tile>();
            // Iterates through 1 and -1, checks to the left and right of pawn
            for (int i = -1; i < 2; i += 2)
            {
                // Adds tile in correct direction
                tiles.Add(Tile.Move(Position, Direction, i));
            }
            // Makes sure tiles added are on the board
            return tiles.Where(tile => tile.IsOnBoard(board));
        }

        public IEnumerable<Tile> GetMoveableTiles(ChessBoard board)
        {
            var tiles = new List<Tile>();
            if (Moves == 0 && 
                !board.TileIsOccupied(Tile.Move(Position, Direction, 0)) &&
                !board.TileIsOccupied(Tile.Move(Position, Direction * 2, 0)))
            {
                tiles.Add(Tile.Move(Position, Direction * 2, 0));
            }
            
            // Adding tile directly in front if not occupied
            if (!board.TileIsOccupied(Tile.Move(Position, Direction, 0)))
            {
                tiles.Add(Tile.Move(Position, Direction, 0));
            }

            // Adding attacking tiles if they have an opposite colour piece
            foreach (Tile tile in GetAttackedTiles(board))
            {
                var tileIsWhite = board.TileIsWhite(tile);
                if (tileIsWhite is null)
                {
                    continue;
                }
                if (tileIsWhite != this.IsWhite)
                {
                    tiles.Add(tile);
                }
            }

            // Makes sure tiles added are on the board
            return tiles.Where(tile => tile.IsOnBoard(board));
        }
    }
}
