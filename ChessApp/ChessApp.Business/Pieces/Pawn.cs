using ChessApp.Business.Moves;
using ChessApp.Core;
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
    /// Pawn piece class
    /// </summary>
    public class Pawn : BindableBase, IPiece
    {
        public Pawn(PieceColour colour, Tile position, int rowsOnBoard)
        {
            Colour = colour;
            Direction = Colour == PieceColour.White ? -1 : 1;
            PromotionRank = Colour == PieceColour.White ? 0 : rowsOnBoard - 1;
            Position = position;
        }

        public PieceColour Colour { get; }
        public Tile Position { get; set; }

        /// <summary>
        /// The direction the pawn moves in
        /// </summary>
        public int Direction { get; }

        /// <summary>
        /// The rank the pawn must reach to promote
        /// </summary>
        public int PromotionRank { get; }

        public IPiece Clone()
        {
            return (IPiece)MemberwiseClone();
        }

        public string ImagePath => Path.Combine(Directory.GetCurrentDirectory(), @$"..\..\..\..\ChessApp.Assets\Pieces\{Config.PieceSpriteName}\{(Colour == PieceColour.White ? 'w' : 'b')}P.svg");

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

        public IEnumerable<StandardMove> GetStandardAndPromotionMoves(ChessBoard board)
        {
            var moves = new List<StandardMove>();

            // Adding tile directly in front if not occupied
            var tileInFront = Tile.Move(Position, Direction, 0);
            if (tileInFront.IsOnBoard(board)
                && !board.TileIsOccupied(tileInFront))
            {
                if (Direction + Position.Row != PromotionRank)
                {
                    moves.Add(new(Position, tileInFront));
                }
                else
                {
                    AddPromotionMovesToList(moves, tileInFront);
                }
            }

            // Adding attacking tiles if they have an opposite colour piece
            foreach (Tile tile in GetAttackedTiles(board))
            {
                if (board[tile]?.Colour != Colour && board[tile] is not null)
                {
                    if (Direction + Position.Row != PromotionRank)
                    {
                        moves.Add(new(Position, tile));
                    }
                    else
                    {
                        AddPromotionMovesToList(moves, tile);
                    }
                }
            }

            return moves;
        }

        public void AddPromotionMovesToList(List<StandardMove> moves, Tile tile)
        {
            moves.Add(new PromotionMove<Rook>(Position, tile));
            moves.Add(new PromotionMove<Knight>(Position, tile));
            moves.Add(new PromotionMove<Bishop>(Position, tile));
            moves.Add(new PromotionMove<Queen>(Position, tile));
        }

        /// <summary>
        /// Gets any double pawn moves the pawn can make
        /// </summary>
        /// <param name="board">The board the pawn is on</param>
        /// <returns>An IEnumerable containing any double pawn moves the pawn can make</returns>
        public IEnumerable<DoublePawnMove> GetDoublePawnMoves(ChessBoard board)
        {
            var moves = new List<DoublePawnMove>();
            var tileOneInFront = Tile.Move(Position, Direction, 0);
            var tileTwoInFront = Tile.Move(Position, Direction * 2, 0);
            if (Moves == 0 &&
                tileOneInFront.IsOnBoard(board) && 
                tileTwoInFront.IsOnBoard(board) &&
                !board.TileIsOccupied(tileOneInFront) &&
                !board.TileIsOccupied(Tile.Move(Position, Direction * 2, 0)))
            {
                moves.Add(new(Position, Tile.Move(Position, Direction * 2, 0)));
            }
            return moves;
        }

        /// <summary>
        /// Gets any en-passant moves the piece can make
        /// </summary>
        /// <param name="board">The board the pawn is on</param>
        /// <returns>Any en passant moves the pawn can make</returns>
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
                    var attackingTile = Tile.Move(square.Tile, -Direction, 0);
                    if (board[attackingTile] is not IPiece attackingPiece)
                    {
                        continue;
                    }
                    result.Add(new(Position, square.Tile, attackingTile, attackingPiece.Colour));
                }
            }
            return result;
        }

        public IEnumerable<IMove> GetMoves(ChessBoard board)
        {
            return ((IEnumerable<IMove>)GetStandardAndPromotionMoves(board))
                .Union(GetEnPassantMoves(board))
                .Union(GetDoublePawnMoves(board));
        }
    }
}
