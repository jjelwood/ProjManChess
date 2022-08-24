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
    public class Pawn : BindableBase, IPiece
    {
        public Pawn(PieceColour colour, Tile position)
        {
            Colour = colour;
            Direction = Colour == PieceColour.White ? -1 : 1;
            Position = position;
        }

        public PieceColour Colour { get; }
        public int Direction { get; }
        public Tile Position { get; set; }

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{Sprites.PieceSpriteName}\{(Colour == PieceColour.White ? 'w' : 'b')}P.svg");

        private int _moves;
        public int Moves
        {
            get { return _moves; }
            set { SetProperty(ref _moves, value); }
        }

        public char Character => Colour == PieceColour.White ? 'P' : 'p';

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
            
            // Adding tile directly in front if not occupied
            if (!board.TileIsOccupied(Tile.Move(Position, Direction, 0)))
            {
                tiles.Add(Tile.Move(Position, Direction, 0));
            }

            // Adding attacking tiles if they have an opposite colour piece
            foreach (Tile tile in GetAttackedTiles(board))
            {
                if (board[tile]?.Colour != Colour && board[tile] is not null)
                {
                    tiles.Add(tile);
                }
            }

            // Makes sure tiles added are on the board
            return tiles.Where(tile => tile.IsOnBoard(board));
        }

        public IEnumerable<DoublePawnMove> GetDoublePawnMoves(ChessBoard board)
        {
            var moves = new List<DoublePawnMove>();
            if (Moves == 0 &&
                !board.TileIsOccupied(Tile.Move(Position, Direction, 0)) &&
                !board.TileIsOccupied(Tile.Move(Position, Direction * 2, 0)))
            {
                moves.Add(new(Position, Tile.Move(Position, Direction * 2, 0)));
            }
            return moves;
        }

        public IEnumerable<EnPassantMove> GetEnPassantMoves(ChessBoard board)
        {
            if (!board.EnPassantSquares.Any())
            {
                return Enumerable.Empty<EnPassantMove>();
            }
            var attackingTiles = GetAttackedTiles(board);
            var result = new List<EnPassantMove>();
            foreach (var square in board.EnPassantSquares)
            {
                if (attackingTiles.Contains(square.Tile))
                {
                    result.Add(new(Position, square.Tile, Tile.Move(square.Tile, -Direction, 0)));
                }
            }
            return result;
        }

        public IEnumerable<IMove> GetMoves(ChessBoard board)
        {
            return MovementMethods.GetStandardMoves(this, board)
                .Union(GetEnPassantMoves(board))
                .Union(GetDoublePawnMoves(board));
        }
    }
}
