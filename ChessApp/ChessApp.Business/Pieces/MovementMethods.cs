using ChessApp.Business.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessApp.Business.Pieces
{
    /// <summary>
    /// Helpful methods related to piece movement used throughout multiple pieces
    /// </summary>
    public static class MovementMethods
    {
        /// <summary>
        /// Bishop movement
        /// </summary>
        /// <param name="board">Board the piece is on</param>
        /// <param name="position">Position of the piece on the board</param>
        /// <returns>Enumerable of tiles the piece can move to</returns>
        public static IEnumerable<Tile> DiagonalTileMovement(ChessBoard board, Tile position)
        {
            var tiles = new List<Tile>();

            // Loop four times for the four diagonals
            for (int i = -1; i < 2; i += 2)
            {
                for (int j = -1; j < 2; j += 2)
                {
                    var checkingTile = position;
                    while (!board.TileIsOccupied(Tile.Move(checkingTile, j, i)) && Tile.Move(checkingTile, j, i).IsOnBoard(board))
                    {
                        tiles.Add(Tile.Move(checkingTile, j, i));
                        checkingTile = Tile.Move(checkingTile, j, i);
                    }
                    if (board.TileIsOccupied(Tile.Move(checkingTile, j, i)))
                    {
                        tiles.Add(Tile.Move(checkingTile, j, i));
                    }
                }
            }

            return tiles;
        }

        /// <summary>
        /// Rook movement
        /// </summary>
        /// <param name="board">Board the piece is on</param>
        /// <param name="position">Position of the piece on the board</param>
        /// <returns>Enumerable of tiles the piece can move to</returns>
        public static IEnumerable<Tile> HorizontalTileMovement(ChessBoard board, Tile position)
        {
            var tiles = new List<Tile>();

            for (int i = -1; i < 2; i += 2)
            {
                var checkingTile = position;
                while (!board.TileIsOccupied(Tile.Move(checkingTile, 0, i)) && Tile.Move(checkingTile, 0, i).IsOnBoard(board))
                {
                    tiles.Add(Tile.Move(checkingTile, 0, i));
                    checkingTile = Tile.Move(checkingTile, 0, i);
                }
                if (board.TileIsOccupied(Tile.Move(checkingTile, 0, i)))
                {
                    tiles.Add(Tile.Move(checkingTile, 0, i));
                }


                checkingTile = position;
                while (!board.TileIsOccupied(Tile.Move(checkingTile, i, 0)) && Tile.Move(checkingTile, i, 0).IsOnBoard(board))
                {
                    tiles.Add(Tile.Move(checkingTile, i, 0));
                    checkingTile = Tile.Move(checkingTile, i, 0);
                }
                if (board.TileIsOccupied(Tile.Move(checkingTile, i, 0)))
                {
                    tiles.Add(Tile.Move(checkingTile, i, 0 ));
                }
            }

            return tiles;
        }

        /// <summary>
        /// Returns a list of moveable tiles where no tile has a piece of the same colour
        /// </summary>
        public static IEnumerable<Tile> FilteredTilesWhereOppositeColour(IEnumerable<Tile> tiles, IPiece piece, ChessBoard board)
        {
            List<Tile> newTiles = new();
            // Adding attacking tiles if they have an opposite colour piece
            foreach (Tile tile in tiles)
            {
                if (board[tile]?.Colour != piece.Colour)
                {
                    newTiles.Add(tile);
                }
            }

            return newTiles;
        }

        public static IEnumerable<StandardMove> ConvertToStandardMoves(IPiece piece, IEnumerable<Tile> tiles, ChessBoard board)
        {
            return tiles.Select(tile => new StandardMove(piece.Position, tile));
        }
    }
}
